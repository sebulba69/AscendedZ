using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ;
using Godot;
using System;
using static Godot.WebSocketPeer;
using System.Reflection;
using AscendedZ.dungeon_crawling.scenes.weapon_armory;
using AscendedZ.game_object;
using System.Diagnostics.Metrics;

public partial class ArmoryScene : CenterContainer
{
	private ItemList _reserves;
	private Label _totalHP, _totalAtk;
	private PanelContainer _primaryWeaponContainer;
	private WeaponGridDisplay _weaponGridDisplay;
	private Button _smeltBtn, _setPrimaryBtn;

	private ArmoryObject _armoryObject;

	private int _rSelected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_reserves = this.GetNode<ItemList>("%ReserveWeaponList");
		_totalHP = this.GetNode<Label>("%TotalHPLabel");
		_totalAtk = this.GetNode<Label>("%TotalAttackLabel");
		_primaryWeaponContainer = this.GetNode<PanelContainer>("%PrimaryWeaponContainer");
		_weaponGridDisplay = this.GetNode<WeaponGridDisplay>("%WeaponGridDisplay");
		_smeltBtn = this.GetNode<Button>("%SmeltWeaponBtn");
		_setPrimaryBtn = this.GetNode<Button>("%SetPrimaryWeaponBtn");

		_reserves.ItemClicked += _OnReserveItemClicked;

		_smeltBtn.Pressed += _OnSmeltBtnPressed;
		_setPrimaryBtn.Pressed += _OnSetPrimaryBtnPressed;

        Button back = this.GetNode<Button>("%BackBtn");
		back.Pressed += () =>
		{
			foreach (var weapon in _armoryObject.GetWeaponList())
				weapon.SmeltInto = false;

            PersistentGameObjects.Save();
            this.QueueFree();
		};

		var go = PersistentGameObjects.GameObjectInstance();
		_armoryObject = new ArmoryObject(go.MainPlayer.DungeonPlayer);

		_rSelected = 0;

		UpdateUI();
	}

	private void UpdateUI()
	{
		var primary = _armoryObject.GetPrimaryWeapon();
		var reserves = _armoryObject.GetWeaponList();

		_reserves.Clear();
		_weaponGridDisplay.Clear();

        if (primary != null)
        {
            var children = _primaryWeaponContainer.GetChildren();
            foreach (var child in children)
                _primaryWeaponContainer.RemoveChild(child);

            var weaponDisplay = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_WEAPON_DISPLAY).Instantiate<WeaponDisplay>();
            _primaryWeaponContainer.AddChild(weaponDisplay);
            weaponDisplay.Initialize(primary);
        }

        foreach (var reserve in reserves)
		{
			if(reserve.Equipped && !reserve.PrimaryWeapon)
				_weaponGridDisplay.AddWeapon(reserve);
			
            _reserves.AddItem(reserve.GetArmoryDisplayString(), SkillAssets.GenerateIcon(reserve.Icon));
        }
			



		SetSelectedIndeces();

        if (_reserves.ItemCount > 0)
			_reserves.Select(_rSelected);

		_totalHP.Text = $"{_armoryObject.GetTotalHP()} HP";
		_totalAtk.Text = $"{_armoryObject.GetTotalAtk()} ATK";
	}

	private void SetSelectedIndeces()
	{
		if (_rSelected >= _reserves.ItemCount) _rSelected = _reserves.ItemCount - 1;
        if (_rSelected < 0) _rSelected = 0;
    }

	private void _OnSmeltBtnPressed()
	{
		if(_reserves.ItemCount > 0)
		{
			try
			{
				int rSelect = _reserves.GetSelectedItems()[0];
				_armoryObject.SmeltReserveWeapon(rSelect);
				UpdateUI();
			}
			catch (IndexOutOfRangeException) { }
		}
	}

	private void _OnSetPrimaryBtnPressed() 
	{
		if (_reserves.ItemCount > 0) 
		{
			int selected = _reserves.GetSelectedItems()[0];
			_armoryObject.SetPrimaryWeapon(selected);
			UpdateUI();
		}
	}

	// Select a weapon to be equipped
	private void _OnReserveItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
		if (mouse_button_index == (long)MouseButton.Right)
		{
			_rSelected = (int)index;
			_armoryObject.ChangeGridStatus(_rSelected);
			UpdateUI();
		}
		else if (mouse_button_index == (long)MouseButton.Left) 
		{
			_rSelected = (int)index;
			_armoryObject.SetSmelt(_rSelected);
			UpdateUI();
		}
    }
}
