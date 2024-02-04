using AscendedZ.entities.partymember_objects;
using Godot;
using System;

public partial class RecruitCustomScreen : CenterContainer
{
	private Node _partyMemberDisplay;
	private ItemList _potentialSkills;
	private ItemList _potentialPartyMembers;
	private TextEdit _ownedVorpex;
	private Label _costLabel;

	private OverworldEntity _overworldEntity;
	private int _cost = 0;
	private int _selectedMember;
	private int _selectedSkill;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_partyMemberDisplay = this.GetNode("%PartyMemberDisplay");
		_potentialSkills = this.GetNode<ItemList>("%PotentialSkills");
		_potentialPartyMembers = this.GetNode<ItemList>("%PotentialMembers");
	}

	private void PopulatePartyMemberList()
	{

	}
}
