using AscendedZ;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

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
    private List<Button> _buttons;
    private bool[] _buttonStates;

    List<OverworldEntity> _reserves;
    private PlayerParty _party;
    private int _selected;

    public override void _Ready()
    {
        GameObject gameObject = PersistentGameObjects.Instance();

        _tierLabel = this.GetNode<Label>("%TierLabel");
        _reservePreviewMember = this.GetNode<PartyMemberDisplay>("%Preview");
        _reserveItemList = this.GetNode<ItemList>("%InReserveMembers");
        _party = gameObject.MainPlayer.Party;
        _reserves = gameObject.MainPlayer.ReserveMembers;
        _partyMemberDisplayNodes = new List<PartyMemberDisplay>();
        _selected = 0;
        _buttons = new List<Button>();
        _buttonStates = new bool[] { false, false, false, false };

        Button leftTier = this.GetNode<Button>("%LeftTierBtn");
        Button rightBtn = this.GetNode<Button>("%RightTierBtn");
        Button embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button backButton = this.GetNode<Button>("%BackButton");
        Button pm1button = this.GetNode<Button>("VBoxContainer/HBoxContainer/InPartyMembers/CenterContainer/InPartyMemberContainer/HBoxContainer/PM1Button");
        Button pm2button = this.GetNode<Button>("VBoxContainer/HBoxContainer/InPartyMembers/CenterContainer/InPartyMemberContainer/HBoxContainer2/PM2Button");
        Button pm3button = this.GetNode<Button>("VBoxContainer/HBoxContainer/InPartyMembers/CenterContainer/InPartyMemberContainer/HBoxContainer3/PM3Button");
        Button pm4button = this.GetNode<Button>("VBoxContainer/HBoxContainer/InPartyMembers/CenterContainer/InPartyMemberContainer/HBoxContainer4/PM4Button");

        PartyMemberDisplay pm1 = this.GetNode<PartyMemberDisplay>("VBoxContainer/HBoxContainer/InPartyMembers/CenterContainer/InPartyMemberContainer/HBoxContainer/PM1");
        PartyMemberDisplay pm2 = this.GetNode<PartyMemberDisplay>("VBoxContainer/HBoxContainer/InPartyMembers/CenterContainer/InPartyMemberContainer/HBoxContainer2/PM2");
        PartyMemberDisplay pm3 = this.GetNode<PartyMemberDisplay>("VBoxContainer/HBoxContainer/InPartyMembers/CenterContainer/InPartyMemberContainer/HBoxContainer3/PM3");
        PartyMemberDisplay pm4 = this.GetNode<PartyMemberDisplay>("VBoxContainer/HBoxContainer/InPartyMembers/CenterContainer/InPartyMemberContainer/HBoxContainer4/PM4");

        gameObject.Tier = gameObject.MaxTier;
        string tierText = "Dungeon Floor:";
        
        _tierLabel.Text = $"{tierText} {PersistentGameObjects.Instance().MaxTier}";

        // on click events
        leftTier.Pressed += () => 
        {
            PersistentGameObjects.Instance().Tier--;
            _tierLabel.Text = $"{tierText} {PersistentGameObjects.Instance().Tier}";
        };

        rightBtn.Pressed += () => 
        {
            PersistentGameObjects.Instance().Tier++;
            _tierLabel.Text = $"{tierText} {PersistentGameObjects.Instance().Tier}"; 
        };

        _reserveItemList.Connect("item_selected",new Callable(this,"_OnItemSelected"));

        embarkButton.Pressed += _OnEmbarkPressed;
        backButton.Pressed += _OnBackButtonClicked;

        pm1button.Pressed += _OnButton1Pressed;
        pm2button.Pressed += _OnButton2Pressed;
        pm3button.Pressed += _OnButton3Pressed;
        pm4button.Pressed += _OnButton4Pressed;

        _partyMemberDisplayNodes.Add(pm1);
        _partyMemberDisplayNodes.Add(pm2);
        _partyMemberDisplayNodes.Add(pm3);
        _partyMemberDisplayNodes.Add(pm4);

        _buttons.Add(pm1button);
        _buttons.Add(pm2button);
        _buttons.Add(pm3button);
        _buttons.Add(pm4button);

        // display all party members we may have added last time we were in this UI
        for (int i = 0; i < _party.Party.Length; i++)
        {
            // display pre-existing party members
            if(_party.Party[i] != null)
                DisplayPartyMemberAtIndex(i);
        }
        RefreshReserveList();
    }

    private void RefreshReserveList()
    {
        _reserveItemList.Clear();

        foreach (OverworldEntity member in _reserves)
        {
            _reserveItemList.AddItem(member.Name);
        }

        int totalReserves = _reserveItemList.ItemCount;
        // show first reserves as default
        if (totalReserves > 0)
        {
            if (_selected == totalReserves)
                _selected = totalReserves - 1;

            _reserveItemList.Select(_selected);
            DisplayPreviewMember(_selected);
        }
        else
        {
            _reservePreviewMember.Call("Clear");
        }

        this.GetNode<Label>("%Tooltip").Text = "Create a party and ascend the Dungeon Tiers.";
    }

    private void _OnItemSelected(int index)
    {
        _selected = index;
        DisplayPreviewMember(_selected);
    }

    private void _OnEmbarkPressed()
    {
        bool canEmbark = false;

        foreach(OverworldEntity member in _party.Party)
        {
            if(member != null)
                canEmbark = true;
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

    private void _OnButton1Pressed()
    {
        ChangeButtonTextState(0);
    }

    private void _OnButton2Pressed()
    {
        ChangeButtonTextState(1);
    }

    private void _OnButton3Pressed()
    {
        ChangeButtonTextState(2);
    }

    private void _OnButton4Pressed()
    {
        ChangeButtonTextState(3);
    }

    private void ChangeButtonTextState(int index)
    {
        bool buttonState = !_buttonStates[index];

        PlayerParty party = PersistentGameObjects.Instance().MainPlayer.Party;
        OverworldEntity member = party.Party[index];

        // left if true, right if false
        if (buttonState)
        {
            // we have a free space open
            if (member == null && _reserves.Count > 0)
            {
                OverworldEntity reserveMember = _reserves[_selected];

                // take the selected reserve member
                if (reserveMember != null)
                {
                    _reserves.RemoveAt(_selected);
                    _party.Party[index] = reserveMember;
                    DisplayPartyMemberAtIndex(index);
                    RefreshReserveList();
                }
            }
            else
            {
                _buttonStates[index] = false;
            }
        }
        else
        {
            if (member != null)
            {
                _buttons[index].Text = ">";
                _buttonStates[index] = false;

                // mark the member as not being in the party
                _reserves.Add(member);

                _partyMemberDisplayNodes[index].Call("Clear");

                party.Party[index] = null;

                RefreshReserveList();
            }
        }

    }

    private void DisplayPartyMemberAtIndex(int index)
    {
        // we only want to change the text if we're provided a party member
        // so we do it here and not below
        _buttons[index].Text = "<";
        _buttonStates[index] = true;
        _partyMemberDisplayNodes[index].Call("DisplayPartyMember", index, false);
    }
}
