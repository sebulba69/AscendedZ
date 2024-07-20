using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.upgrade_screen;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

public partial class UpgradeScreen : CenterContainer
{
	private Label _vorpexCount, _partyCoinCount;
	private Label _nameLabel;
	private RichTextLabel _description;
	private ItemList _partyList;
	private TextureRect _partyImage;
	private Button _upgradeButton, _refundButton;
	private Button _backButton;

    private int _selected;
    private UpgradeScreenObject _upgradeScreenObject;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vorpexCount = this.GetNode<Label>("%VorpexCount");
		_partyCoinCount = GetNode<Label>("%OwnedTalismans");
        _nameLabel = this.GetNode<Label>("%NameLabel");
        _description = this.GetNode<RichTextLabel>("%MemberDescription");
        _partyList = this.GetNode<ItemList>("%PartyList");
		_partyImage = this.GetNode<TextureRect>("%MemberImage");
		_upgradeButton = this.GetNode<Button>("%UpgradeButton");
		_refundButton = this.GetNode<Button>("%RefundButton");
        _backButton = this.GetNode<Button>("%BackButton");

        _selected = 0;
        _upgradeScreenObject = new UpgradeScreenObject();

		_partyList.ItemSelected += _OnItemSelected;
        _upgradeButton.Pressed += _OnUpgradeButtonPressed;
        _refundButton.Pressed += _OnRefundButtonPressed;
		_backButton.Pressed += _OnBackButtonPressed;

        RefreshItemList();
    }

	private void _OnItemSelected(long selectedIndex)
	{
		_selected = (int)selectedIndex;
		_upgradeScreenObject.ChangeSelected(_selected);
		DisplaySelectedItem();
    }

	private void _OnUpgradeButtonPressed()
	{
        _upgradeScreenObject.Upgrade();
        RefreshItemList();
    }

	private void _OnRefundButtonPressed()
	{
		_upgradeScreenObject.Refund();
		RefreshItemList();
	}

	private void _OnBackButtonPressed()
	{
		this.QueueFree();
	}

    private void RefreshItemList()
    {
        _partyList.Clear();

		var displays = _upgradeScreenObject.GetUpgradeItemListDisplays();

		foreach (var display in displays)
		{
            _partyList.AddItem(display.PartyMemberEntry, CharacterImageAssets.GetTextureForItemList(display.PartyMemberImage));
        }

		if (_selected >= _partyList.ItemCount)
			_selected = _partyList.ItemCount - 1;

        _partyList.Select(_selected);
		_upgradeScreenObject.ChangeSelected(_selected);
        DisplaySelectedItem();

		_refundButton.Visible = (displays.Count > 3);
    }

    private void DisplaySelectedItem()
	{
		var upgradeDisplay = _upgradeScreenObject.GetUpgradeScreenDisplay();

		_nameLabel.Text = upgradeDisplay.DisplayName;
		_partyImage.Texture = ResourceLoader.Load<Texture2D>(upgradeDisplay.Image);
		_description.Text = upgradeDisplay.SelectedUpgradeString;
		_vorpexCount.Text = upgradeDisplay.VorpexAmount;
        _partyCoinCount.Text = upgradeDisplay.PartyCoinAmount;


    }
}
