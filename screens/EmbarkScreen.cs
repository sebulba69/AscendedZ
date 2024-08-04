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
public partial class EmbarkScreen : TextureRect
{
    private Label _tierLabel;
    private PartyEditScreen _partyEditScreen;
    private Button _endlessDungeonBtn, _labrybuceBtn;

    public bool Embark { get; set; }

    public bool DungeonCrawling { get => _partyEditScreen.DungeonCrawling; }

    public override void _Ready()
    {
        AddUserSignal("CloseEmbarkScreen");

        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        _tierLabel = this.GetNode<Label>("%TierLabel");

        Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetBackground(gameObject.MaxTier));

        Button leftTier = this.GetNode<Button>("%LeftTierBtn");
        Button rightBtn = this.GetNode<Button>("%RightTierBtn");
        Button left10Btn = this.GetNode<Button>("%LeftTier10Btn");
        Button right10Btn = this.GetNode<Button>("%RightTier10Btn");
        _endlessDungeonBtn = this.GetNode<Button>("%EndlessDungeonBtn");
        _labrybuceBtn = this.GetNode<Button>("%LabribuceBtn");

        _labrybuceBtn.Visible = (gameObject.MaxTier >= 10);

        gameObject.Tier = gameObject.MaxTier;
        gameObject.TierDC = gameObject.MaxTierDC;
        string tierText = "Dungeon Floor:";

        _tierLabel.Text = $"{tierText} {PersistentGameObjects.GameObjectInstance().MaxTier}";

        // on click events
        left10Btn.Pressed += () => 
        {
            for (int i = 0; i < 10; i++)
                _OnLeftBtnPressed();
        };

        right10Btn.Pressed += () =>
        {
            for (int i = 0; i < 10; i++)
                _OnRightBtnPressed();
        };

        leftTier.Pressed += _OnLeftBtnPressed;
        rightBtn.Pressed += _OnRightBtnPressed;
        
        _labrybuceBtn.Pressed += _OnLabrybuceButtonPressed;
        _endlessDungeonBtn.Pressed += _OnEndlessDungeonButtonPressed;

        _partyEditScreen = this.GetNode<PartyEditScreen>("%PartyEditScreen");
        _partyEditScreen.DoEmbark += _OnEmbarkPressed;
    }

    private void _OnEmbarkPressed(object sender, bool embarkPressed)
    {
        Embark = embarkPressed;
        Visible = false;
        EmitSignal("CloseEmbarkScreen");
    }

    private void _OnLabrybuceButtonPressed()
    {
        var gameObject = PersistentGameObjects.GameObjectInstance();
        _endlessDungeonBtn.Disabled = false;
        _labrybuceBtn.Disabled = true;

        SetTierText(gameObject.TierDC);
        _partyEditScreen.DungeonCrawling = true;
    }

    private void _OnEndlessDungeonButtonPressed()
    {
        var gameObject = PersistentGameObjects.GameObjectInstance();
        _endlessDungeonBtn.Disabled = true;
        _labrybuceBtn.Disabled = false;
        SetTierText(gameObject.Tier);
        _partyEditScreen.DungeonCrawling = false;
    }

    private void _OnLeftBtnPressed()
    {
        var gameObject = PersistentGameObjects.GameObjectInstance();
        if (_labrybuceBtn.Disabled) 
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
        if (_labrybuceBtn.Disabled)
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
        string tierText = (!_labrybuceBtn.Disabled) ? "Dungeon Floor:" : "Labrybuce Sector:";
        _tierLabel.Text = $"{tierText} {tier}";
    }

}
