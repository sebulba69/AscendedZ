using AscendedZ;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using Godot;
using System;

public partial class RecruitCustomScreen : CenterContainer
{
	private Node _partyMemberDisplay;
	private ItemList _potentialSkills;
	private ItemList _potentialPartyMembers;
	private TextEdit _ownedVorpex;
	private Label _costLabel;

	private int _selectedIndexMembers = 0;
	private int _selectedIndexSkills = 0;

	private RecruitCustomObject _recruitCustomObject;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_partyMemberDisplay = this.GetNode("%PartyMemberDisplay");
		_potentialSkills = this.GetNode<ItemList>("%PotentialSkills");
		_potentialPartyMembers = this.GetNode<ItemList>("%PotentialMembers");
		_ownedVorpex = this.GetNode<TextEdit>("%OwnedVorpex");
        _costLabel = this.GetNode<Label>("%CostLabel");

        _potentialPartyMembers.Connect("item_selected",new Callable(this, "_OnRecruitSelected"));
        _potentialSkills.Connect("item_selected",new Callable(this, "_OnSkillSelected"));

        var gameObject = PersistentGameObjects.GameObjectInstance();

        _recruitCustomObject = new RecruitCustomObject();
		_recruitCustomObject.Initialize();

		ChangePotentialPartyMembers();
        ChangePotentialSkills();

        Button buyButton = this.GetNode<Button>("%BuyButton");
        Button addButton = this.GetNode<Button>("%AddButton");
        Button removeButton = this.GetNode<Button>("%RemoveButton");

		buyButton.Pressed += () => 
		{
			var mainPlayer = gameObject.MainPlayer;
            var vorpex = mainPlayer.Wallet.Currency[SkillAssets.VORPEX_ICON];
			var selected = _recruitCustomObject.SelectedEntity;

            bool canAfford = (vorpex.Amount >= _recruitCustomObject.Cost);
			bool isOwnedByPlayer = (mainPlayer.IsPartyMemberOwned(selected.Name));
			bool hasSkills = _recruitCustomObject.SelectedEntity.Skills.Count > 0;

			if(canAfford && !isOwnedByPlayer && hasSkills)
			{
				vorpex.Amount -= _recruitCustomObject.Cost;

				// prevent any references in memory back to this screen
				var partyMember = PartyMemberGenerator.MakePartyMember(selected.Name);
				foreach(var skill in selected.Skills)
					partyMember.Skills.Add(skill.Clone());

				mainPlayer.ReserveMembers.Add(partyMember);
				ChangePotentialPartyMembers();
                
				PersistentGameObjects.Save();
            }

			_ownedVorpex.Text = $"{vorpex.Amount} VC";
        };

		addButton.Pressed += () => 
		{ 
			_recruitCustomObject.AddSkill(_selectedIndexSkills);
			ShowPreviewPartyMember();
			UpdateCost();
        };

		removeButton.Pressed += () => 
		{
            _recruitCustomObject.RemoveSkill(_selectedIndexSkills);
			ShowPreviewPartyMember();
			UpdateCost();
        };

		_ownedVorpex.Text = $"{gameObject.MainPlayer.Wallet.Currency[SkillAssets.VORPEX_ICON].Amount} VC";
    }

	private void ChangePotentialPartyMembers()
	{
		_potentialPartyMembers.Clear();

		var mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;

        foreach (var availablePartyMember in _recruitCustomObject.AvailableMembers)
        {
            string owned = string.Empty;
            if (mainPlayer.IsPartyMemberOwned(availablePartyMember.Name))
                owned = " [OWNED]";

            _potentialPartyMembers.AddItem($"{availablePartyMember.DisplayName} - {(int)(availablePartyMember.ShopCost * 1.5)} VC{owned}", CharacterImageAssets.GetTextureForItemList(availablePartyMember.Image));
        }

		if (_selectedIndexMembers >= _potentialPartyMembers.ItemCount)
			_selectedIndexMembers = _potentialPartyMembers.ItemCount - 1;

		_potentialPartyMembers.Select(_selectedIndexMembers);

        ShowPreviewPartyMember();
    }

	private void ChangePotentialSkills()
	{
		_potentialSkills.Clear();

        var skills = _recruitCustomObject.AvailableSkills;
        foreach (var skill in skills)
            _potentialSkills.AddItem(skill.Name, SkillAssets.GenerateIcon(skill.Icon));

		if(_selectedIndexSkills >= _potentialSkills.ItemCount)
			_selectedIndexSkills = _potentialSkills.ItemCount - 1;

		_potentialSkills.Select(_selectedIndexSkills);
    }

	private void _OnRecruitSelected(int index)
	{
		_selectedIndexMembers = index;
		_recruitCustomObject.SetPreviewPartyMember(index);

		ChangePotentialSkills();
		ShowPreviewPartyMember();
    }

	private void _OnSkillSelected(int index)
	{
		_selectedIndexSkills = index;
    }

    private void ShowPreviewPartyMember()
    {
        _partyMemberDisplay.Call("Clear");
        _partyMemberDisplay.Call("ShowRandomEntity", new EntityUIWrapper { Entity = _recruitCustomObject.SelectedEntity });
        UpdateCost();
    }

	private void UpdateCost()
	{
		_costLabel.Text = $"Cost: {_recruitCustomObject.Cost} VC";
	}
}
