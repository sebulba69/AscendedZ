using AscendedZ;
using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Godot.HttpRequest;
using static Godot.WebSocketPeer;

public partial class BattleEnemyScene : Node2D
{
    private bool _canInput;

    private HBoxContainer _partyMembers;
    private HBoxContainer _enemyMembers;
    private PanelContainer _skillDisplayIcons;
    private BattleSceneObject _battleSceneObject;
    private Button _skillButton;
    private Button _backToHomeButton, _retryFloorButton, _continueButton;
    private ItemList _skillList;
    private ItemList _targetList;
    private CenterContainer _endBox;
    private bool _uiUpdating = false;

    private Label _skillName;
    private TextureRect _skillIcon;
    private HBoxContainer _turnIconContainer;

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

        _skillList = this.GetNode<ItemList>("%SkillsList");
        _targetList = this.GetNode<ItemList>("%TargetList");
        _skillButton = this.GetNode<Button>("%Skill");

        _endBox = this.GetNode<CenterContainer>("%EndBox");
        _backToHomeButton = this.GetNode<Button>("%BackToHomeBtn");
        _retryFloorButton = this.GetNode<Button>("%RetryFloorBtn");
        _continueButton = this.GetNode<Button>("%ContinueBtn");

        _skillButton.Pressed += _OnSkillButtonPressed;
        _skillList.Connect("item_selected", new Callable(this, "_OnSkillListItemSelected"));

        AudioStreamPlayer audioPlayer = this.GetNode<AudioStreamPlayer>("MusicPlayer");
        PersistentGameObjects.GameObjectInstance().MusicPlayer.SetStreamPlayer(audioPlayer);

        _backToHomeButton.Pressed += () =>
        {
            PackedScene mainScreenScene = ResourceLoader.Load<PackedScene>(Scenes.MAIN);
            this.GetTree().Root.AddChild(mainScreenScene.Instantiate());
            this.QueueFree();
        };

        _retryFloorButton.Pressed += () =>
        {
            SetEndScreenVisibility(false);
            InitializeBattleScene();
        };

        _continueButton.Pressed += () =>
        {
            SetEndScreenVisibility(false);
            PersistentGameObjects.GameObjectInstance().Tier++;
            InitializeBattleScene();
        };

