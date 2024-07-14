using AscendedZ;
using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public partial class BattleEnemyScene : Node2D
{
    private HBoxContainer _partyMembers;
    private HBoxContainer _enemyMembers;
    private PanelContainer _skillDisplayIcons;
    private BattleSceneObject _battleSceneObject;
    private Button _backToHomeButton, _retryFloorButton, _continueButton, _changePartyButton;
    private CenterContainer _endBox;
    private bool _uiUpdating = false;
    private bool _dungeonCrawlEncounter = false;

    private Label _skillName;
    private TextureRect _skillIcon;
    private HBoxContainer _turnIconContainer;
    private ActionMenu _actionMenu;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("UIUpdated");

        _skillName = this.GetNode<Label>("%SkillName");
        _skillIcon = this.GetNode<TextureRect>("%SkillIcon");
        _skillDisplayIcons = this.GetNode<PanelContainer>("%SkillDisplayIcons");

        _partyMembers = this.GetNode<HBoxContainer>("%PartyPortraits");
        _enemyMembers = this.GetNode<HBoxContainer>("%EnemyContainerBox");
        _turnIconContainer = this.GetNode<HBoxContainer>("%TurnIconContainer");

        _endBox = this.GetNode<CenterContainer>("%EndBox");
        _backToHomeButton = this.GetNode<Button>("%BackToHomeBtn");
        _retryFloorButton = this.GetNode<Button>("%RetryFloorBtn");
        _continueButton = this.GetNode<Button>("%ContinueBtn");
        _changePartyButton = this.GetNode<Button>("%ChangePartyBtn");

        _actionMenu = this.GetNode<ActionMenu>("%ActionMenu");

        AudioStreamPlayer audioPlayer = this.GetNode<AudioStreamPlayer>("MusicPlayer");
        PersistentGameObjects.GameObjectInstance().MusicPlayer.SetStreamPlayer(audioPlayer);

        _backToHomeButton.Pressed += _OnBackToHomeBtnPressed;
        _retryFloorButton.Pressed += _OnRetryFloorBtnPressed;
        _changePartyButton.Pressed += _OnChangePartyBtnPressed;
        _continueButton.Pressed += _OnContinueBtnPressed;
    }

    private void _OnBackToHomeBtnPressed()
    {
        PackedScene mainScreenScene = ResourceLoader.Load<PackedScene>(Scenes.MAIN);
        this.GetTree().Root.AddChild(mainScreenScene.Instantiate());
        this.QueueFree();
    }


    private void _OnRetryFloorBtnPressed()
    {
        SetEndScreenVisibility(false);
        InitializeBattleScene();
    }

    private void _OnContinueBtnPressed()
    {
        if (!_dungeonCrawlEncounter) 
        {
            SetEndScreenVisibility(false);
            PersistentGameObjects.GameObjectInstance().Tier++;
            InitializeBattleScene();
        }
        else
        {
            QueueFree();
        }
    }

    private async void _OnChangePartyBtnPressed()
    {
        var vbox = this.GetNode<VBoxContainer>("%EndVBox");
        vbox.Visible = false;

        var partyChangeScene = ResourceLoader.Load<PackedScene>(Scenes.PARTY_CHANGE).Instantiate<PartyEditScreen>();

        _endBox.AddChild(partyChangeScene);

        partyChangeScene.DisableEmbarkButton();

        await ToSignal(partyChangeScene, "tree_exited");

        vbox.Visible = true;
    }

    public void SetupForNormalEncounter()
    {
        _dungeonCrawlEncounter = false;
        InitializeBattleScene();
    }

    public void SetupForDungeonCrawlEncounter(List<BattlePlayer> players)
    {
        _dungeonCrawlEncounter = true;
        InitializeBattleScene(players);
    }

    private void InitializeBattleScene(List<BattlePlayer> players = null)
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();
        int tier = (_dungeonCrawlEncounter) ? gameObject.TierDC : gameObject.Tier;

        ClearChildrenFromNode(_partyMembers);
        ClearChildrenFromNode(_enemyMembers);

        TextureRect background = this.GetNode<TextureRect>("%Background");
        background.Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetCombatBackground(tier));

        _battleSceneObject = new BattleSceneObject(tier);
        _actionMenu.BattleSceneObject = _battleSceneObject;
        _actionMenu.DungeonCrawling = _dungeonCrawlEncounter;

        _battleSceneObject.InitializeEnemies(tier);

        if(!_dungeonCrawlEncounter)
            players = gameObject.MakeBattlePlayerListFromParty();

        _battleSceneObject.InitializePartyMembers(players);

        _battleSceneObject.UpdateUI += _OnUIUpdate;

        // add players to the scene
        foreach (var member in _battleSceneObject.Players)
        {
            HBoxContainer hBoxContainer = new HBoxContainer() { Alignment = BoxContainer.AlignmentMode.Center };
            var partyBox = ResourceLoader.Load<PackedScene>(Scenes.PARTY_BOX).Instantiate<EntityDisplayBox>();

            _partyMembers.AddChild(hBoxContainer);
            hBoxContainer.AddChild(partyBox);

            partyBox.InstanceEntity(new EntityWrapper() { BattleEntity = member });
            if (member.IsActiveEntity)
                _actionMenu.Reparent(hBoxContainer);
        }

        HashSet<Type> enemyTypes = new HashSet<Type>();
        foreach (var enemy in _battleSceneObject.Enemies)
        {
            var enemyBox = (enemy.IsBoss) 
                ? ResourceLoader.Load<PackedScene>(Scenes.BOSS_BOX).Instantiate()
                : ResourceLoader.Load<PackedScene>(Scenes.ENEMY_BOX).Instantiate();
            
            _enemyMembers.AddChild(enemyBox);
 
            enemyBox.Call("InstanceEntity", new EntityWrapper() { BattleEntity = enemy, IsBoss = enemy.IsBoss });

            if (!enemy.IsBoss)
            {
                // Prevent there from being repeat descriptions of enemy ais.
                if (!enemyTypes.Contains(enemy.GetType()))
                {
                    enemyTypes.Add(enemy.GetType());
                }
            }
                
        }

        // set the turns and prep the b.s.o. for processing battle stuff
        _battleSceneObject.StartBattle();

        UpdateTurnsUsingTurnState(TurnState.PLAYER);

        string dungeonTrack = MusicAssets.GetDungeonTrack(tier);
        bool isBoss = (tier % 10 == 0);
        gameObject.MusicPlayer.PlayMusic(dungeonTrack);
        _actionMenu.CanInput = true;
        
    }

    private void ClearChildrenFromNode(Node node)
    {
        foreach (var child in node.GetChildren())
        {
            node.RemoveChild(child);
            child.QueueFree();
        }
    }

    private async void _OnUIUpdate(object sender, BattleUIUpdate update)
    {
        // handle battle results if any
        if (update.Result != null)
        {
            var result = update.Result;

            // check if we're running from this battle
            if (result.ResultType == BattleResultType.Retreat)
            {
                // ask here
                this.EndBattle(false, true);
                return;
            }

            // display skill icon display
            if (result.SkillUsed != null)
            {
                _skillDisplayIcons.Visible = true;
                ChangeSkillIconRegion(SkillAssets.GetIcon(result.SkillUsed.Icon));
                _skillName.Text = result.SkillUsed.Name;
            }

            if (result.User != null)
            {
                Node userNode = FindBattleEntityNode(result.User);
                BattleEffectWrapper userEffects = new BattleEffectWrapper()
                {
                    IsEntitySkillUser = true,
                    Result = result
                };

                userNode.Call("UpdateBattleEffects", userEffects);
                await ToSignal(userNode, "EffectPlayed");
            }

            if (result.Target != null)
            {
                Node targetNode = FindBattleEntityNode(result.Target);
                BattleEffectWrapper targetNodeEffects = new BattleEffectWrapper() { Result = result };

                targetNode.Call("UpdateBattleEffects", targetNodeEffects);
                await ToSignal(targetNode, "EffectPlayed");
            }

            // slight delay so the skill icon doesn't auto vanish
            await Task.Delay(350);
            ResetSkillIcon();

            _battleSceneObject.ChangeActiveEntity();
        }

        List<Enemy> enemies = _battleSceneObject.Enemies;
        // update HP values on everyone
        for (int i = 0; i < enemies.Count; i++)
        {
            var enemyDisplay = _enemyMembers.GetChild(i);
            var enemy = enemies[i];
            var enemyWrapper = new EntityWrapper() { BattleEntity = enemy, IsBoss = enemy.IsBoss };
            enemyDisplay.Call("UpdateEntityDisplay", enemyWrapper);
        }

        UpdatePlayerDisplay(_battleSceneObject.Players);

        // check if win conditions were met
        if (_battleSceneObject.DidPartyMembersWin())
        {
            this.EndBattle(true);
            return;
        }

        if (_battleSceneObject.DidEnemiesWin())
        {
            this.EndBattle(false);
            return;
        }

        _actionMenu.LoadActiveSkillList();

        bool playerTurn = _battleSceneObject.TurnState == TurnState.PLAYER;
        SetNewTurns(playerTurn);

        _actionMenu.Visible = playerTurn;

        if (_battleSceneObject.PressTurn.TurnEnded)
        {
            _battleSceneObject.PressTurn.TurnEnded = false; // set turns
            _battleSceneObject.ChangeTurnState(); // change turn state
            UpdateTurnsUsingTurnState(_battleSceneObject.TurnState);
        }

        // after we fully display an animation and process a skill
        // then we want to use an enemy skill
        if (_battleSceneObject.TurnState == TurnState.ENEMY)
            _battleSceneObject.DoEnemyMove();
        else
            _actionMenu.CanInput = true;
    }

    private void UpdateTurnsUsingTurnState(TurnState turnState)
    {
        if (turnState == TurnState.PLAYER)
        {
            _battleSceneObject.SetPartyMemberTurns();
            SetNewTurns(true);

            _actionMenu.LoadActiveSkillList();
        }
        else
        {
            _battleSceneObject.SetupEnemyTurns();
            SetNewTurns(false);
        }

        // change our active player display
        // if enemy, no active players
        // if player, 1 active player
        UpdatePlayerDisplay(_battleSceneObject.Players);
    }

    /// <summary>
    /// We use this function multiple times.
    /// 1. After a move to show who the next player is.
    /// 2. At the end of a turn to remove the current player icon.
    /// </summary>
    private void UpdatePlayerDisplay(List<BattlePlayer> players)
    {
        for (int j = 0; j < players.Count; j++)
        {
            var vBoxContainer = _partyMembers.GetChild(j);
            var partyDisplay = vBoxContainer.GetChild(0);

            var playerWrapper = new EntityWrapper() { BattleEntity = players[j] };
            partyDisplay.Call("UpdateEntityDisplay", playerWrapper);

            if (players[j].IsActiveEntity)
                _actionMenu.Reparent(vBoxContainer);
        }
    }

    #region Battle Result Functions
    private Node FindBattleEntityNode(BattleEntity entity)
    {
        Node nodeToFind;

        if (entity.GetType() == typeof(BattlePlayer))
        {
            int pIndex = _battleSceneObject.Players.IndexOf((BattlePlayer)entity);
            nodeToFind = _partyMembers.GetChild(pIndex).GetChild(0);
        }
        else
        {
            int eIndex = _battleSceneObject.Enemies.IndexOf((Enemy)entity);
            nodeToFind = _enemyMembers.GetChild(eIndex);
        }

        return nodeToFind;
    }

    private void ResetSkillIcon()
    {
        _skillDisplayIcons.Visible = false;
        _skillName.Text = String.Empty;
        ChangeSkillIconRegion(new KeyValuePair<int, int>(0, 0));
    }

    private void ChangeSkillIconRegion(KeyValuePair<int, int> coords)
    {
        AtlasTexture atlas = _skillIcon.Texture as AtlasTexture;
        atlas.Region = new Rect2(coords.Key, coords.Value, 32, 32);
    }

    #endregion

    private void SetNewTurns(bool isPlayer)
    {
        // clear all icons to redraw them
        var children = _turnIconContainer.GetChildren();
        foreach (var child in children)
            _turnIconContainer.RemoveChild(child);

        List<int> turns = _battleSceneObject.PressTurn.TurnIcons;
        foreach (int turn in turns)
        {
            var turnIconScene = ResourceLoader.Load<PackedScene>(Scenes.TURN_ICONS).Instantiate();
            _turnIconContainer.AddChild(turnIconScene);
            turnIconScene.Call("SetIconState", isPlayer, turn == 1);
        }
    }

    private async void EndBattle(bool didPlayerWin, bool retreated=false)
    {
        _actionMenu.CanInput = false;
        SetEndScreenVisibility(true);

        Label endLabel = this.GetNode<Label>("%EndOfBattleLabel");

        // heal everyone
        foreach(var member in _battleSceneObject.Players)
        {
            if(!_dungeonCrawlEncounter)
                member.HP = member.MaxHP;

            member.StatusHandler.Clear();
        }

        if (didPlayerWin)
        {
            endLabel.Text = "Encounter Complete!";

            var gameObject = PersistentGameObjects.GameObjectInstance();

            if(gameObject.Tier % 10 == 0)
            {
                gameObject.MusicPlayer.PlayMusic(MusicAssets.BOSS_VICTORY);
                gameObject.MusicPlayer.ResetAllTracksAfterBoss();
            }

            if (_dungeonCrawlEncounter)
            {
                ChangeEndScreenVisibilityOnly(false);

                var rewardScene = ResourceLoader.Load<PackedScene>(Scenes.REWARDS).Instantiate<RewardScreen>();
                this.GetTree().Root.AddChild(rewardScene);
                rewardScene.InitializeDungeonCrawlEncounterRewards();
                await ToSignal(rewardScene, "tree_exited");

                ChangeEndScreenVisibilityOnly(true);
            }
            else
            {
                if (gameObject.Tier == gameObject.MaxTier)
                {
                    gameObject.MaxTier++;
                    ChangeEndScreenVisibilityOnly(false);

                    var rewardScene = ResourceLoader.Load<PackedScene>(Scenes.REWARDS).Instantiate<RewardScreen>();
                    this.GetTree().Root.AddChild(rewardScene);
                    rewardScene.InitializeSMTRewards();
                    await ToSignal(rewardScene, "tree_exited");

                    ChangeEndScreenVisibilityOnly(true);
                }
            }


            // do reward stuff here
            int currentTier, tierCap, maxTier;

            if(_dungeonCrawlEncounter)
            {
                currentTier = gameObject.TierDC;
                tierCap = gameObject.TierDCCap;
                maxTier = gameObject.MaxTierDC;
                _continueButton.Visible = true;
                _continueButton.Text = $"Continue exploring.";
                _backToHomeButton.Visible = false;
            }
            else
            {
                currentTier = gameObject.Tier;
                tierCap = gameObject.TierCap;
                maxTier = gameObject.MaxTier;
                _continueButton.Visible = (currentTier + 1 < tierCap);
                if (currentTier + 1 == maxTier + 1)
                {
                    _continueButton.Visible = false;
                }
                else
                {
                    _continueButton.Text = $"Tier {currentTier + 1}";
                }

                _backToHomeButton.Text = "Leave";
            }
        }
        else if (retreated)
        {
            endLabel.Text = "Retreated from battle.";
            if (_dungeonCrawlEncounter)
            {
                endLabel.Text = "Retreated from dungeon...";
                _backToHomeButton.Visible = true;
            } 
        }
        else
        {
            endLabel.Text = "You died.";
        }

        PersistentGameObjects.Save();
    }

    private void SetEndScreenVisibility(bool visible)
    {
        ChangeEndScreenVisibilityOnly(visible);

        if (_continueButton.Visible)
            _continueButton.Visible = false;

        this.GetNode<VBoxContainer>("%PlayerVBoxContainer").Visible = !visible;
        _skillDisplayIcons.Visible = false;
        _enemyMembers.Visible = !visible;
    }

    private void ChangeEndScreenVisibilityOnly(bool visible)
    {
        _endBox.Visible = visible;

        if (_dungeonCrawlEncounter)
        {
            _retryFloorButton.Visible = false;
            _backToHomeButton.Visible = false;
            _changePartyButton.Visible = false;
        }
        else
        {
            _retryFloorButton.Visible = visible;
            _backToHomeButton.Visible = visible;
            _changePartyButton.Visible = visible;
        }

       
    }
}
