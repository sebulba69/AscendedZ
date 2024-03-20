using AscendedZ;
using AscendedZ.currency;
using AscendedZ.dungeon_crawling.backend;
using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonScreen : Node2D
{
	private TileScene _root;
    private readonly string TILE_SCENE = "res://dungeon_crawling/scenes/TileScene.tscn";
    private Floor _floor;
    private Marker2D _childNodes;
    private TileScene[,] _dungeonScenes;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_root = this.GetNode<TileScene>("%Root");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.UP))
        {
            if (_floor.IsPathUp())
            {

            }
        }

        if (@event.IsActionPressed(Controls.LEFT))
        {
            if (_floor.IsPathLeft())
            {

            }
        }

        if (@event.IsActionPressed(Controls.RIGHT))
        {
            if (_floor.IsPathRight())
            {

            }
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            if (_floor.IsPathDown())
            {
            }
        }

        if (@event.IsActionPressed(Controls.ENTER))
        {
            GenerateDungeon();
        }
    }

    private void GenerateDungeon()
    {
        _floor = new Floor();
        _floor.StartDungeon(1);

        Tile[,] dungeon = _floor.DungeonFloor;
        int rows = dungeon.GetLength(0);
        int columns = dungeon.GetLength(1);

        _dungeonScenes = new TileScene[rows, columns];

        double tileDistance = _root.GetTileDistance();
        double startR = (_floor.R * tileDistance) * -1; // top
        double startC = (_floor.C * tileDistance) * -1; // far left
        double currentC = startC;

        for(int r = 0; r < rows; r++)
        {
            for(int c = 0; c < columns; c++)
            {
                Vector2 tilePosition = new Vector2((float)currentC, (float)startR);

                if (!tilePosition.Equals(_root.Position))
                {
                    TileScene tile = MakeTile();
                    this.AddChild(tile);
                    tile.Position = tilePosition;
                    _dungeonScenes[r, c] = tile;
                }

                currentC += tileDistance;
            }

            startR += tileDistance;
            currentC = startC;
        }
    }

    private TileScene MakeTile()
    {
        return ResourceLoader.Load<PackedScene>(TILE_SCENE).Instantiate<TileScene>();
    }

    private void GeneratePaths()
    {
        TileScene current = _dungeonScenes[_floor.R, _floor.C];

        if (_floor.IsPathUp())
            current.AddUpLine();

        if (_floor.IsPathLeft())
            current.AddLeftLine();

        if (_floor.IsPathRight())
            current.AddRightLine();
    }
}
