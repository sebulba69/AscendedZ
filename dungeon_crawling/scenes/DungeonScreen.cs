using AscendedZ;
using AscendedZ.currency;
using AscendedZ.dungeon_crawling.backend;
using AscendedZ.dungeon_crawling.backend.Tiles;
using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class UITile
{
    public TileScene TileScene { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public partial class DungeonScreen : Node2D
{
    private Marker2D _tiles;
    private List<UITile> _scenes;
    private readonly string TILE_SCENE = "res://dungeon_crawling/scenes/TileScene.tscn";
    private Dungeon _floor;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_tiles = this.GetNode<Marker2D>("%Tiles");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.ENTER))
        {

        }
    }
}
