using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities.enemy_objects;
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
	private ItemList _partyList;
	private Button _backButton;
	private int _selected;
	private UpgradeItem _item;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vorpexCount = this.GetNode<Label>("%VorpexCount");
		_partyCoinCount = GetNode<Label>("%OwnedTalismans");
        _partyList = this.GetNode<ItemList>("%ItemList");
        _backButton = this.GetNode<Button>("%BackButton");
		_backButton.Pressed += _OnBackButtonPressed;
		_item = GetNode<UpgradeItem>("%UpgradeItem");
		_selected = 0;

		_item.UpdatePartyDisplay += _OnUpdatePartyDisplay;

		_partyList.ItemSelected += _OnItemSelected;

		RefreshReserveList();
    }

	private void RefreshReserveList()
	{
		_partyList.Clear();

        var mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;
        var reserves = mainPlayer.ReserveMembers;
        var currency = mainPlayer.Wallet.Currency;

        foreach (var reserve in reserves)
        {
            _partyList.AddItem(reserve.DisplayName, CharacterImageAssets.GetTextureForItemList(reserve.Image)); ;
        }

		if (_selected >= reserves.Count)
			_selected = reserves.Count - 1;

		_partyList.Select(_selected);
		_item.Initialize(reserves[_selected]);
        _vorpexCount.Text = currency[SkillAssets.VORPEX_ICON].Amount.ToString();
        _partyCoinCount.Text = currency[SkillAssets.PARTY_COIN_ICON].Amount.ToString();
    }

	private void _OnItemSelected(long index)
	{
		_selected = (int)index;
        var reserves = PersistentGameObjects.GameObjectInstance().MainPlayer.ReserveMembers;
		_item.Initialize(reserves[(int)index]);
	}

    private void _OnUpdatePartyDisplay(object sender, EventArgs e)
    {
		RefreshReserveList();
    }

	private void _OnBackButtonPressed()
	{
		this.QueueFree();
	}
}
