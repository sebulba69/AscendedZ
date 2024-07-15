using AscendedZ.dungeon_crawling.backend;
using AscendedZ.dungeon_crawling.backend.Tiles;
using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class TileScene : Node2D
{
    private Dictionary<Direction, Sprite2D> _doors;
    private Dictionary<Direction, Marker2D> _roomCoord;
    private DungeonEntity _graphic;

	public override void _Ready()
	{
        _graphic = this.GetNode<DungeonEntity>("%Graphic");

        _doors = new Dictionary<Direction, Sprite2D>()
        {
            { Direction.Up, this.GetNode<Sprite2D>("%Up") },
            { Direction.Left, this.GetNode<Sprite2D>("%Left") },
            { Direction.Right, this.GetNode<Sprite2D>("%Right") },
            { Direction.Down, this.GetNode<Sprite2D>("%Down") }
        };

        _roomCoord = new Dictionary<Direction, Marker2D>() 
        {
            { Direction.Up, this.GetNode<Marker2D>("%RoomUp") },
            { Direction.Left, this.GetNode<Marker2D>("%RoomLeft") },
            { Direction.Right, this.GetNode<Marker2D>("%RoomRight") },
            { Direction.Down, this.GetNode<Marker2D>("%RoomDown") }
        };
    }

    public void SetGraphic(string graphic)
    {
        if (string.IsNullOrEmpty(graphic))
            return;

        _graphic.Visible = true;
        _graphic.SetGraphic(graphic);
    }

    public void TurnOffGraphic()
    {
        _graphic.Visible = false;
    }

    public void AddDoor(Direction direction)
    {
        if (!_doors[direction].Visible)
        {
            _doors[direction].Visible = true;
        }
    }

    public Vector2 GetGlobalPosition(Direction direction)
    {
        return _roomCoord[direction].GlobalPosition;
    }
}
