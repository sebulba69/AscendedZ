using AscendedZ;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using static Godot.WebSocketPeer;

public partial class PartyEditScreen : HBoxContainer
{
    private PartyMemberDisplay _reservePreviewMember;
    private ItemList _reserveItemList;

    // party buttons
    private List<PartyMemberDisplay> _partyMemberDisplayNodes;

    private List<OverworldEntity> _reserves;
    private PlayerParty _party;
    private Button _embarkButton;
    private int _selectedIndex;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        Button backButton = this.GetNode<Button>("%BackButton");
        _embarkButton = this.GetNode<Button>("%EmbarkButton");

        _selectedIndex = 0;
        _party = gameObject.MainPlayer.Party;
        _reserves = gameObject.MainPlayer.ReserveMembers;

        foreach(var reserve in _reserves)
        {
            if (reserve.IsInParty)
            {
                _party.RefreshPartyMember(reserve);
            }
        }

        _partyMemberDisplayNodes = new List<PartyMemberDisplay>();

        _reservePreviewMember = this.GetNode<PartyMemberDisplay>("%Preview");
        _reserveItemList = this.GetNode<ItemList>("%InReserveMembers");

        for (int i = 1; i <= 4; i++)
        {
            _partyMemberDisplayNodes.Add(this.GetNode<PartyMemberDisplay>($"%PM{i}"));
        }

        _reserveItemList.ItemClicked += _OnReserveMemberClicked;
        _reserveItemList.ItemSelected += _OnItemSelected;

        backButton.Pressed += () => { this.QueueFree(); };
        _embarkButton.Pressed += _OnEmbarkPressed;

        DisplayPartyMembers();
        RefreshReserveList();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.UP))
        {
            _selectedIndex--;
            if (_selectedIndex <= 0)
                _selectedIndex = 0;

            _reserveItemList.Select(_selectedIndex);
            DisplayPreviewMember(_selectedIndex);
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            _selectedIndex++;
            if (_selectedIndex >= _reserveItemList.ItemCount)
                _selectedIndex = _reserveItemList.ItemCount - 1;

            _reserveItemList.Select(_selectedIndex);
            DisplayPreviewMember(_selectedIndex);
        }
    }

    public void DisableEmbarkButton()
    {
        _embarkButton.Visible = false;
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

        _reserveItemList.Select(_selectedIndex);
        DisplayPreviewMember(_selectedIndex);
    }

    private void DisplayPreviewMember(int index)
    {
        _reservePreviewMember.Call("DisplayPartyMember", index, true);
    }

    private void DisplayPartyMembers()
    {
        for (int i = 0; i < _party.Party.Length; i++)
        {
            var member = _party.Party[i];
            if (member != null)
                _partyMemberDisplayNodes[i].Call("DisplayPartyMember", i, false);
            else
                _partyMemberDisplayNodes[i].Call("Clear");
        }
    }

    private void _OnItemSelected(long index)
    {
        _selectedIndex = (int)index;
        DisplayPreviewMember(_selectedIndex);
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

    private void _OnEmbarkPressed()
    {
        var go = PersistentGameObjects.GameObjectInstance();

        if (go.Tier == go.TierCap)
            return;

        bool canEmbark = false;

        foreach (OverworldEntity member in go.MainPlayer.Party.Party)
        {
            // you need at least 1 party member to embark
            if (member != null)
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
}
