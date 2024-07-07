using AscendedZ;
using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using Godot;
using System;

public partial class WeaponDisplay : VBoxContainer
{
	private Icon _weaponIcon, _elementIcon;
	private Label _hp, _attack;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_weaponIcon = this.GetNode<Icon>("%WeaponIcon");
		_elementIcon = this.GetNode<Icon>("%ElementIcon");
		_hp = this.GetNode<Label>("%HPLabel");
		_attack = this.GetNode<Label>("%AttackLabel");
	}

	public void Initialize(Weapon weapon)
	{
		string elementIcon = SkillAssets.GetElementIconByElementEnum(weapon.Element);
		
		_weaponIcon.SetIcon(weapon.Icon);
		_elementIcon.SetIcon(elementIcon);
		_hp.Text = $"{weapon.HP} HP";
		_attack.Text = $"{weapon.Attack} ATK";
	}
}
