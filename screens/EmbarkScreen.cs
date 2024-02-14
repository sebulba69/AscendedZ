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
    private PartyMemberDisplay _reservePreviewMember;
    private ItemList _reserveItemList;
   
    // party buttons
    private List<PartyMemberDisplay> _partyMemberDisplayNodes;

    List<OverworldEntity> _reserves;
    private PlayerParty _party;
    private int _selectedIndex;

    public override void _Ready()
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        _tierLabel = this.GetNode<Label>("%TierLabel");
        _reservePreviewMember = this.GetNode<PartyMemberDisplay>("%Preview");
        _reserveItemList = this.GetNode<ItemList>("%InReserveMembers");
        _party = gameObject.MainPlayer.Party;
        _reserves = gameObject.MainPlayer.ReserveMembers;
        _partyMemberDisplayNodes = new List<PartyMemberDisplay>();
        _selectedIndex = 0;

        Button leftTier = this.GetNode<Button>("%LeftTierBtn");
        Button rightBtn = this.GetNode<Button>("%RightTierBtn");
        Button embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button backButton = this.GetNode<Button>("%BackButton");

        PartyMemberDisplay pm1 = this.GetNode<PartyMemberDisplay>("%PM1");
        PartyMemberDisplay pm2 = this.GetNode<PartyMemberDisplay>("%PM2");
        PartyMemberDisplay pm3 = this.GetNode<PartyMemberDisplay>("%PM3");
        PartyMemberDisplay pm4 = this.GetNode<PartyMemberDisplay>("%PM4");

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

        _reserveItemList.Connect("item_selected",new Callable(this,"_OnItemSelected"));

        embarkButton.Pressed += _OnEmbarkPressed;
        backButton.Pressed += _OnBackButtonClicked;

        _partyMemberDisplayNodes.Add(pm1);
        _partyMemberDisplayNodes.Add(pm2);
        _partyMemberDisplayNodes.Add(pm3);
        _partyMemberDisplayNodes.Add(pm4);

        _reserveItemList.ItemClicked += _OnReserveMemberClicked;

        DisplayPartyMembers();
        RefreshReserveList();
    }

    private void RefreshReserveList()
    {
        _reserveItemList.Clear();

        foreach (OverworldEntity member in _reserves)
        {
            string displayName = member.DisplayName;

            if (member.IsInParty)
                displayName += " [PARTY]";

            _reserveItemList.AddItem(displayName, CharacterImageAssets.GetTextureForItemList(member.Image));
        }

        int totalReserves = _reserveItemList.ItemCount;
        // show first reserves as default
        if (totalReserves > 0)
        {
            if (_selectedIndex == totalReserves)
                _selectedIndex = totalReserves - 1;

            _reserveItemList.Select(_selectedIndex);
            DisplayPreviewMember(_selectedIndex);
        }
        else
        {
            _reservePreviewMember.Call("Clear");
        }

        this.GetNode<Label>("%Tooltip").Text = "Create a party and ascend the Dungeon Tiers.";
    }

    private void _OnItemSelected(int index)
    {
        _selectedIndex = index;
        DisplayPreviewMember(_selectedIndex);
    }

    private void _OnEmbarkPressed()
    {
        var go = PersistentGameObjects.GameObjectInstance();
        
        if (go.Tier == go.TierCap)
            return;

        bool canEmbark = false;

        foreach(OverworldEntity member in _party.Party)
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

    private void DisplayPreviewMember(int index)
    {
        _reservePreviewMember.Call("DisplayPartyMember", index, true);
    }

    private void _OnBackButtonClicked()
    {
        this.QueueFree();
    }

    private void _OnReserveMemberClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        _selectedIndex = (int)index;

        if (mouse_button_index == (long)MouseButton.Left)
        {
            PlayerParty party = PersistentGameObjects.GameObjectInstance().MainPlayer.Party;

            // we have a free space open
            if (_reserves.Count > 0)
            {
                OverworldEntity reserveMember = _reserves[_selectedIndex];

                if (!reserveMember.IsInParty)
                    _party.AddPartyMember(reserveMember);
                else
                    _party.RemovePartyMember(reserveMember);

                DisplayPartyMembers();
                RefreshReserveList();
            }
        }
    }

    private void DisplayPartyMembers()
    {
        for(int i = 0; i < _party.Party.Length; i++)
        {
            var member = _party.Party[i];
            if(member != null)
                _partyMemberDisplayNodes[i].Call("DisplayPartyMember", i, false);
            else
                _partyMemberDisplayNodes[i].Call("Clear");
        }
    }
}
