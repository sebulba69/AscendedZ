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

    public override void _Ready()
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        _tierLabel = this.GetNode<Label>("%TierLabel");

        Button leftTier = this.GetNode<Button>("%LeftTierBtn");
        Button rightBtn = this.GetNode<Button>("%RightTierBtn");
        Button embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button backButton = this.GetNode<Button>("%BackButton");

        gameObject.Tier = gameObject.MaxTier;
        string tierText = "Dungeon Floor:";
        
        _tierLabel.Text = $"{tierText} {PersistentGameObjects.GameObjectInstance().MaxTier}";

        // on click events
        leftTier.Pressed += () => 
        {
            PersistentGameObjects.GameObjectInstance().Tier--;
            _tierLabel.Text = $"{tierText} {PersistentGameObjects.GameObjectInstance().Tier}";
        };

        rightBtn.Pressed += () => 
        {
            PersistentGameObjects.GameObjectInstance().Tier++;
            _tierLabel.Text = $"{tierText} {PersistentGameObjects.GameObjectInstance().Tier}"; 
        };

        embarkButton.Pressed += _OnEmbarkPressed;
        backButton.Pressed += _OnBackButtonClicked;

        this.GetNode<Label>("%Tooltip").Text = "Create a party and ascend the Dungeon Tiers.";
    }

    private void _OnEmbarkPressed()
    {
        var go = PersistentGameObjects.GameObjectInstance();
        
        if (go.Tier == go.TierCap)
            return;

        bool canEmbark = false;

        foreach(OverworldEntity member in go.MainPlayer.Party.Party)
        {
            // you need at least 1 party member to embark
            if(member != null)
            {
                canEmbark = true;
                break;
            }
        }

        if (canEmbark)
        {
            this.GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>(Scenes.BATTLE_SCENE).Instantiate());

            // notify parent too
            this.QueueFree();
        }
        else
        {
            this.GetNode<Label>("%Tooltip").Text = "You need to have at least 1 Party Member equipped to Embark.";
        }
    }

    private void _OnBackButtonClicked()
    {
        this.QueueFree();
    }

}
