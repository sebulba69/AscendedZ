using AscendedZ;
using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.skills;
using Godot;
using Godot.NativeInterop;
using System;

public enum ActionMenuState
{
    Menu,
    SkillSelect,
    TargetSelect
}

public partial class ActionMenu : PanelContainer
{
	private ItemList _actionList;
	private Label _menu;
    private bool _canInput;
    private int _selectedIndex;
    private ActionMenuState _state;

    private readonly string MENU_STR = "(D/←) Menu";
    private readonly string SKILL_STR = "(A/→) Skills";
    private readonly string TARGET_STR = "(D/←) Skills";
    private readonly string BACK_STR = "← Back";

    public bool CanInput { get => _canInput; set => _canInput = value; }

    private BattleSceneObject _battleSceneObject;

    private PlayerTargetSelectedEventArgs _playerTargetSelectedEventArgs;

    public BattleSceneObject BattleSceneObject 
    {
        get => _battleSceneObject;
        set
        {
            if(_battleSceneObject == null)
            {
                _battleSceneObject = value;
            }
        }
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _state = ActionMenuState.SkillSelect;
		_actionList = this.GetNode<ItemList>("%ActionList");
		_menu = this.GetNode<Label>("%MenuLabel");
        _canInput = true;
        _selectedIndex = 0;

        // in the menu is Skill, Retreat
        _menu.Text = MENU_STR;
        // _menu.Text = "Menu ● (Space/Click) Use";

        // item_clicked
        _actionList.ItemSelected += (long selected) => { _selectedIndex = (int)selected; };
        _actionList.ItemClicked += _OnMenuItemClicked;
    }

    public override void _Input(InputEvent @event)
    {
        if (!_canInput)
            return;

        if (@event.IsActionPressed(Controls.UP))
        {
            _selectedIndex--;
            if (_selectedIndex <= 0)
                _selectedIndex = 0;

            _actionList.Select(_selectedIndex);
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            _selectedIndex++;
            if (_selectedIndex >= _actionList.ItemCount)
                _selectedIndex = _actionList.ItemCount - 1;

            _actionList.Select(_selectedIndex);
        }

        if (@event.IsActionPressed(Controls.LEFT))
        {
            if (_state == ActionMenuState.SkillSelect)
            {
                LoadMenu();
            }
            else if(_state == ActionMenuState.TargetSelect)
            {
                LoadActiveSkillList();
            }
        }

        if (@event.IsActionPressed(Controls.RIGHT))
        {
            if (_state == ActionMenuState.Menu)
            {
                LoadActiveSkillList();
            }
        }

        if (@event.IsActionPressed(Controls.ENTER))
        {
            if(_state != ActionMenuState.Menu)
                _OnMenuItemClicked((long)_selectedIndex, new Vector2(), (long)MouseButton.Left);
        }
    }

    private void LoadMenu()
    {
        _actionList.Clear();
        _actionList.AddItem("Magic", SkillAssets.GenerateIcon(SkillAssets.MAGIC_ICON));

        var retreat = SkillDatabase.Retreat;
        _actionList.AddItem(retreat.GetBattleDisplayString(), SkillAssets.GenerateIcon(retreat.Icon));

        _menu.Text = SKILL_STR;
        _state = ActionMenuState.Menu;

        _selectedIndex = 0;
        _actionList.Select(0);
    }

    public void LoadActiveSkillList()
    {
        _actionList.Clear();

        var active = BattleSceneObject.ActivePlayer;
        foreach (ISkill skill in active.Skills)
            _actionList.AddItem(skill.GetBattleDisplayString(), SkillAssets.GenerateIcon(skill.Icon));

        _actionList.AddItem(BACK_STR);
        _selectedIndex = 0;
        _actionList.Select(0);

        _menu.Text = MENU_STR;
        _state = ActionMenuState.SkillSelect;
    }

    private void LoadTargetList()
    {
        _actionList.Clear();

        TargetTypes skillTargetType = _battleSceneObject.ActivePlayer.Skills[_playerTargetSelectedEventArgs.SkillIndex].TargetType;
        
        int count = 1;
        // only show valid targets for the skill we have selected
        if (skillTargetType == TargetTypes.SINGLE_OPP)
        {
            foreach (var enemy in _battleSceneObject.Enemies.FindAll(enemy => enemy.HP > 0))
                _actionList.AddItem($"{count++}. {enemy.Name}", CharacterImageAssets.GetTextureForItemList(enemy.Image));
        }
        else
        {
            foreach (var player in _battleSceneObject.AlivePlayers)
                _actionList.AddItem($"{count++}. {player.Name}", CharacterImageAssets.GetTextureForItemList(player.Image));
        }

        _state = ActionMenuState.TargetSelect;

        _menu.Text = TARGET_STR;

        _actionList.AddItem(BACK_STR);
        _selectedIndex = 0;
        _actionList.Select(0);
    }

    private void _OnMenuItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if(_canInput && mouse_button_index == (long)MouseButton.Left)
        {
            _selectedIndex = (int)index;
            switch (_state)
            {
                case ActionMenuState.Menu:
                    if (_selectedIndex == 0) 
                    {
                        LoadActiveSkillList();
                    }
                    else if (_selectedIndex == 1)
                    {
                        _battleSceneObject.HandlePostTurnProcessing(new BattleResult
                        {
                            SkillUsed = SkillDatabase.Retreat,
                            ResultType = BattleResultType.Retreat
                        });
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case ActionMenuState.SkillSelect:
                    if(_selectedIndex == _actionList.ItemCount - 1)
                    {
                        LoadMenu();
                        break;
                    }

                    _playerTargetSelectedEventArgs = new PlayerTargetSelectedEventArgs
                    {
                        SkillIndex = _selectedIndex
                    };
                    LoadTargetList();
                    break;
                case ActionMenuState.TargetSelect:
                    if (_selectedIndex == _actionList.ItemCount - 1)
                    {
                        LoadActiveSkillList();
                        break;
                    }

                    _playerTargetSelectedEventArgs.TargetIndex = _selectedIndex;
                    _battleSceneObject.SkillSelected?.Invoke(_battleSceneObject, _playerTargetSelectedEventArgs);
                    _canInput = false;
                    break;
            }
        }
    }
}
