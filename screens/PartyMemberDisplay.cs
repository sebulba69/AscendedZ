using AscendedZ.battle;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
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

    public void ShowRandomEntity(EntityUIWrapper wrapper)
    {
        var partyMember = wrapper.Entity;
        _playerPicture.Texture = ResourceLoader.Load<Texture2D>(partyMember.Image);

        StringBuilder description = new StringBuilder();
        description.AppendLine(partyMember.DisplayName);
        description.Append(partyMember.ToString());

        _description.Text = description.ToString();
    }

    public void ShowFusionEntity(EntityUIWrapper wrapper) 
    {
        var partyMember = wrapper.Entity;
        _playerPicture.Texture = ResourceLoader.Load<Texture2D>(partyMember.Image);

        StringBuilder description = new StringBuilder();
        description.AppendLine(partyMember.DisplayName);
        description.Append(partyMember.GetFusionString());

        _description.Text = description.ToString();
    }

    public void Clear()
    {
        _playerPicture.Texture = null;
        _description.Text = string.Empty;
    }
}
