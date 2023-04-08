using AscendedZ;
using AscendedZ.battle;
using AscendedZ.entities;
using Godot;
using System;
using System.Text;

public partial class PartyMemberDisplay : HBoxContainer
{
    private TextureRect _playerPicture;
    private TextEdit _description;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _playerPicture = this.GetNode<TextureRect>("PanelContainer/PlayerProfile");
        _description = this.GetNode<TextEdit>("DescriptionBox");
    }

    /// <summary>
    /// Sets up all the values and the descriptions of the party member associated with this UI.
    /// </summary>
    /// <param name="partyMember"></param>
    public void DisplayPartyMember(int index, bool isReserve)
    {
        OverworldEntity partyMember;
        
        if (isReserve)
            partyMember = PersistentGameObjects.Instance().MainPlayer.ReserveMembers[index];
        else
            partyMember = PersistentGameObjects.Instance().MainPlayer.Party.Party[index];

        if(partyMember != null)
        {
            _playerPicture.Texture = ResourceLoader.Load<Texture2D>(partyMember.Image);

            StringBuilder description = new StringBuilder();
            description.AppendLine(partyMember.Name);
            description.Append(partyMember.ToString());

            _description.Text = description.ToString();
        }
    }

    public void Clear()
    {
        _playerPicture.Texture = null;
        _description.Text = string.Empty;
    }
}
