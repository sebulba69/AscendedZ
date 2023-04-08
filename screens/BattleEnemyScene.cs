using AscendedZ;
using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using Godot.Collections;
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
    [Signal]
    public delegate void StartEnemyTurnEventHandler();

    private HBoxContainer _partyMembers;
    private HBoxContainer _enemyMembers;
    private PanelContainer _skillDisplayIcons;
    private BattleSceneObject _battleSceneObject;
    private ProgressBar _ap;
    private Button _skillButton;
    private Button _backToHomeButton, _retryFloorButton, _continueButton;
    private ItemList _skillList;
    private ItemList _targetList;
    private CenterContainer _endBox;
    private bool _uiUpdating = false;

    private Label _skillName;
    private TextureRect _skillIcon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("UIUpdated");

        _skillName = this.GetNode<Label>("%SkillName");
        _skillIcon = this.GetNode<TextureRect>("%SkillIcon");
        _skillDisplayIcons = this.GetNode<PanelContainer>("%SkillDisplayIcons");

        _partyMembers = this.GetNode<HBoxContainer>("%PartyPortraits");
        _enemyMembers = this.GetNode<HBoxContainer>("%EnemyContainerBox");
        _ap = this.GetNode<ProgressBar>("%APBar");

        _skillList = this.GetNode<ItemList>("%SkillsList");
        _targetList = this.GetNode<ItemList>("%TargetList");
        _skillButton = this.GetNode<Button>("%Skill");

        _endBox = this.GetNode<CenterContainer>("%EndBox");
        _backToHomeButton = this.GetNode<Button>("%BackToHomeBtn");
        _retryFloorButton = this.GetNode<Button>("%RetryFloorBtn");
        _continueButton = this.GetNode<Button>("%ContinueBtn");

        _skillButton.Pressed += _OnSkillButtonPressed;
        _skillList.Connect("item_selected", new Callable(this, "_OnSkillListItemSelected"));

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
            PersistentGameObjects.Instance().Tier++;
            InitializeBattleScene();
        };

        InitializeBattleScene();
    }

    private void InitializeBattleScene()
    {
        ClearChildrenFromNode(_partyMembers);
        ClearChildrenFromNode(_enemyMembers);
        _skillList.Clear();

        _skillButton.Disabled = false;

        _battleSceneObject = new BattleSceneObject();
        _battleSceneObject.InitializeEnemies(PersistentGameObjects.Instance().Tier);
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
            var enemyBox = ResourceLoader.Load<PackedScene>(Scenes.ENEMY_BOX).Instantiate();
            _enemyMembers.AddChild(enemyBox);
            enemyBox.Call("InstanceEntity", new EntityWrapper() { BattleEntity = enemy });
        }

        // set the turns and prep the b.s.o. for processing battle stuff
        _battleSceneObject.StartCurrentState(true);
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

    /// <summary>
    /// We handle our battle sequencing here because it's a really easy way of avoiding
    /// Spaghetti code with our events. It's easier to just have the Enemies do everything
    /// in one place so we don't have weird sync up issues.
    /// </summary>
    private void _OnSkillButtonPressed()
    {
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
                this.EndBattle(false);
                return;
            }

            // display skill icon display
            if (result.SkillUsed != null)
            {
                _skillDisplayIcons.Visible = true;
                ChangeSkillIconRegion(ArtAssets.ICONS[result.SkillUsed.Icon]);
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

        // update HP values on everyone
        for (int i = 0; i < update.Enemies.Count; i++)
        {
            var enemyDisplay = _enemyMembers.GetChild(i);
            var enemyWrapper = new EntityWrapper() { BattleEntity = update.Enemies[i] };
            enemyDisplay.Call("UpdateEntityDisplay", enemyWrapper);
        }

        for (int j = 0; j < update.Players.Count; j++)
        {
            var partyDisplay = _partyMembers.GetChild(j);
            var playerWrapper = new EntityWrapper() { BattleEntity = update.Players[j] };
            partyDisplay.Call("UpdateEntityDisplay", playerWrapper);
        }

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

        // populate skill list with new player
        int skillIndex = 0;
        if (_skillList.GetSelectedItems().Length > 0)
            skillIndex = _skillList.GetSelectedItems()[0];
        _skillList.Clear();

        foreach (ISkill skill in _battleSceneObject.ActivePlayer.Skills)
            _skillList.AddItem(skill.GetBattleDisplayString(), ArtAssets.GenerateIcon(skill.Icon));
        _skillList.Select(skillIndex);

        UpdateTargetList();

        _ap.Value = update.CurrentAPBarTurnValue;
        if (update.DidTurnStateChange)
        {
            if (_battleSceneObject.TurnState == TurnState.PLAYER)
            {
                _battleSceneObject.SetPartyMemberTurns();
                string playerYellow = "ffff2ad7";
                SetNewAPBar(playerYellow);
            }
            else
            {
                _battleSceneObject.SetupEnemyTurns();
                string enemyOrange = "ff922a";
                SetNewAPBar(enemyOrange);
            }
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
                _targetList.AddItem($"{count++}. {enemy.Name}");
        }
        else
        {
            foreach (var player in _battleSceneObject.Players.FindAll(party => party.HP > 0))
                _targetList.AddItem($"{count++}. {player.Name}");
        }

        if (_targetList.ItemCount > 0)
        {
            if (targetIndex == _targetList.ItemCount)
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

    private void SetNewAPBar(string color)
    {
        StyleBoxFlat styleBox = ResourceLoader.Load<StyleBoxFlat>("res://screens/APBarStyleBox.tres");
        styleBox.BgColor = new Color(color);
        _ap.AddThemeStyleboxOverride("fill", styleBox);
        _ap.MaxValue = _battleSceneObject.PressTurn.Turns;
        _ap.Value = _ap.MaxValue;
    }

    private async void EndBattle(bool didPlayerWin, bool retreated=false)
    {
        SetEndScreenVisibility(true);

        Label endLabel = this.GetNode<Label>("%EndOfBattleLabel");

        // heal everyone
        foreach(var member in _battleSceneObject.Players)
        {
            member.HP = member.MaxHP;
        }

        if (didPlayerWin)
        {
            endLabel.Text = "Encounter Complete!";

            var gameObject = PersistentGameObjects.Instance();
            if (gameObject.Tier == gameObject.MaxTier)
            {
                gameObject.MaxTier++;
                if (RewardGenerator.REWARD_TIERS.Contains(gameObject.Tier))
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
            this.GetNode<Button>("%RetryFloorBtn").Visible = false;
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
        this.GetNode<PanelContainer>("%APContainer").Visible = !visible;
        _skillDisplayIcons.Visible = !visible;
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
