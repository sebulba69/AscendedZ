using Godot;
using System;

public partial class DungeonEntity : Node2D
{
	private Sprite2D _pic;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pic = this.GetNode<Sprite2D>("%EntityPic");
	}

	public void SetGraphic(string graphic)
	{
        _pic.Texture = ResourceLoader.Load<Texture2D>(graphic);
    }
}
