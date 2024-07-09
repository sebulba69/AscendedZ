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
	private ItemList _reserves, _equipped;
	private Label _totalHP, _totalAtk;
	private PanelContainer _primaryWeaponContainer;
	private WeaponGridDisplay _weaponGridDisplay;
	private Button _smeltBtn, _setPrimaryBtn;

	private ArmoryObject _armoryObject;

	private int _rSelected, _eSelected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_reserves = this.GetNode<ItemList>("%ReserveWeaponList");
		_equipped = this.GetNode<ItemList>("%EquippedWeaponList");
		_totalHP = this.GetNode<Label>("%TotalHPLabel");
		_totalAtk = this.GetNode<Label>("%TotalAttackLabel");
		_primaryWeaponContainer = this.GetNode<PanelContainer>("%PrimaryWeaponContainer");
		_weaponGridDisplay = this.GetNode<WeaponGridDisplay>("%WeaponGridDisplay");
		_smeltBtn = this.GetNode<Button>("%SmeltWeaponBtn");
		_setPrimaryBtn = this.GetNode<Button>("%SetPrimaryWeaponBtn");

		_reserves.ItemClicked += _OnReserveItemClicked;

		_smeltBtn.Pressed += _OnSmeltBtnPressed;
		_setPrimaryBtn.Pressed += _OnSetPrimaryBtnPressed;

		_equipped.ItemSelected += (index) => { _eSelected = (int)index; };

        Button back = this.GetNode<Button>("%BackBtn");
		back.Pressed += () => this.QueueFree();

		var go = PersistentGameObjects.GameObjectInstance();
		_armoryObject = new ArmoryObject(go.MainPlayer.DungeonPlayer);

		_rSelected = 0;
		_eSelected = 0;

		UpdateUI();
	}

	private void UpdateUI()
	{
		var reserves = _armoryObject.GetReserves();
		var equipped = _armoryObject.GetEquipped();
		var primary = _armoryObject.GetPrimaryWeapon();

		_reserves.Clear();
		_equipped.Clear();
		_weaponGridDisplay.Clear();

		foreach(var reserve in reserves)
		{
            _reserves.AddItem(reserve.GetArmoryDisplayString(), SkillAssets.GenerateIcon(reserve.Icon));
        }

		foreach(var equip in equipped)
		{
			if(!equip.PrimaryWeapon)
				_weaponGridDisplay.AddWeapon(equip);

			_equipped.AddItem(equip.GetArmoryDisplayString(), SkillAssets.GenerateIcon(equip.Icon));
		}

		if (primary != null) 
		{
			var children = _primaryWeaponContainer.GetChildren();
			foreach (var child in children)
				_primaryWeaponContainer.RemoveChild(child);

			var weaponDisplay = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_WEAPON_DISPLAY).Instantiate<WeaponDisplay>();
			_primaryWeaponContainer.AddChild(weaponDisplay);
			weaponDisplay.Initialize(primary);
		}

		SetSelectedIndeces();

        if (_reserves.ItemCount > 0)
			_reserves.Select(_rSelected);

		if(_equipped.ItemCount > 0)
			_equipped.Select(_eSelected);

		_totalHP.Text = $"{_armoryObject.GetTotalHP()} HP";
		_totalAtk.Text = $"{_armoryObject.GetTotalAtk()} ATK";
	}

	private void SetSelectedIndeces()
	{
		if (_rSelected >= _reserves.ItemCount) _rSelected = _reserves.ItemCount - 1;
        if (_eSelected >= _equipped.ItemCount) _eSelected = _equipped.ItemCount - 1;
        if (_rSelected < 0) _rSelected = 0;
        if (_eSelected < 0) _eSelected = 0;
    }

	private void _OnSmeltBtnPressed()
	{
		if(_reserves.ItemCount > 0 && _equipped.ItemCount > 0)
		{
			try
			{
				int rSelect = _reserves.GetSelectedItems()[0];
				int eSelect = _equipped.GetSelectedItems()[0];
				_armoryObject.SmeltReserveWeapon(rSelect, eSelect);
				UpdateUI();
			}
			catch (IndexOutOfRangeException) { }
		}
	}

	private void _OnSetPrimaryBtnPressed() 
	{
		if (_equipped.ItemCount > 0) 
		{
			int selected = _equipped.GetSelectedItems()[0];
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
            _armoryObject.ChangeGridStatus((int)index);
			UpdateUI();
        }
    }
}
