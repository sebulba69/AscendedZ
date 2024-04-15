using AscendedZ;
using AscendedZ.currency;
using AscendedZ.dungeon_crawling.backend;
using AscendedZ.dungeon_crawling.backend.Tiles;
using Godot;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Transactions;

public class UITile
{
    public TileScene Scene { get; set; }
    public UITile Up { get; set; }
    public UITile Down { get; set; }
    public UITile Left { get; set; }
    public UITile Right { get; set; }
}

public partial class DungeonScreen : Node2D
{
    private readonly string TILE_SCENE = "res://dungeon_crawling/scenes/TileScene.tscn";
    private Marker2D _tiles;
    private Sprite2D _player;

    private List<TileScene> _scenes;
    private UITile _currentScene;
    private bool _onMainPath;

    private TileScene _currentSceneOLD;

    private Dungeon _dungeon;
    private HashSet<int> _addedScenes;
    private int _currentIndex;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_tiles = this.GetNode<Marker2D>("%Tiles");
        _player = this.GetNode<Sprite2D>("%Player");

        _dungeon = new Dungeon(1);

        _dungeon.Start();

        StartDungeon();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.RIGHT))
        {
            _dungeon.MoveRight();
            
            if(_currentScene.Right != null)
            {
                _currentScene = _currentScene.Right;
                _player.Position = _currentScene.Scene.Position;
            }

            DrawNextTile();
        }

        if (@event.IsActionPressed(Controls.LEFT))
        {
            _dungeon.MoveLeft();
            if (_currentScene.Left != null)
            {
                _currentScene = _currentScene.Left;
                _player.Position = _currentScene.Scene.Position;
            }
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            _dungeon.MoveDown();
            if (_currentScene.Down != null)
            {
                _currentScene = _currentScene.Down;
                _player.Position = _currentScene.Scene.Position;
                _onMainPath = _dungeon.CurrentTile.IsMainTile;
            }

        }

        if (@event.IsActionPressed(Controls.UP))
        {
            _dungeon.MoveUp();
            if (_currentScene.Up != null)
            {
                _currentScene = _currentScene.Up;
                _player.Position = _currentScene.Scene.Position;
                _onMainPath = _dungeon.CurrentTile.IsMainTile;
            }


        }
    }

    private void StartDungeon()
    {
        for (int c = 0; c < _tiles.GetChildCount(); c++)
        {
            var child = _tiles.GetChild(c);
            child.QueueFree();
            _tiles.RemoveChild(child);
        }

        _currentScene = MakeNewUITile();
        _onMainPath = true;
        DrawNextTile();
    }

    private void DrawNextTile()
    {
        if(_dungeon.CurrentTile.Right != null 
            && _currentScene.Right == null
            && _onMainPath)
        {
            // draw the initial scene
            UITile next = MakeNewUITile();

            _currentScene.Right = next;
            next.Left = _currentScene;

            _currentScene.Scene.AddRightLine();
            next.Scene.Position = _currentScene.Scene.GetRightPosition();

            // draw any paths that branch off from the main path
            HashSet<ITile> visited = new HashSet<ITile>();
            DrawBranchingTiles(next, _dungeon.CurrentTile.Right, visited);
        }
    }

    private void DrawBranchingTiles(UITile uiTile, ITile tile, HashSet<ITile> visited)
    {
        if (visited.Contains(tile))
            return;

        visited.Add(tile);
        if(tile.Up != null && uiTile.Up == null)
        {
            UITile up = MakeNewUITile();

            uiTile.Up = up;
            up.Down = uiTile;

            uiTile.Scene.AddUpLine();
            up.Scene.Position = uiTile.Scene.GetUpPosition();
            DrawBranchingTiles(up, tile.Up, visited);
        }

        if (tile.Down != null && uiTile.Down == null)
        {
            UITile down = MakeNewUITile();

            uiTile.Down = down;
            down.Up = uiTile;

            uiTile.Scene.AddDownLine();
            down.Scene.Position = uiTile.Scene.GetDownPosition();
            DrawBranchingTiles(down, tile.Down, visited);
        }

        if (tile.Left != null && uiTile.Left == null)
        {
            UITile left = MakeNewUITile();

            uiTile.Left = left;
            left.Right = uiTile;

            uiTile.Scene.AddLeftLine();
            left.Scene.Position = uiTile.Scene.GetLeftPosition();
            DrawBranchingTiles(left, tile.Left, visited);
        }

        if (tile.Right != null && !tile.Right.IsMainTile && uiTile.Right == null)
        {
            UITile right = MakeNewUITile();

            uiTile.Right = right;
            right.Left = uiTile;

            uiTile.Scene.AddRightLine();
            right.Scene.Position = uiTile.Scene.GetRightPosition();
            DrawBranchingTiles(right, tile.Right, visited);
        }
    }

    private UITile MakeNewUITile() 
    {
        UITile scene = new UITile() { Scene = MakeTileScene() };
        _tiles.AddChild(scene.Scene);
        return scene;
    }

    private void AddTileSceneToMainPathOLD()
    {
        TileScene scene = MakeTileScene();
        _scenes.Add(scene);
        _tiles.AddChild(scene);
    }

    private void Clear()
    {

    }

    private TileScene MakeTileScene()
    {
        return ResourceLoader.Load<PackedScene>(TILE_SCENE).Instantiate<TileScene>();
    }
}
