using AscendedZ;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonCrawlUI : PanelContainer
{
	private VBoxContainer _container;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_container = GetNode<VBoxContainer>("%PartyContainer");
	}

	public void SetParty(List<BattlePlayer> players)
	{
		foreach (var child in _container.GetChildren())
			_container.RemoveChild(child);

		foreach (var p in players)
		{
			var scene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_CRAWL_PARTY_MEMBER).Instantiate<PartyMemberDCDisplay>();
			_container.AddChild(scene);
			scene.SetPartyMember(p);
		}
	}
}
