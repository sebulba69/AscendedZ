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
using System.Reflection;

public partial class RecruitScreen : CenterContainer
{
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
    private TextEdit _vorpexCost;

    /// <summary>
    /// Available party members the vendor has to offer.
    /// </summary>
    private List<OverworldEntity> _availablePartyMembers;

    private Currency _vorpex;

    private int _selected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _availableRecruits = this.GetNode<ItemList>("%PartyMemberList");
        _displayImage = this.GetNode<TextureRect>("VBoxContainer/HBoxContainer/VBoxContainer/CharContainer/CharImageBox");
        _displayName = this.GetNode<Label>("VBoxContainer/HBoxContainer/VBoxContainer/PanelContainer/CenterContainer/CharNameLabel");
        _displayDescription = this.GetNode<Label>("VBoxContainer/HBoxContainer/VBoxContainer/CharDescription/MarginContainer/CharDescriptionBox");
        _vorpexCost = this.GetNode<TextEdit>("VBoxContainer/HBoxContainer/VBoxContainer2/HBoxContainer/OwnedTalismans");

        Button buyButton = this.GetNode<Button>("VBoxContainer/HBoxContainer/VBoxContainer2/HBoxContainer/BuyButton");
        Button backButton = this.GetNode<Button>("VBoxContainer/HBoxContainer/VBoxContainer2/HBoxContainer/BackButton");

        buyButton.Pressed += _OnBuyButtonPressed;
        backButton.Pressed += _OnBackButtonPressed;

        _availablePartyMembers = EntityDatabase.MakeShopVendorWares(PersistentGameObjects.Instance().MaxTier);
        _availablePartyMembers.Reverse();
        _availableRecruits.Connect("item_selected",new Callable(this,"_OnItemSelected"));

        _vorpex = PersistentGameObjects.Instance().MainPlayer.Wallet.Currency[SkillAssets.VORPEX_ICON];
        RefreshVendorWares(0);
    }

    private void _OnItemSelected(int index)
    {
        _selected = index;
        DisplayPartyMemberOnScreen(index);
    }

    private void _OnBuyButtonPressed()
    {
        if (_availablePartyMembers.Count == 0)
            return;

        GameObject instance = PersistentGameObjects.Instance();
        MainPlayer mainPlayer = instance.MainPlayer;

        OverworldEntity partyMember = _availablePartyMembers[_selected];

        if (_vorpex.Amount >= partyMember.ShopCost)
        {
            if (mainPlayer.IsPartyMemberOwned(partyMember.Name))
            {
                return;
            }

            _vorpex.Amount -= partyMember.ShopCost;
            mainPlayer.ReserveMembers.Add(partyMember);

            RefreshVendorWares(_selected);
            PersistentGameObjects.Save();
        }
    }

    private void RefreshVendorWares(int lastSelected)
    {
        _availableRecruits.Clear();

        MainPlayer mainPlayer = PersistentGameObjects.Instance().MainPlayer;
        foreach(OverworldEntity availablePartyMember in _availablePartyMembers)
        {
            string owned = string.Empty;
            if (mainPlayer.IsPartyMemberOwned(availablePartyMember.Name))
                owned = " [OWNED]";

            _availableRecruits.AddItem($"{availablePartyMember.Name} - {availablePartyMember.ShopCost} VC{owned}");
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

        _vorpexCost.Text = $"{_vorpex.Amount} VC";
    }

    private void DisplayPartyMemberOnScreen(int index)
    {
        OverworldEntity member = _availablePartyMembers[index];
        _displayImage.Texture = ResourceLoader.Load<Texture2D>(member.Image);
        _displayName.Text = member.Name;
        _displayDescription.Text = member.ToString().TrimEnd('\r','\n');
    }

    private void _OnBackButtonPressed()
    {
        this.QueueFree();
    }

}