        InitializeBattleScene();
    }

    public override void _Input(InputEvent @event)
    {
        if (!_canInput)
            return;

        if (@event.IsActionPressed(Controls.UP))
        {
            if (_skillList.GetSelectedItems().Length > 0)
            {
                int upIndex = _skillList.GetSelectedItems()[0];
                upIndex--;
                if (upIndex < 0)
                    upIndex = 0;
                _skillList.Select(upIndex);
                UpdateTargetList();
            } 
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            if (_skillList.GetSelectedItems().Length > 0)
            {
                int downIndex = _skillList.GetSelectedItems()[0];
                downIndex++;
                if (downIndex >= _skillList.ItemCount)
                    downIndex = _skillList.ItemCount - 1;
                _skillList.Select(downIndex);
                UpdateTargetList();
            }
        }

        if (@event.IsActionPressed(Controls.LEFT))
        {
            if (_targetList.GetSelectedItems().Length > 0)
            {
                int upIndex = _targetList.GetSelectedItems()[0];
                upIndex--;
                if (upIndex < 0)
                    upIndex = 0;
                _targetList.Select(upIndex);
            }
        }

        if (@event.IsActionPressed(Controls.RIGHT))
        {
            if (_targetList.GetSelectedItems().Length > 0)
            {
                int downIndex = _targetList.GetSelectedItems()[0];
                downIndex++;
                if (downIndex >= _targetList.ItemCount)
                    downIndex = _targetList.ItemCount - 1;
                _targetList.Select(downIndex);
            }
        }

        if (@event.IsActionPressed(Controls.ENTER))
        {
            if(!_skillButton.Disabled)
                _OnSkillButtonPressed();
        }
    }

    private void InitializeBattleScene()
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        ClearChildrenFromNode(_partyMembers);
        ClearChildrenFromNode(_enemyMembers);
        _skillList.Clear();

        _skillButton.Disabled = false;

        TextureRect background = this.GetNode<TextureRect>("%Background");
        background.Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetCombatBackground(gameObject.Tier));

        _battleSceneObject = new BattleSceneObject();
        _battleSceneObject.InitializeEnemies(gameObject.Tier);
        _battleSceneObject.InitializePartyMembers();

        _battleSceneObject.UpdateUI += _OnUIUpdate;

        // add players to the scene
        foreach (var member in _battleSceneObject.Players)
        {
            var partyBox = ResourceLoader.Load<PackedScene>(Scenes.PARTY_BOX).Instantiate();
            _partyMembers.AddChild(partyBox);
            partyBox.Call("InstanceEntity", new EntityWrapper() { BattleEntity = member });
        }

        foreach (var enemy in _battleSceneObject.Enemies)
        {
            var enemyBox = (enemy.IsBoss) 
                ? ResourceLoader.Load<PackedScene>(Scenes.BOSS_BOX).Instantiate()
                : ResourceLoader.Load<PackedScene>(Scenes.ENEMY_BOX).Instantiate();
            
            _enemyMembers.AddChild(enemyBox);
            enemyBox.Call("InstanceEntity", new EntityWrapper() { BattleEntity = enemy, IsBoss = enemy.IsBoss });
            enemyBox.Call("SetDescription", enemy.Description);
        }

        // set the turns and prep the b.s.o. for processing battle stuff
        _battleSceneObject.StartBattle();
        UpdateTurnsUsingTurnState(TurnState.PLAYER);

        string dungeonTrack = MusicAssets.GetDungeonTrack(gameObject.Tier);
        gameObject.MusicPlayer.PlayMusic(dungeonTrack, (gameObject.Tier == 5 || gameObject.Tier % 10 == 0));
        _canInput = true;
    }

    private void _OnSkillListItemSelected(int index)
    {
        UpdateTargetList();
    }

    private void ClearChildrenFromNode(Node node)
    {
        foreach (var child in node.GetChildren())
        {
            node.RemoveChild(child);
            child.QueueFree();
        }
    }

    private void _OnSkillButtonPressed()
    {
        _canInput = false;
        _skillButton.Disabled = true;

        int selectedSkillIndex = _skillList.GetSelectedItems()[0];
        int selectedTargetIndex = _targetList.GetSelectedItems()[0];

        _battleSceneObject.SkillSelected?.Invoke(_battleSceneObject, new PlayerTargetSelectedEventArgs() 
        {
            SkillIndex = selectedSkillIndex,
            TargetIndex = selectedTargetIndex
        });
    }

    private async void _OnUIUpdate(object sender, BattleUIUpdate update)
    {
        // handle battle results if any
        if(update.Result != null)
        {
            _skillButton.Disabled = true;

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

            // if our user can provide inputs, then re-enable the button
            if (update.UserCanInput)
                _skillButton.Disabled = false;

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

        SetActiveSkills();

        SetNewTurns(_battleSceneObject.TurnState == TurnState.PLAYER);
        if (_battleSceneObject.PressTurn.TurnEnded)
        {
            _battleSceneObject.PressTurn.TurnEnded = false; // set turns
            _battleSceneObject.ChangeTurnState(); // change turn state
            UpdateTurnsUsingTurnState(_battleSceneObject.TurnState); // change ap bar visuals
        }

        // after we fully display an animation and process a skill
        // then we want to use an enemy skill
        if (_battleSceneObject.TurnState == TurnState.ENEMY)
            _battleSceneObject.DoEnemyMove();
        else
            _canInput = true;
    }

    private void SetActiveSkills()
    {
        // populate skill list with new active player
        int skillIndex = 0;
        if (_skillList.GetSelectedItems().Length > 0)
            skillIndex = _skillList.GetSelectedItems()[0];
        _skillList.Clear();

        if(_battleSceneObject.ActivePlayer != null)
        {
            foreach (ISkill skill in _battleSceneObject.ActivePlayer.Skills)
                _skillList.AddItem(skill.GetBattleDisplayString(), SkillAssets.GenerateIcon(skill.Icon));
            _skillList.Select(skillIndex);

            UpdateTargetList();
        }
    }

    private void UpdateTurnsUsingTurnState(TurnState turnState)
    {
        if (turnState == TurnState.PLAYER)
        {
            _battleSceneObject.SetPartyMemberTurns();
            SetNewTurns(true);

            SetActiveSkills();

            _skillButton.Disabled = false;
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
            var partyDisplay = _partyMembers.GetChild(j);
            var playerWrapper = new EntityWrapper() { BattleEntity = players[j] };
            partyDisplay.Call("UpdateEntityDisplay", playerWrapper);
        }
    }

    private void UpdateTargetList()
    {
        int targetIndex = 0;
        int skillIndex = 0;

        if (_targetList.GetSelectedItems().Length > 0)
            targetIndex = _targetList.GetSelectedItems()[0];

        if (_skillList.GetSelectedItems().Length > 0)
            skillIndex = _skillList.GetSelectedItems()[0];

        ISkill skill = _battleSceneObject.ActivePlayer.Skills[skillIndex];

        int count = 1;
        _targetList.Clear();

        // only show valid targets for the skill we have selected
        if (skill.TargetType == TargetTypes.SINGLE_OPP)
        {
            foreach (var enemy in _battleSceneObject.Enemies.FindAll(enemy => enemy.HP > 0))
                _targetList.AddItem($"{count++}. {enemy.Name}", CharacterImageAssets.GetTextureForItemList(enemy.Image));
        }
        else
        {
            foreach (var player in _battleSceneObject.AlivePlayers)
                _targetList.AddItem($"{count++}. {player.Name}", CharacterImageAssets.GetTextureForItemList(player.Image));
        }

        if (_targetList.ItemCount > 0)
        {
            if (targetIndex >= _targetList.ItemCount)
                targetIndex = _targetList.ItemCount - 1;

            _targetList.Select(targetIndex);
        }
    }

    #region Battle Result Functions
    private Node FindBattleEntityNode(BattleEntity entity)
    {
        Node nodeToFind;

        if (entity.GetType() == typeof(BattlePlayer))
        {
            int pIndex = _battleSceneObject.Players.IndexOf((BattlePlayer)entity);
            nodeToFind = _partyMembers.GetChild(pIndex);
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
        _canInput = false;
        SetEndScreenVisibility(true);

        Label endLabel = this.GetNode<Label>("%EndOfBattleLabel");

        // heal everyone
        foreach(var member in _battleSceneObject.Players)
        {
            member.HP = member.MaxHP;
            member.StatusHandler.Clear();
        }

        if (didPlayerWin)
        {
            endLabel.Text = "Encounter Complete!";

            var gameObject = PersistentGameObjects.GameObjectInstance();

            if(gameObject.Tier == 5 || gameObject.Tier % 10 == 0)
            {
                gameObject.MusicPlayer.PlayMusic(MusicAssets.BOSS_VICTORY);
                gameObject.MusicPlayer.ResetAllTracksAfterBoss();
            }
                

            if (gameObject.Tier == gameObject.MaxTier)
            {
                gameObject.MaxTier++;
                if (RewardGenerator.CanGenerateRewardsAtTier(gameObject.Tier))
                {
                    ChangeEndScreenVisibilityOnly(false);

                    var rewardScene = ResourceLoader.Load<PackedScene>(Scenes.REWARDS).Instantiate();
                    this.GetTree().Root.AddChild(rewardScene);
                    await ToSignal(rewardScene, "tree_exited");

                    ChangeEndScreenVisibilityOnly(true);
                }
            }

            // do reward stuff here
            _continueButton.Visible = true;

            int nextTier = gameObject.Tier + 1;
            if (nextTier == gameObject.MaxTier + 1)
            {
                _continueButton.Visible = false;
            }
            else
            {
                _continueButton.Text = $"Tier {nextTier}";
            }

            _backToHomeButton.Text = "Leave";
        }
        else if (retreated)
        {
            endLabel.Text = "Retreated from battle.";
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

        this.GetNode<CenterContainer>("%PlayerContainer").Visible = !visible;
        _skillDisplayIcons.Visible = false;
        _enemyMembers.Visible = !visible;
    }

    private void ChangeEndScreenVisibilityOnly(bool visible)
    {
        _endBox.Visible = visible;
        _backToHomeButton.Visible = visible;
        _retryFloorButton.Visible = visible;
    }

    private void _OnExitScene()
    {
        this.GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>(Scenes.BATTLE_SCENE).Instantiate());
        this.QueueFree();
    }
}
