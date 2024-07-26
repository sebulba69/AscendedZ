using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection;

public partial class RecruitScreen : CenterContainer
{
    private int _cost = 1;

    /// <summary>
    /// Party members available for recruiting.
    /// </summary>
    private ItemList _availableRecruits;

    /// <summary>
    /// Display name for the selected party member.
    /// </summary>
    private Label _displayName;

    /// <summary>
    /// Display image for the selected party member.
    /// </summary>
    private TextureRect _displayImage;

    /// <summary>
    /// Display information for the selected party member.
    /// </summary>
    private Label _displayDescription;

    /// <summary>
    /// Displays the Talisman the player owns that are relevant to
    /// the selected party member in the vendor list.
    /// </summary>
    private Label _partyCoinCost;

    /// <summary>
    /// Available party members the vendor has to offer.
    /// </summary>
    private List<OverworldEntity> _availablePartyMembers;

    private Currency _partyCoins;

    private int _selected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _availableRecruits = this.GetNode<ItemList>("%PartyMemberList");
        _displayImage = this.GetNode<TextureRect>("VBoxContainer/HBoxContainer/VBoxContainer/CharContainer/CharImageBox");
        _displayName = this.GetNode<Label>("VBoxContainer/HBoxContainer/VBoxContainer/PanelContainer/CenterContainer/CharNameLabel");
        _displayDescription = this.GetNode<Label>("VBoxContainer/HBoxContainer/VBoxContainer/CharDescription/MarginContainer/CharDescriptionBox");
        _partyCoinCost = this.GetNode<Label>("%OwnedTalismans");

        Button buyButton = this.GetNode<Button>("VBoxContainer/HBoxContainer/VBoxContainer2/HBoxContainer/BuyButton");

        buyButton.Pressed += _OnBuyButtonPressed;
        _availableRecruits.ItemSelected += _OnItemSelected;
        _partyCoins = PersistentGameObjects.GameObjectInstance().MainPlayer.Wallet.Currency[SkillAssets.PARTY_COIN_ICON];
        SetShopVendorWares();
    }

    public void SetOwnedPartyCoin()
    {
        _partyCoinCost.Text = $"{_partyCoins.Amount} PC";
    }

    public void SetShopVendorWares()
    {
        _availablePartyMembers = EntityDatabase.MakeShopVendorWares(PersistentGameObjects.GameObjectInstance().MaxTier);
        _availablePartyMembers.Reverse();

        int shopLevel = PersistentGameObjects.GameObjectInstance().ShopLevel;
        for (int i = 0; i < shopLevel; i++)
        {
            foreach (var member in _availablePartyMembers) 
            {
                member.LevelUp();
            }

            _cost = shopLevel + 1;
        }

        RefreshVendorWares(0);
    }

    private void _OnItemSelected(long index)
    {
        _selected = (int)index;
        DisplayPartyMemberOnScreen(_selected);
    }

    private void _OnBuyButtonPressed()
    {
        if (_availablePartyMembers.Count == 0)
            return;

        GameObject gameObject = PersistentGameObjects.GameObjectInstance();
        MainPlayer mainPlayer = gameObject.MainPlayer;

        OverworldEntity partyMember = _availablePartyMembers[_selected];

        if (_partyCoins.Amount >= _cost)
        {
            if (mainPlayer.IsPartyMemberOwned(partyMember.Name))
                return;

            _partyCoins.Amount -= _cost;

            mainPlayer.ReserveMembers.Add(partyMember);

            RefreshVendorWares(_selected);

            if(!gameObject.PartyMemberObtained)
                gameObject.PartyMemberObtained = true;

            PersistentGameObjects.Save();
        }
    }

    private void RefreshVendorWares(int lastSelected)
    {
        _availableRecruits.Clear();

        MainPlayer mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;
        foreach(OverworldEntity availablePartyMember in _availablePartyMembers)
        {
            string owned = string.Empty;
            if (mainPlayer.IsPartyMemberOwned(availablePartyMember.Name))
                owned = " [OWNED]";

            _availableRecruits.AddItem($"{availablePartyMember.DisplayName} - {_cost} PC{owned}", CharacterImageAssets.GetTextureForItemList(availablePartyMember.Image));
        }

        if(_availablePartyMembers.Count == 0)
        {
            _displayImage.Texture = null;
            _displayName.Text = "";
            _displayDescription.Text = "";
        }
        else
        {
            DisplayPartyMemberOnScreen(lastSelected);
        }

        _partyCoinCost.Text = $"{_partyCoins.Amount} PC";
    }

    private void DisplayPartyMemberOnScreen(int index)
    {
        OverworldEntity member = _availablePartyMembers[index];
        _displayImage.Texture = ResourceLoader.Load<Texture2D>(member.Image);
        _displayName.Text = member.DisplayName;
        _displayDescription.Text = member.ToString().TrimEnd('\r','\n');
    }
}
