using AscendedZ;
using Godot;
using System;

public partial class Icon : CenterContainer
{
	private Sprite2D _sprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = this.GetNode<Sprite2D>("%IconSprite");
	}

	public void SetIcon(string icon)
	{
		_sprite.Texture = ArtAssets.GenerateIcon(icon);
		this.TooltipText = icon;
	}
}
