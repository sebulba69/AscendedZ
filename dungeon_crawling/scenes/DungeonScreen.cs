using AscendedZ;
using AscendedZ.currency;
using AscendedZ.dungeon_crawling.backend;
using AscendedZ.dungeon_crawling.backend.TileEvents;
using AscendedZ.dungeon_crawling.backend.Tiles;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

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
    private DungeonEntity _player;

    private List<TileScene> _scenes;
    private UITile _currentScene;
    private bool _onMainPath;
    private bool _processingEvent;

    private TileScene _currentSceneOLD;

    private Dungeon _dungeon;
    private HashSet<int> _addedScenes;
    private int _currentIndex;
    private Camera2D _camera;
    private AudioStreamPlayer _audioStreamPlayer;
    private GameObject _gameObject;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_tiles = this.GetNode<Marker2D>("%Tiles");
        _player = this.GetNode<DungeonEntity>("%Player");
        _camera = this.GetNode<Camera2D>("%Camera2D");
        _audioStreamPlayer = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        _gameObject = PersistentGameObjects.GameObjectInstance();
        int tier = _gameObject.TierDC;
        string track = MusicAssets.GetDungeonTrackDC(tier);
        _gameObject.MusicPlayer.SetStreamPlayer(_audioStreamPlayer);

        _player.SetGraphic(_gameObject.MainPlayer.Image);
        _gameObject.MusicPlayer.PlayMusic(track);

        _dungeon = new Dungeon(tier);
        _dungeon.Generate();

        _dungeon.TileEventTriggered += OnTileEventTriggeredAsync;

        StartDungeon();
    }

    public override void _Input(InputEvent @event)
    {
        if (_processingEvent)
            return;

        if (@event.IsActionPressed(Controls.RIGHT))
        {
            MoveDirection(_currentScene.Right, Direction.Right);
        }

        if (@event.IsActionPressed(Controls.LEFT))
        {
            MoveDirection(_currentScene.Left, Direction.Left);
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            MoveDirection(_currentScene.Down, Direction.Down);
        }

        if (@event.IsActionPressed(Controls.UP))
        {
            MoveDirection(_currentScene.Up, Direction.Up);
        }
    }

    private void MoveDirection(UITile tile, Direction direction)
    {
        if(tile != null)
        {
            _currentScene = tile;
            _player.Position = _currentScene.Scene.Position;
            _dungeon.MoveDirection(direction);
            _onMainPath = _dungeon.CurrentTile.IsMainTile;
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

        DrawNextTile(_dungeon.CurrentTile.GetDirection());
    }

    private void DrawNextTile(Direction direction)
    {
        if (_onMainPath)
        {
            HashSet<ITile> visited = new HashSet<ITile>();

            switch (direction)
            {
                case Direction.Right:
                    if(_dungeon.CurrentTile.Right != null && _currentScene.Right == null)
                    {
                        UITile next = MakeNewUITile();

                        _currentScene.Right = next;
                        next.Left = _currentScene;

                        AddDoors(_currentScene, next, direction);
                        // draw any paths that branch off from the main path
                        DrawBranchingTiles(next, _dungeon.CurrentTile.Right, visited);
                    }
                    break;

                case Direction.Left:
                    if (_dungeon.CurrentTile.Left != null && _currentScene.Left == null)
                    {
                        UITile next = MakeNewUITile();

                        _currentScene.Left = next;
                        next.Right = _currentScene;

                        AddDoors(_currentScene, next, direction);
                        DrawBranchingTiles(next, _dungeon.CurrentTile.Left, visited);
                    }
                    break;

                case Direction.Up:
                    if (_dungeon.CurrentTile.Up != null && _currentScene.Up == null)
                    {
                        UITile next = MakeNewUITile();

                        _currentScene.Up = next;
                        next.Down = _currentScene;

                        AddDoors(_currentScene, next, direction);

                        DrawBranchingTiles(next, _dungeon.CurrentTile.Up, visited);
                    }
                    break;


                case Direction.Down:
                    if (_dungeon.CurrentTile.Down != null && _currentScene.Down == null)
                    {
                        UITile next = MakeNewUITile();

                        _currentScene.Down = next;
                        next.Up = _currentScene;

                        AddDoors(_currentScene, next, direction);

                        DrawBranchingTiles(next, _dungeon.CurrentTile.Down, visited);
                    }
                    break;
            }
        }
    }

    private void AddDoors(UITile current, UITile destination, Direction direction)
    {
        current.Scene.AddLine(direction);
        destination.Scene.AddOppositeLine(direction);
        destination.Scene.Position = current.Scene.GetGlobalPosition(direction);
    }

    private void DrawBranchingTiles(UITile uiTile, ITile tile, HashSet<ITile> visited)
    {
        if (visited.Contains(tile))
            return;

        uiTile.Scene.SetGraphic(tile.Graphic);

        visited.Add(tile);
        if(tile.Up != null && uiTile.Up == null)
        {
            UITile up = MakeNewUITile();

            uiTile.Up = up;
            up.Down = uiTile;

            AddDoors(uiTile, up, Direction.Up);
            DrawBranchingTiles(up, tile.Up, visited);
        }

        if (tile.Down != null && uiTile.Down == null)
        {
            UITile down = MakeNewUITile();

            uiTile.Down = down;
            down.Up = uiTile;

            AddDoors(uiTile, down, Direction.Down);
            DrawBranchingTiles(down, tile.Down, visited);
        }

        if (tile.Left != null && uiTile.Left == null)
        {
            UITile left = MakeNewUITile();

            uiTile.Left = left;
            left.Right = uiTile;

            AddDoors(uiTile, left, Direction.Left);
            DrawBranchingTiles(left, tile.Left, visited);
        }

        if (tile.Right != null && uiTile.Right == null)
        {
            UITile right = MakeNewUITile();

            uiTile.Right = right;
            right.Left = uiTile;

            AddDoors(uiTile, right, Direction.Right);
            DrawBranchingTiles(right, tile.Right, visited);
        }
    }

    private UITile MakeNewUITile() 
    {
        TileScene tileScene = ResourceLoader.Load<PackedScene>(TILE_SCENE).Instantiate<TileScene>();
        UITile uiTile = new UITile() { Scene = tileScene };
        _tiles.AddChild(uiTile.Scene);
        return uiTile;
    }

    private async void OnTileEventTriggeredAsync(object sender, ITileEvent tileEvent)
    {
        // start of the event, prevent further inputs
        _processingEvent = true;

        TileEventId id = tileEvent.Id;
        
        switch (id)
        {
            case TileEventId.Item:
                ItemEvent itemEvent = (ItemEvent)tileEvent;
                ItemTile itemTile = itemEvent.Tile;
                // get the item from the tile
                // play a little jingle :)
                // show what you got
                _currentScene.Scene.TurnOffGraphic();
                break;

            case TileEventId.Encounter:
                
                EncounterEvent encounterEvent = (EncounterEvent)tileEvent;
                MainEncounterTile mainEncounterTile = encounterEvent.Tile;
                // put up battle scene <-- handle rewards there

                var combatScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_COMBAT).Instantiate<DungeonCombat>();
                this.AddChild(combatScene);

                var player = _gameObject.MainPlayer.DungeonPlayer.MakeBattlePlayerDC();
                player.Image = _gameObject.MainPlayer.Image;

                combatScene.Initialize(player, mainEncounterTile.Encounter);

                _camera.Enabled = false;
                await ToSignal(combatScene, "tree_exited");
                _camera.Enabled = true;

                _currentScene.Scene.TurnOffGraphic(); // <-- turnoff when finished
                break;

            case TileEventId.Heal:
                // heal some amount of HP/MP

                _currentScene.Scene.TurnOffGraphic(); // <-- turnoff when finished
                break;

            case TileEventId.Shop:
                // bring up the shop scene
                break;

            case TileEventId.Exit:
                // handle dungeon end stuff (do yes/no box)
                break;
        }


        // on completion of the event
        _processingEvent = false;
    }
}
