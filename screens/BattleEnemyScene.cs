﻿using AscendedZ;
using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.game_object;
using AscendedZ.screens.end_screen;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;


public partial class BattleEnemyScene : Node2D
{
    private readonly PackedScene _partyScreen = ResourceLoader.Load<PackedScene>(Scenes.PARTY_CHANGE);
    private readonly PackedScene _partyBox = ResourceLoader.Load<PackedScene>(Scenes.PARTY_BOX);
    private readonly PackedScene _enemyBox = ResourceLoader.Load<PackedScene>(Scenes.ENEMY_BOX);
    private readonly PackedScene _turnIcons = ResourceLoader.Load<PackedScene>(Scenes.TURN_ICONS);
    private readonly PackedScene _rewardScene = ResourceLoader.Load<PackedScene>(Scenes.REWARDS);

    private HBoxContainer _partyMembers;
    private HBoxContainer _enemyMembers;
    private PanelContainer _skillDisplayIcons;
    private BattleSceneObject _battleSceneObject;
    private CenterContainer _endBox;
    private bool _uiUpdating = false;
    private bool _dungeonCrawlEncounter = false;
    private Label _skillName;
    private TextureRect _skillIcon;
    private HBoxContainer _turnIconContainer;
    private ActionMenu _actionMenu;
    private EndScreenOptions _endScreenOptions;
    private bool _random, _randomBoss;

