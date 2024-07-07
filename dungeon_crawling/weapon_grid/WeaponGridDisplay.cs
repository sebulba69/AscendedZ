using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using Godot;
using System;

public partial class WeaponGridDisplay : PanelContainer
{
	private GridContainer _weaponGridContainer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_weaponGridContainer = this.GetNode<GridContainer>("%WeaponGridContainer");
	}
}
