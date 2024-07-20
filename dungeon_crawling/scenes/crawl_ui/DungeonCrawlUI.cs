using AscendedZ;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonCrawlUI : PanelContainer
{
	private VBoxContainer _container;
	private Label _tier, _coordinates, _encounters;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tier = GetNode<Label>("%Tier");
        _coordinates = GetNode<Label>("%Coordinates");
		_container = GetNode<VBoxContainer>("%PartyContainer");
		_encounters = GetNode<Label>("%EncountersRemaining");
	}

	public void SetParty(int tier, List<BattlePlayer> players, int orbs, int encounters)
	{
		_tier.Text = $"TIER: {tier}, Orbs: {orbs}";

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

	public void SetCoordinates(int x, int y)
	{
        _coordinates.Text = $"[{x},{y}]";
    }
}
