using AscendedZ;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Godot.WebSocketPeer;

/// <summary>
/// This class is focused on managing Reserve Party members.
/// How the UI displays characters *in* the party is handled in InPartyMemberContainer.cs.
/// </summary>
public partial class EmbarkScreen : CenterContainer
{
    private Label _tierLabel;
    private bool _dungeonCrawling;
    private PartyEditScreen _partyEditScreen;
    private Button _labrybuceBtn;

    public override void _Ready()
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        _tierLabel = this.GetNode<Label>("%TierLabel");

        Button leftTier = this.GetNode<Button>("%LeftTierBtn");
        Button rightBtn = this.GetNode<Button>("%RightTierBtn");
        _labrybuceBtn = this.GetNode<Button>("%DungeonCrawlBtn");

        _labrybuceBtn.Visible = (gameObject.MaxTier > 10);

        gameObject.Tier = gameObject.MaxTier;
        string tierText = "Dungeon Floor:";

        _tierLabel.Text = $"{tierText} {PersistentGameObjects.GameObjectInstance().MaxTier}";

        // on click events
        leftTier.Pressed += _OnLeftBtnPressed;
        rightBtn.Pressed += _OnRightBtnPressed;
        _labrybuceBtn.Pressed += _OnLabrybuceButtonPressed;

        _partyEditScreen = this.GetNode<PartyEditScreen>("%PartyEditScreen");
        _partyEditScreen.TreeExited += () => 
        {
            this.QueueFree();
        };

        this.GetNode<Label>("%Tooltip").Text = "Create a party and ascend the Dungeon Tiers.";
    }

    private void _OnLabrybuceButtonPressed()
    {
        _dungeonCrawling = !_dungeonCrawling;
        var gameObject = PersistentGameObjects.GameObjectInstance();
        if (_dungeonCrawling)
        {
            _labrybuceBtn.Text = "Endless Dungeon";
            SetTierText(gameObject.TierDC);
        }
        else
        {
            _labrybuceBtn.Text = "The Labrybuce";
            SetTierText(gameObject.Tier);
        }
        _partyEditScreen.DungeonCrawling = _dungeonCrawling;
    }

    private void _OnLeftBtnPressed()
    {
        var gameObject = PersistentGameObjects.GameObjectInstance();
        if (_dungeonCrawling) 
        {
            gameObject.TierDC--;
            SetTierText(gameObject.TierDC);
        }
        else 
        {
            gameObject.Tier--;
            SetTierText(gameObject.Tier);
        }
    }

    private void _OnRightBtnPressed() 
    {
        var gameObject = PersistentGameObjects.GameObjectInstance();
        if (_dungeonCrawling)
        {
            gameObject.TierDC++;
            SetTierText(gameObject.TierDC);
        }
        else
        {
            gameObject.Tier++;
            SetTierText(gameObject.Tier);
        }
    }

    private void SetTierText(int tier)
    {
        string tierText = (!_dungeonCrawling) ? "Dungeon Floor:" : "Labrybuce Sector:";
        _tierLabel.Text = $"{tierText} {tier}";
    }

}
