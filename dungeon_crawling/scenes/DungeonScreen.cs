using AscendedZ;
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
            MapDungeon();
        }
    }

    private void MapDungeon()
    {
        foreach(var child in _childNodes.GetChildren())
            child.QueueFree();

        _floor.ClearNodes();

        _floor.Generate();

        _root.ClearPoints();

        MapTilesR(_floor.Root, _root, new HashSet<Vector2>() { _root.GlobalPosition });

        // in-line function to avoid Godot adding Tiles to its partial class
        // we do this so we can serialize the tiles
        void MapTilesR(Tile tile, TileScene scene, HashSet<Vector2> visited)
        {
            if (tile.Left != null)
            {
                scene.AddLeftLine();

                TileScene left = GetTileScene();

                this.AddChild(left);

                Vector2 leftPos = scene.GetLeftPosition();

                if (!visited.Contains(leftPos))
                {
                    visited.Add(leftPos);
                    left.Position = leftPos;
                    left.Reparent(_childNodes);

                    MapTilesR(tile.Left, left, visited);
                }
                else
                {
                    left.QueueFree();
                }
            }

            if (tile.Right != null)
            {
                scene.AddRightLine();

                TileScene right = GetTileScene();

                this.AddChild(right);

                Vector2 rightPos = scene.GetRightPosition();
                if (!visited.Contains(rightPos))
                {
                    visited.Add(rightPos);
                    right.Position = rightPos;
                    right.Reparent(_childNodes);

                    MapTilesR(tile.Right, right, visited);
                }
                else
                {
                    right.QueueFree();
                }
                
            }

            if (tile.BottomRight != null)
            {
                scene.AddBottomRightLine();

                TileScene bRight = GetTileScene();

                this.AddChild(bRight);

                Vector2 bRightPos = scene.GetBRightPosition();
                if (!visited.Contains(bRightPos))
                {
                    visited.Add(bRightPos);
                    bRight.Position = bRightPos;
                    bRight.Reparent(_childNodes);

                    MapTilesR(tile.BottomRight, bRight, visited);
                }
                else
                {
                    bRight.QueueFree();
                }
            }

            if (tile.BottomLeft != null)
            {
                scene.AddBottomLeftLine();

                TileScene bLeft = GetTileScene();

                this.AddChild(bLeft);

                Vector2 bLeftPos = scene.GetBLeftPosition();
                if (!visited.Contains(bLeftPos))
                {
                    visited.Add(bLeftPos);
                    bLeft.Position = bLeftPos;
                    bLeft.Reparent(_childNodes);

                    MapTilesR(tile.BottomLeft, bLeft, visited);
                }
                else
                {
                    bLeft.QueueFree();
                }
            }
        }
    }

    private TileScene GetTileScene()
    {
        return ResourceLoader.Load<PackedScene>(TILE_SCENE).Instantiate<TileScene>();
    }
}