    public EventHandler BackToHome;

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
        _actionMenu = this.GetNode<ActionMenu>("%ActionMenu");
        _endScreenOptions = GetNode<EndScreenOptions>("%EndScreenOptions");
    }

    private async void _OnBackToHomeBtnPressed(object sender, EventArgs e)
    {
        _endScreenOptions.CanInput = false;
        if (!_dungeonCrawlEncounter) 
        {
            PackedScene mainScreenScene = ResourceLoader.Load<PackedScene>(Scenes.MAIN);
            var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
            AddChild(transition);
            transition.PlayFadeIn();
            await ToSignal(transition.Player, "animation_finished");
            this.GetTree().Root.AddChild(mainScreenScene.Instantiate());
            transition.PlayFadeOut();
            await ToSignal(transition.Player, "animation_finished");
            transition.QueueFree();
            QueueFree();
            
        }
        else
        {
            BackToHome?.Invoke(this, null);
        }

        this.QueueFree();
    }


    private void _OnRetryFloorBtnPressed(object sender, EventArgs e)
    {
        _endScreenOptions.CanInput = false;
        SetEndScreenVisibility(false);
        InitializeBattleScene();
    }

    private void _OnContinueBtnPressed(object sender, EventArgs e)
    {
        if (!_dungeonCrawlEncounter) 
        {
            _endScreenOptions.CanInput = false;
            SetEndScreenVisibility(false);
            PersistentGameObjects.GameObjectInstance().Tier++;
            InitializeBattleScene();
        }
        else
        {
            QueueFree();
        }
    }

    private void _OnChangePartyBtnPressed(object sender, EventArgs e)
    {
        _endScreenOptions.CanInput = false;

        var vbox = this.GetNode<VBoxContainer>("%EndVBox");
        vbox.Visible = false;

        var partyChangeScene = _partyScreen.Instantiate<PartyEditScreen>();

        _endBox.AddChild(partyChangeScene);

        partyChangeScene.DisableEmbarkButton();

        partyChangeScene.DoEmbark += (sender, args) => 
        {
            partyChangeScene.QueueFree();
            vbox.Visible = true;
            _endScreenOptions.CanInput = true;
        };
    }

    public void SetupForNormalEncounter()
    {
        _dungeonCrawlEncounter = false;
        AudioStreamPlayer audioPlayer = this.GetNode<AudioStreamPlayer>("MusicPlayer");
        PersistentGameObjects.GameObjectInstance().MusicPlayer.SetStreamPlayer(audioPlayer);
        InitializeBattleScene();
    }

    public void SetupForDungeonCrawlEncounter(List<BattlePlayer> players, bool random, bool isRandomBoss)
    {
        _random = random;
        _randomBoss = isRandomBoss;
        _dungeonCrawlEncounter = true;
        InitializeBattleScene(players, random, isRandomBoss);
    }

    private void InitializeBattleScene(List<BattlePlayer> players = null, bool random = false, bool isRandomBoss = false)
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();
        int tier = (_dungeonCrawlEncounter) ? gameObject.TierDC : gameObject.Tier;

        ClearChildrenFromNode(_partyMembers);
        ClearChildrenFromNode(_enemyMembers);

        TextureRect background = this.GetNode<TextureRect>("%Background");
        string backgroundString = (_dungeonCrawlEncounter) ? BackgroundAssets.GetCombatDCBackground(tier) :  BackgroundAssets.GetCombatBackground(tier);
        background.Texture = ResourceLoader.Load<Texture2D>(backgroundString);

        _battleSceneObject = new BattleSceneObject(tier);
        _actionMenu.EmptyClick = false;
        _actionMenu.BattleSceneObject = _battleSceneObject;

        if(!_dungeonCrawlEncounter)
        {
            _battleSceneObject.InitializeEnemies(tier, _dungeonCrawlEncounter);
            players = gameObject.MakeBattlePlayerListFromParty();
        }
        else
        {
            int dcTier = tier + 5;
            _battleSceneObject.InitializeEnemies(dcTier, _dungeonCrawlEncounter, random, isRandomBoss);
        }
            

        _battleSceneObject.InitializePartyMembers(players);
        _battleSceneObject.UpdateUI += _OnUIUpdate;

        // add players to the scene
        foreach (var member in _battleSceneObject.Players)
        {
            HBoxContainer hBoxContainer = new HBoxContainer() { Alignment = BoxContainer.AlignmentMode.Center };
            var partyBox = _partyBox.Instantiate<EntityDisplayBox>();

            _partyMembers.AddChild(hBoxContainer);
            hBoxContainer.AddChild(partyBox);

            partyBox.InstanceEntity(new EntityWrapper() { BattleEntity = member });
            if (member.IsActiveEntity)
            {
                _actionMenu.Reparent(hBoxContainer);
                _actionMenu.EmptyClick = false;
            }
        }

        foreach (var enemy in _battleSceneObject.Enemies)
        {
            EntityDisplayBox enemyBox;

            if (enemy.IsBoss)
            {
                enemyBox = ResourceLoader.Load<PackedScene>(Scenes.BOSS_BOX).Instantiate<EntityDisplayBox>();
                _enemyMembers.AddChild(enemyBox);
                enemyBox.InstanceEntity(new EntityWrapper() { BattleEntity = enemy, IsBoss = enemy.IsBoss });
            }
            else
            {
                enemyBox = _enemyBox.Instantiate<EntityDisplayBox>();
                _enemyMembers.AddChild(enemyBox);
                enemyBox.InstanceEntity(new EntityWrapper() { BattleEntity = enemy, IsBoss = enemy.IsBoss }, enemy.RandomEnemy);
                enemyBox.SetDescription(enemy.Description);
            }
        }

        // set the turns and prep the b.s.o. for processing battle stuff
        _battleSceneObject.StartBattle();

        UpdateTurnsUsingTurnState(TurnState.PLAYER);

        if (!_dungeonCrawlEncounter)
        {
            string dungeonTrack = MusicAssets.GetDungeonTrack(tier);
            bool isBoss = (tier % 10 == 0);
            gameObject.MusicPlayer.PlayMusic(dungeonTrack);
        }
        else
        {
            if(tier % 50 == 0)
            {
                gameObject.MusicPlayer.PlayMusic(MusicAssets.DC_BOSS);
            }

            if (isRandomBoss)
            {
                gameObject.MusicPlayer.PlayMusic(MusicAssets.GetDungeonTrackRandomBoss(tier));
            }
        }

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
                EntityDisplayBox userNode = (EntityDisplayBox)FindBattleEntityNode(result.User);
                BattleEffectWrapper userEffects = new BattleEffectWrapper()
                {
                    IsEntitySkillUser = true,
                    Result = result
                };

                await userNode.UpdateBattleEffects(userEffects);
            }

            if (result.Targets.Count > 0) 
            {
                Task[] tasks = new Task[result.Targets.Count];
                List<Node> targetNodes = new List<Node>();
                foreach(var target in result.Targets)
                    targetNodes.Add(FindBattleEntityNode(target));

                for(int t = 0; t < result.Targets.Count; t++)
                {
                    int index = t;
                    EntityDisplayBox targetNode = (EntityDisplayBox)targetNodes[index];
                    BattleResult subResult = new BattleResult();

                    subResult.ResultType = result.Results[index];
                    if(result.AllHPChanged.Count > 0)
                    {
                        subResult.HPChanged = result.AllHPChanged[index];
                    }
                    subResult.SkillUsed = result.SkillUsed;

                    BattleEffectWrapper targetNodeEffects = new BattleEffectWrapper() { Result = subResult };

                   tasks[t] = targetNode.UpdateBattleEffects(targetNodeEffects);
                    await Task.Delay(200);
                }

                await Task.WhenAll(tasks);
            }
            else if (result.Target != null)
            {
                EntityDisplayBox targetNode = (EntityDisplayBox)FindBattleEntityNode(result.Target);
                BattleEffectWrapper targetNodeEffects = new BattleEffectWrapper() { Result = result };

                await targetNode.UpdateBattleEffects(targetNodeEffects);
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
            var enemyDisplay = (EntityDisplayBox)_enemyMembers.GetChild(i);
            var enemy = enemies[i];
            var enemyWrapper = new EntityWrapper() { BattleEntity = enemy, IsBoss = enemy.IsBoss };
            enemyDisplay.UpdateEntityDisplay(enemyWrapper);
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
            var partyDisplay = (EntityDisplayBox)vBoxContainer.GetChild(0);

            var playerWrapper = new EntityWrapper() { BattleEntity = players[j] };
            partyDisplay.UpdateEntityDisplay(playerWrapper);

            if (players[j].IsActiveEntity)
            {
                _actionMenu.Reparent(vBoxContainer);
                _actionMenu.EmptyClick = false;
            }  
        }
    }

    #region Battle Result Functions
    private Node FindBattleEntityNode(BattleEntity entity)
    {
        Node nodeToFind;

        if (entity.Type == EntityType.Player)
        {
            int pIndex = _battleSceneObject.Players.IndexOf((BattlePlayer)entity);
            var child = _partyMembers.GetChild(pIndex);
            nodeToFind = child.GetChild(0);
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
            var turnIconScene = _turnIcons.Instantiate();
            _turnIconContainer.AddChild(turnIconScene);
            turnIconScene.Call("SetIconState", isPlayer, turn == 1);
        }
    }

    private async void EndBattle(bool didPlayerWin, bool retreated=false)
    {
        _actionMenu.CanInput = false;
        SetEndScreenVisibility(true);
        _endScreenOptions.Visible = false;
        _endScreenOptions.CanInput = false;
        Label endLabel = this.GetNode<Label>("%EndOfBattleLabel");

        // heal everyone
        foreach(var member in _battleSceneObject.Players)
        {
            if(!_dungeonCrawlEncounter)
                member.HP = member.MaxHP;

            member.DefenseModifier = 0;
            for(int i = 0; i < member.ElementDamageModifiers.Length; i++)
            {
                member.ElementDamageModifiers[i] = 0;
            }
            member.IsActiveEntity = false;
            member.StatusHandler.Clear();
        }

        List<EndScreenItem> options = new List<EndScreenItem>();

        if (didPlayerWin)
        {
            ChangeEndScreenVisibilityOnly(false);

            endLabel.Text = "Encounter Complete!";

            var gameObject = PersistentGameObjects.GameObjectInstance();

            if(gameObject.Tier % 10 == 0 && !_dungeonCrawlEncounter || gameObject.TierDC % 50 == 0 && _dungeonCrawlEncounter || _randomBoss)
            {
                gameObject.MusicPlayer.PlayMusic(MusicAssets.BOSS_VICTORY);
                gameObject.MusicPlayer.ResetAllTracksAfterBoss();
            }

            if (_dungeonCrawlEncounter)
            {
                GetNode<AudioStreamPlayer>("%ItemSfxPlayer").Play();
                var rewardScene = _rewardScene.Instantiate<RewardScreen>();
                this.GetTree().Root.AddChild(rewardScene);

                if (_random)
                    if (_randomBoss)
                        rewardScene.InitializeDungeonCrawlEncounterSpecialBossRewards();
                    else
                        rewardScene.InitializeDungeonCrawlEncounterSpecialRewards();
                else
                    rewardScene.InitializeDungeonCrawlEncounterRewards();

                await ToSignal(rewardScene, "tree_exited");
            }
            else
            {
                if (gameObject.Tier == gameObject.MaxTier)
                {
                    GetNode<AudioStreamPlayer>("%ItemSfxPlayer").Play();
                    var rewardScene = _rewardScene.Instantiate<RewardScreen>();
                    this.GetTree().Root.AddChild(rewardScene);

                    gameObject.MaxTier++;
                    ChangeEndScreenVisibilityOnly(false);
                    rewardScene.InitializeSMTRewards();
                    await ToSignal(rewardScene, "tree_exited");
                }
            }

            ChangeEndScreenVisibilityOnly(true);

            // do reward stuff here
            if (_dungeonCrawlEncounter)
            {
                var continueExploringItem = new EndScreenItem() { ItemText = "Continue exploring..." };
                continueExploringItem.ItemSelected += _OnContinueBtnPressed;
                options.Add(continueExploringItem);
            }
            else
            {
                int currentTier = gameObject.Tier;
                int tierCap = gameObject.TierCap;
                int maxTier = gameObject.MaxTier;

                var continueToNextTierItem = new EndScreenItem() { ItemText = $"Tier {currentTier + 1}" };


                continueToNextTierItem.ItemSelected += _OnContinueBtnPressed;


                if (currentTier + 1 < tierCap && currentTier + 1 != maxTier + 1)
                    options.Add(continueToNextTierItem);

                AddBasicDungeonOptions(options);
            }
        }
        else if (retreated)
        {
            endLabel.Text = "Retreated from battle.";
            
            if (_dungeonCrawlEncounter)
            {
                endLabel.Text = "Retreated from dungeon...";
                AddBackToHomeButton(options);
            }
            else
            {
                AddBasicDungeonOptions(options);
            }
        }
        else
        {
            endLabel.Text = "You died.";
            if (_dungeonCrawlEncounter)
            {
                AddBackToHomeButton(options);
            }
            else
            {
                AddBasicDungeonOptions(options);
            }
        }

        _endScreenOptions.SetItems(options);
        _endScreenOptions.CanInput = true;
        _endScreenOptions.Visible = true;
        PersistentGameObjects.Save();
    }

    private void AddBasicDungeonOptions(List<EndScreenItem> options)
    {
        var partyChangeItem = new EndScreenItem() { ItemText = "Party" };
        var retryItem = new EndScreenItem() { ItemText = "Retry" };

        partyChangeItem.ItemSelected += _OnChangePartyBtnPressed;
        retryItem.ItemSelected += _OnRetryFloorBtnPressed;

        options.Add(partyChangeItem);
        options.Add(retryItem);
        AddBackToHomeButton(options);
    }

    private void AddBackToHomeButton(List<EndScreenItem> options)
    {
        var backToHomeItem = new EndScreenItem() { ItemText = "Leave" };
        backToHomeItem.ItemSelected += _OnBackToHomeBtnPressed;
        options.Add(backToHomeItem);
    }

    private void SetEndScreenVisibility(bool visible)
    {
        ChangeEndScreenVisibilityOnly(visible);

        this.GetNode<VBoxContainer>("%PlayerVBoxContainer").Visible = !visible;
        _skillDisplayIcons.Visible = false;
        _enemyMembers.Visible = !visible;
    }

    private void ChangeEndScreenVisibilityOnly(bool visible)
    {
        _endBox.Visible = visible;
        _endScreenOptions.Visible = visible;
        _endScreenOptions.EmptyClick = false;
    }
}
