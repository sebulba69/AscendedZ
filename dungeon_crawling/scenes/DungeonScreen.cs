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
		_root = this.GetNode<TileScene>("%TileScene");
        _childNodes = this.GetNode<Marker2D>("%ChildNodes");
        _floor = new Floor();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.UP))
        {
            if (_floor.IsPathUp())
            {
                TileScene previousTile = _dungeonScenes[_floor.R, _floor.C];
                previousTile.SetTileOccupied(false);

                _floor.MoveUp();

                if (_dungeonScenes[_floor.R, _floor.C] == null)
                {
                    TileScene up = GetTileScene();
                    _dungeonScenes[_floor.R, _floor.C] = up; // by this point, floor R and C have been incremented
                    
                    this.AddChild(up);

                    up.Position = previousTile.GetUpPosition();

                    GeneratePaths();
                }

                _dungeonScenes[_floor.R, _floor.C].SetTileOccupied(true);
            }
        }

        if (@event.IsActionPressed(Controls.LEFT))
        {
            if (_floor.IsPathLeft())
            {
                TileScene previousTile = _dungeonScenes[_floor.R, _floor.C];
                previousTile.SetTileOccupied(false);

                _floor.MoveLeft();

                if (_dungeonScenes[_floor.R, _floor.C] == null)
                {
                    TileScene left = GetTileScene();
                    _dungeonScenes[_floor.R, _floor.C] = left; // by this point, floor R and C have been incremented
                    this.AddChild(left);
                    left.Position = previousTile.GetLeftPosition();
                    GeneratePaths();
                }

                _dungeonScenes[_floor.R, _floor.C].SetTileOccupied(true);
            }
        }

        if (@event.IsActionPressed(Controls.RIGHT))
        {
            if (_floor.IsPathRight())
            {
                TileScene previousTile = _dungeonScenes[_floor.R, _floor.C];
                previousTile.SetTileOccupied(false);
   
                _floor.MoveRight();

                if (_dungeonScenes[_floor.R, _floor.C] == null)
                {
                    TileScene right = GetTileScene();
                    _dungeonScenes[_floor.R, _floor.C] = right; // by this point, floor R and C have been incremented
                    this.AddChild(right);
                    right.Position = previousTile.GetRightPosition();
                    GeneratePaths();
                }

                _dungeonScenes[_floor.R, _floor.C].SetTileOccupied(true);
            }
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            if (_floor.IsPathDown())
            {
                TileScene previousTile = _dungeonScenes[_floor.R, _floor.C];
                previousTile.SetTileOccupied(false);
                _floor.MoveDown();
                _dungeonScenes[_floor.R, _floor.C].SetTileOccupied(true);
            }
        }

        if (@event.IsActionPressed(Controls.ENTER))
        {
            _floor.StartDungeon(1);

            Tile[,] dungeon = _floor.DungeonFloor;
            int rows = dungeon.GetLength(0);
            int columns = dungeon.GetLength(1);

            _dungeonScenes = new TileScene[rows, columns];

            _dungeonScenes[_floor.R, _floor.C] = _root;
            TileScene current = _dungeonScenes[_floor.R, _floor.C];
            current.SetTileOccupied(true);
            GeneratePaths();
        }
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

    private TileScene GetTileScene()
    {
        return ResourceLoader.Load<PackedScene>(TILE_SCENE).Instantiate<TileScene>();
    }
}
