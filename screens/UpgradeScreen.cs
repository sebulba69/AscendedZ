using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

public partial class UpgradeScreen : CenterContainer
{
	private Label _vorpexCount;
	private Label _nameLabel;
	private RichTextLabel _description;
	private ItemList _partyList;
	private TextureRect _partyImage;
	private Button _upgradeButton;
	private Button _smeltButton;
	private Button _backButton;
	private CheckBox _ascendedCheckbox;
	private TextureRect _upgradeCurrencyIcon;

	private List<OverworldEntity> _allPartyMembers;
	private OverworldEntity _selectedEntity;
	private int _selected;
	private Wallet _wallet;
	private bool _ascendedPressed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vorpexCount = this.GetNode<Label>("%VorpexCount");
        _nameLabel = this.GetNode<Label>("%NameLabel");
        _description = this.GetNode<RichTextLabel>("%MemberDescription");
        _partyList = this.GetNode<ItemList>("%PartyList");
		_partyImage = this.GetNode<TextureRect>("%MemberImage");
		_upgradeButton = this.GetNode<Button>("%UpgradeButton");
        _smeltButton = this.GetNode<Button>("%SmeltButton");
        _backButton = this.GetNode<Button>("%BackButton");
        _ascendedCheckbox = this.GetNode<CheckBox>("%AscendBox");
		_upgradeCurrencyIcon = this.GetNode<TextureRect>("%UpgradeCurrencyIcon");

        GameObject gameObject = PersistentGameObjects.GameObjectInstance();
		_wallet = gameObject.MainPlayer.Wallet;

        _selected = 0;

        _allPartyMembers = gameObject.MainPlayer.ReserveMembers;

		_partyList.ItemSelected += _OnItemSelected;
		_ascendedPressed = false;

        _upgradeButton.Pressed += _OnUpgradeButtonPressed;
		_backButton.Pressed += _OnBackButtonPressed;

        _upgradeButton.Text = "Upgrade";

        _ascendedCheckbox.Visible = (gameObject.MaxTier > TierRequirements.QUESTS_PARTY_MEMBERS_UPGRADE);
        _ascendedCheckbox.Pressed += _OnAscendedPressed;

		_ascendedPressed = false;

        RefreshItemList();
    }

	private void _OnItemSelected(long selectedIndex)
	{
		_selected = (int)selectedIndex;
		DisplaySelectedItem();
    }

	private void _OnUpgradeButtonPressed()
	{
		if (!_ascendedPressed)
        {
            int cost = _selectedEntity.VorpexValue;
            Currency vorpex = _wallet.Currency[SkillAssets.VORPEX_ICON];

            if (vorpex.Amount >= cost && !_selectedEntity.GradeCapHit)
            {
                vorpex.Amount -= cost;
                _selectedEntity.LevelUp();

                RefreshItemList();

                PersistentGameObjects.Save();
            }
        }
		else
		{
            // We're Ascending
            int cost = _selectedEntity.UpgradeShardValue;
            Currency upgradeShards = _wallet.Currency[SkillAssets.UPGRADESHARD_ICON];

            if (upgradeShards.Amount >= cost && _selectedEntity.CanAscend())
			{
                upgradeShards.Amount -= cost;
                _selectedEntity.Ascend();

                RefreshItemList();

                PersistentGameObjects.Save();
            }
        }
    }

	private void _OnBackButtonPressed()
	{
		this.QueueFree();
	}

	private void _OnAscendedPressed()
	{
		var gameObject = PersistentGameObjects.GameObjectInstance();
		if (!gameObject.ProgressFlagObject.AscensionViewed)
		{
			gameObject.ProgressFlagObject.AscensionViewed = true;
            PersistentGameObjects.Save();
        }

        _ascendedPressed = !_ascendedPressed;

		_smeltButton.Visible = _ascendedPressed;

		if (_smeltButton.Visible)
		{
            _smeltButton.Text = $"Smelt [{_allPartyMembers.Count - 4}/4]";
        }

        _upgradeButton.Text = (_ascendedPressed) ? "Ascend" : "Upgrade";

        _ascendedCheckbox.Text = (_ascendedPressed) ? "Upgrade": "Ascend";

        RefreshItemList();
    }

    private void RefreshItemList()
    {
        _partyList.Clear();

        foreach (var member in _allPartyMembers)
        {
			string displayName;

			if(_ascendedPressed)
				displayName = $"{member.DisplayName} [{member.UpgradeShardYield} US]";
			else
				displayName = $"{member.DisplayName} [{member.VorpexValue} VC]";

            _partyList.AddItem(displayName, CharacterImageAssets.GetTextureForItemList(member.Image));
        }

        _partyList.Select(_selected);
        DisplaySelectedItem();
    }

    private void DisplaySelectedItem()
	{
		_selectedEntity = _allPartyMembers[_selected];

		_nameLabel.Text = _selectedEntity.DisplayName;
		_partyImage.Texture = ResourceLoader.Load<Texture2D>(_selectedEntity.Image);
		_description.Text = _selectedEntity.GetUpgradeString(_ascendedPressed);

		string icon = (_ascendedPressed) ? SkillAssets.UPGRADESHARD_ICON : SkillAssets.VORPEX_ICON;

        _upgradeCurrencyIcon.Texture = SkillAssets.GenerateIcon(icon);
        _vorpexCount.Text = _wallet.Currency[icon].Amount.ToString();
    }
}
