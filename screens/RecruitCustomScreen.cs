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
	private Label _ownedPartyCoin;
	private Label _costLabel, _description;

	private int _selectedIndexMembers = 0;
	private int _selectedIndexSkills = 0;

	private RecruitCustomObject _recruitCustomObject;
	private GameObject _gameObject;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_partyMemberDisplay = this.GetNode("%PartyMemberDisplay");
		_potentialSkills = this.GetNode<ItemList>("%PotentialSkills");
		_potentialPartyMembers = this.GetNode<ItemList>("%PotentialMembers");
		_ownedPartyCoin = this.GetNode<Label>("%OwnedPartyCoin");
        _costLabel = this.GetNode<Label>("%CostLabel");
        _description = this.GetNode<Label>("%Description");

        _potentialPartyMembers.Connect("item_selected",new Callable(this, "_OnRecruitSelected"));
		_potentialSkills.ItemSelected += _OnSkillSelected;

        _gameObject = PersistentGameObjects.GameObjectInstance();

        _recruitCustomObject = new RecruitCustomObject();
		_recruitCustomObject.Initialize();

		ChangePotentialPartyMembers();
        ChangePotentialSkills();

        Button buyButton = this.GetNode<Button>("%BuyButton");
        Button addButton = this.GetNode<Button>("%AddButton");
        Button removeButton = this.GetNode<Button>("%RemoveButton");

		buyButton.Pressed += () => 
		{
			var mainPlayer = _gameObject.MainPlayer;
            var partyCoin = mainPlayer.Wallet.Currency[SkillAssets.PARTY_COIN_ICON];
			var selected = _recruitCustomObject.SelectedEntity;

            bool canAfford = (partyCoin.Amount >= _recruitCustomObject.Cost);
			bool isOwnedByPlayer = (mainPlayer.IsPartyMemberOwned(selected.Name));
			bool hasSkills = _recruitCustomObject.SelectedEntity.Skills.Count > 0;

			if(canAfford && !isOwnedByPlayer && hasSkills)
			{
				partyCoin.Amount -= _recruitCustomObject.Cost;

				// prevent any references in memory back to this screen
				var partyMember = PartyMemberGenerator.MakePartyMember(selected.Name);

				partyMember.Skills.Clear();

				for (int i = 0; i < _gameObject.ShopLevel; i++)
					partyMember.LevelUp();

				foreach(var skill in selected.Skills)
					partyMember.Skills.Add(skill.Clone());

				mainPlayer.ReserveMembers.Add(partyMember);
				ChangePotentialPartyMembers();
                
				PersistentGameObjects.Save();
            }

			_ownedPartyCoin.Text = $"{partyCoin.Amount} PC";
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

		_ownedPartyCoin.Text = $"{_gameObject.MainPlayer.Wallet.Currency[SkillAssets.PARTY_COIN_ICON].Amount} PC";
    }

    public void SetShopVendorWares()
    {
		_recruitCustomObject.Initialize();
        ChangePotentialPartyMembers();
        ChangePotentialSkills();
    }

	public void SetOwnedPartyCoin()
	{
        _ownedPartyCoin.Text = $"{_gameObject.MainPlayer.Wallet.Currency[SkillAssets.PARTY_COIN_ICON].Amount} PC";
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

            _potentialPartyMembers.AddItem($"{availablePartyMember.DisplayName}{owned}", CharacterImageAssets.GetTextureForItemList(availablePartyMember.Image));
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

	private void _OnSkillSelected(long index)
	{
		_selectedIndexSkills = (int)index;
		_description.Text = _recruitCustomObject.GetSkillDescription(_selectedIndexSkills);
    }

    private void ShowPreviewPartyMember()
    {
        _partyMemberDisplay.Call("Clear");
        _partyMemberDisplay.Call("ShowRandomEntity", new EntityUIWrapper { Entity = _recruitCustomObject.SelectedEntity });
        UpdateCost();
    }

	private void UpdateCost()
	{
		_costLabel.Text = $"Cost: {_recruitCustomObject.Cost} PC";
	}
}
