using AscendedZ;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonCrawlUI : Control
{
	private GridContainer _container;
	private Label _tier, _coordinates, _encounters, _orbs, _pickaxes, _endCoords;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tier = GetNode<Label>("%Tier");
        _coordinates = GetNode<Label>("%Coordinates");
		_container = GetNode<GridContainer>("%PartyContainer");
		_encounters = GetNode<Label>("%EncountersRemaining");
		_endCoords = GetNode<Label>("%EndCoords");
		_orbs = GetNode<Label>("%OrbAmount");
        _pickaxes = GetNode<Label>("%PickAxeAmount");
	}

	public void SetParty(int tier, List<BattlePlayer> players, int orbs, int pickaxes, int encounters)
	{
		_tier.Text = $"TIER: {tier}";

		_orbs.Text = orbs.ToString();
		_pickaxes.Text = pickaxes.ToString();

        foreach (var child in _container.GetChildren())
			_container.RemoveChild(child);

		foreach (var p in players)
		{
			var scene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_CRAWL_PARTY_MEMBER).Instantiate<PartyMemberDCDisplay>();
			_container.AddChild(scene);
			scene.SetPartyMember(p);
		}

		_encounters.Text = $"Required Encounters: {encounters}";
    }

	public void SetExit(int x, int y)
	{
        _endCoords.Text = $"EXIT: [{y},{x}]";
    }

	public void SetCoordinates(int x, int y)
	{
        _coordinates.Text = $"COORDS: [{y},{x}]";
    }
}
