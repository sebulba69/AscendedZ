using AscendedZ;
using AscendedZ.currency;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public partial class UpgradeScreen : CenterContainer
{
	private Label _vorpexCount;
	private Label _nameLabel;
	private Label _costLabel;
	private RichTextLabel _description;
	private ItemList _partyList;
	private TextureRect _partyImage;
	private Button _upgradeButton;
	private Button _backButton;

	private List<OverworldEntity> _allPartyMembers;
	private OverworldEntity _selectedEntity;
	private int _selected;
	private Wallet _wallet;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vorpexCount = this.GetNode<Label>("%VorpexCount");
        _nameLabel = this.GetNode<Label>("%NameLabel");
        _costLabel = this.GetNode<Label>("%CostLabel");
        _description = this.GetNode<RichTextLabel>("%MemberDescription");
        _partyList = this.GetNode<ItemList>("%PartyList");
		_partyImage = this.GetNode<TextureRect>("%MemberImage");
		_upgradeButton = this.GetNode<Button>("%UpgradeButton");
        _backButton = this.GetNode<Button>("%BackButton");

		GameObject gameObject = PersistentGameObjects.GameObjectInstance();
		_wallet = gameObject.MainPlayer.Wallet;

        _selected = 0;

        _allPartyMembers = new List<OverworldEntity>();
		_allPartyMembers.AddRange(gameObject.MainPlayer.Party.Party);
		_allPartyMembers.AddRange(gameObject.MainPlayer.ReserveMembers);

		_partyList.Connect("item_selected", new Callable(this, "_OnItemSelected"));

        RefreshItemList();
    }

	private void _OnItemSelected(int selectedIndex)
	{
		_selected = selectedIndex;
		DisplaySelectedItem();
    }

	private void _OnUpgradeButtonClicked()
	{
		int cost = _selectedEntity.VorpexValue;
		Currency vorpex = _wallet.Currency[SkillAssets.VORPEX_ICON];

		if(vorpex.Amount >= cost)
		{
			vorpex.Amount -= cost;
            _selectedEntity.LevelUp();
			RefreshItemList();
			PersistentGameObjects.Save();
        }
    }

    private void RefreshItemList()
    {
        _partyList.Clear();

        foreach (var member in _allPartyMembers)
        {
            Texture2D memberImage = ResourceLoader.Load<Texture2D>(member.Image);
            _partyList.AddItem(member.Name, memberImage);
        }

        _partyList.Select(_selected);
        DisplaySelectedItem();
    }

    private void DisplaySelectedItem()
	{
		_selectedEntity = _allPartyMembers[_selected];

		_nameLabel.Text = _selectedEntity.Name;
		_costLabel.Text = $"Cost: {_selectedEntity.VorpexValue} VC";
		_partyImage.Texture = ResourceLoader.Load<Texture2D>(_selectedEntity.Image);
		_description.Text = _selectedEntity.GetUpgradeString();
		_vorpexCount.Text = _wallet.Currency[SkillAssets.VORPEX_ICON].Amount.ToString();
    }
}
