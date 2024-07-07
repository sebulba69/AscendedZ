using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ;
using Godot;
using System;
using static Godot.WebSocketPeer;
using System.Reflection;

public partial class ArmoryScene : CenterContainer
{
	private ItemList _reserves, _equipped;
	private Label _totalHP, _totalAtk;
	private PanelContainer _primaryWeaponContainer;
	private WeaponGridDisplay _weaponGridDisplay;
	private Button _smeltBtn, _setPrimaryBtn;

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
	}

	// Select a weapon to be equipped
	private void _OnReserveItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if (mouse_button_index == (long)MouseButton.Left)
        {

        }
    }

	// Select a weapon to be used for leveling up.
    private void _OnEquippedItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if (mouse_button_index == (long)MouseButton.Left)
        {

        }
    }
}
