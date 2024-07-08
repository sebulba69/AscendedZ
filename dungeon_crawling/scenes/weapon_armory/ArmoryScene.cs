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

        Button back = this.GetNode<Button>("%BackBtn");
		back.Pressed += () => this.QueueFree();

		var go = PersistentGameObjects.GameObjectInstance();
		_armoryObject = new ArmoryObject(go.MainPlayer.DungeonPlayer);

		UpdateUI();
	}

	private void UpdateUI()
	{
		var reserves = _armoryObject.GetReserves();
		var equipped = _armoryObject.GetEquipped();

		_reserves.Clear();
		_equipped.Clear();
		_weaponGridDisplay.Clear();

		foreach(var reserve in reserves)
		{
            _reserves.AddItem(reserve.GetArmoryDisplayString(), SkillAssets.GenerateIcon(reserve.Icon));
        }

		foreach(var equip in equipped)
		{
			_weaponGridDisplay.AddWeapon(equip);
			_equipped.AddItem(equip.GetArmoryDisplayString(), SkillAssets.GenerateIcon(equip.Icon));
		}
	}

	private void OnSmeltBtnPressed()
	{
		
	}

	private void OnSetPrimaryBtnPressed() 
	{ 

	}

	// Select a weapon to be equipped
	private void _OnReserveItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if (mouse_button_index == (long)MouseButton.Right)
        {
            _armoryObject.ChangeGridStatus((int)index);
			UpdateUI();
        }
    }
}
