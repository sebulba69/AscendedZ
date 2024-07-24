using AscendedZ;
using AscendedZ.currency;
using AscendedZ.dungeon_crawling.backend;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using Godot;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;
using static Godot.TextServer;
using static System.Net.Mime.MediaTypeNames;

public class UITile
{
    public int X { get; set; }
    public int Y { get; set; }
    public TileScene Scene { get; set; }
}

public partial class DungeonScreen : Transitionable2DScene
{
    private bool _prematurelyLeave;
    private Marker2D _tiles;
    private CanvasLayer _popup;
    private TextureRect _background;
    private DungeonCrawlUI _crawlUI;
    private FloorExitScene _floorExitScene;
    private Camera2D _camera;
    private AudioStreamPlayer _audioStreamPlayer, _encounterSfxPlayer, _healSfxPlayer, _itemSfxPlayer;
    private Button _retreat;
    private DungeonEntity _player;
    private UITile _currentScene;

    private bool _processingEvent;
    private bool _endingScene;
    private Dungeon _dungeon;
    private GameObject _gameObject;
    private List<BattlePlayer> _battlePlayers;

    private UITile[,] _uiTiles;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_tiles = this.GetNode<Marker2D>("%Tiles");
        _player = this.GetNode<DungeonEntity>("%Player");
        _camera = this.GetNode<Camera2D>("%Camera2D");
        _background = this.GetNode<TextureRect>("%Background");
        _audioStreamPlayer = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        _encounterSfxPlayer = this.GetNode<AudioStreamPlayer>("%EncounterSfxPlayer");
        _healSfxPlayer = this.GetNode<AudioStreamPlayer>("%HealSfxPlayer");
        _itemSfxPlayer = this.GetNode<AudioStreamPlayer>("%ItemSfxPlayer");
        _crawlUI = this.GetNode<DungeonCrawlUI>("%DungeonCrawlUi");
        _popup = this.GetNode<CanvasLayer>("%Popups");
        _floorExitScene = this.GetNode<FloorExitScene>("%FloorExitScene");

        _floorExitScene.Stay.Pressed += _OnStayButtonPressed;
        _floorExitScene.Continue.Pressed += _OnContinueToNextFloor;
        _floorExitScene.Back.Pressed += _OnRetreatButtonPressed;

        _retreat = this.GetNode<Button>("%RetreatBtn");
        _retreat.Pressed += () =>
        {
            _prematurelyLeave = true;
            _OnRetreatButtonPressed();
        };

        _player.Up.Pressed += () => 
        {
            MineDirection(_currentScene.X - 1, _currentScene.Y);
            SetPlayerDirections(_currentScene.X, _currentScene.Y);
        };

        _player.Down.Pressed += () => 
        {
            MineDirection(_currentScene.X + 1, _currentScene.Y);
            SetPlayerDirections(_currentScene.X, _currentScene.Y);
        };

        _player.Left.Pressed += () => 
        {
            MineDirection(_currentScene.X, _currentScene.Y - 1);
            SetPlayerDirections(_currentScene.X, _currentScene.Y);
        };

        _player.Right.Pressed += () => 
        {
            MineDirection(_currentScene.X, _currentScene.Y + 1);
            SetPlayerDirections(_currentScene.X, _currentScene.Y);
        };

        _gameObject = PersistentGameObjects.GameObjectInstance();
        _gameObject.MusicPlayer.SetStreamPlayer(_audioStreamPlayer);
        _battlePlayers = _gameObject.MakeBattlePlayerListFromParty();
        _player.SetGraphic(_gameObject.MainPlayer.Image);
        
        StartDungeon();
    }

    private void MineDirection(int x, int y)
    {
        GetNode<AudioStreamPlayer>("%MinePlayer").Play();
        _dungeon.Mine(x, y);
        var tiles = _dungeon.Tiles;
        DrawDoors(_uiTiles[x, y], tiles[x, y], tiles);
        _uiTiles[x, y].Scene.Visible = true;
        FillInDoors(x, y);
        _gameObject.Pickaxes--;

        SetCrawlValues();
        PersistentGameObjects.Save();
    }

    private void FillInDoors(int x, int y)
    {
        var tiles = _dungeon.Tiles;
        if (y - 1 >= 0 && _uiTiles[x, y - 1].Scene.Visible)
        {
            DrawDoors(_uiTiles[x, y - 1], tiles[x, y - 1], tiles);
        }

        if (x - 1 >= 0 && _uiTiles[x - 1, y].Scene.Visible)
        {
            DrawDoors(_uiTiles[x - 1, y], tiles[x - 1, y], tiles);
        }

        if (x + 1 < tiles.GetLength(0) && _uiTiles[x + 1, y].Scene.Visible)
        {
            DrawDoors(_uiTiles[x + 1, y], tiles[x + 1, y], tiles);
        }

        if (y + 1 < tiles.GetLength(0) && _uiTiles[x, y + 1].Scene.Visible)
        {
            DrawDoors(_uiTiles[x, y + 1], tiles[x, y + 1], tiles);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (_endingScene) return;
        if (_processingEvent) return;
        if (_currentScene == null) return;

        int x = _currentScene.X;
        int y = _currentScene.Y;
        
        if (@event.IsActionPressed(Controls.RIGHT))
        {
            MoveDirection(x, y + 1);
        }

        if (@event.IsActionPressed(Controls.LEFT))
        {
            MoveDirection(x, y - 1);
        }

        if (@event.IsActionPressed(Controls.DOWN))
        {
            MoveDirection(x + 1, y);
        }

        if (@event.IsActionPressed(Controls.UP))
        {
            MoveDirection(x - 1, y);
        }
    }

    private void MoveDirection(int x, int y)
    {
        if (x >= 0 && x < _uiTiles.GetLength(0) && y >= 0 && y < _uiTiles.GetLength(1))
        {
            var tile = _uiTiles[x, y];
            if (tile.Scene.Visible)
            {
                _player.SetArrows(false, false, false, false);
                _currentScene = tile;
                _processingEvent = true;
                var tween = CreateTween();
                tween.TweenProperty(_player, "position", _currentScene.Scene.Position, 0.25);
                tween.Finished += () =>
                {
                    _processingEvent = false;
                    _dungeon.MoveDirection(x, y);
                    _crawlUI.SetCoordinates(x, y);
                };

                SetPlayerDirections(x, y);
            }
        }
    }

    private void SetPlayerDirections(int x, int y)
    {
        if (_gameObject.Pickaxes > 0)
        {
            bool up = (x - 1 >= 0 && !_uiTiles[x - 1, y].Scene.Visible);
            bool right = (y + 1 < _uiTiles.GetLength(0) && !_uiTiles[x, y + 1].Scene.Visible);
            bool down = (x + 1 < _uiTiles.GetLength(0) && !_uiTiles[x + 1, y].Scene.Visible);
            bool left = (y - 1 >= 0 && !_uiTiles[x, y - 1].Scene.Visible);

            _player.SetArrows(up, down, left, right);
        }
    }

    private void SetCrawlValues()
    {
        var tier = PersistentGameObjects.GameObjectInstance().TierDC;
        _crawlUI.SetParty(tier, _battlePlayers, _gameObject.Orbs, _gameObject.Pickaxes, _dungeon.EncounterCount);
    }

    private void StartDungeon()
    {
        _background.Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetCombatDCBackground(_gameObject.TierDC));
        _player.SetArrows(false, false, false, false);
        int tier = _gameObject.TierDC;

        if (tier % 50 != 0)
        {
            _gameObject.MusicPlayer.PlayMusic(MusicAssets.GetDungeonTrackDC(_gameObject.TierDC));
        }
        else
        {
            _gameObject.MusicPlayer.PlayMusic(MusicAssets.DC_BOSS_PRE);
        }

        _currentScene = null;
        _dungeon = new Dungeon(_gameObject.TierDC);
        _dungeon.Generate();
        _dungeon.TileEventTriggered += OnTileEventTriggeredAsync;

        foreach(var child in _tiles.GetChildren())
            _tiles.RemoveChild(child);

        var tiles = _dungeon.Tiles;
        int rows = tiles.GetLength(0);
        int columns = rows;

        _uiTiles = new UITile[rows, columns];
        var position = new Vector2(0, 0);
        for (int r = 0; r < rows; r++)
        {
            for(int c = 0; c < columns; c++)
            {
                _uiTiles[r, c] = MakeNewUITile(r, c);
                _uiTiles[r, c].Scene.Position = position;
                if (tiles[r, c].IsPartOfMaze)
                {
                    DrawDoors(_uiTiles[r, c], tiles[r, c], tiles);
                    _uiTiles[r, c].Scene.SetGraphic(tiles[r,c].Graphic);
                }
                else
                {
                    _uiTiles[r, c].Scene.Visible = false;
                }

                if(c < columns - 1)
                    position = _uiTiles[r, c].Scene.GetGlobalPosition(Direction.Right);
            }

            if (r < rows - 1)
                position = _uiTiles[r, 0].Scene.GetGlobalPosition(Direction.Down);
        }

        var start =_dungeon.Current;
        _currentScene = _uiTiles[start.X, start.Y];
        _player.Position = _currentScene.Scene.Position;
        _crawlUI.SetCoordinates(_currentScene.X, _currentScene.Y);

        if(tier % 50 != 0)
        {
            SetPlayerDirections(start.X, start.Y);
        }

        SetCrawlValues();
    }

    private void DrawDoors(UITile uiTile, Tile tile, Tile[,] tiles)
    {
        int x = tile.X;
        int y = tile.Y;
        int length = tiles.GetLength(0);

        // look left
        if (y - 1 >= 0)
            if (tiles[x, y - 1].IsPartOfMaze)
                uiTile.Scene.AddDoor(Direction.Left);

        // look right
        if (y + 1 < length)
            if (tiles[x, y+1].IsPartOfMaze)
                uiTile.Scene.AddDoor(Direction.Right);

        // look up
        if (x - 1 >= 0)
            if (tiles[x - 1, y].IsPartOfMaze)
                uiTile.Scene.AddDoor(Direction.Up);
            
        // look down
        if (x + 1 < length)
            if (tiles[x + 1, y].IsPartOfMaze)
                uiTile.Scene.AddDoor(Direction.Down);
    }

    private UITile MakeNewUITile(int x, int y) 
    {
        TileScene tileScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_TILE_SCENE).Instantiate<TileScene>();
        UITile uiTile = new UITile() { Scene = tileScene, X = x, Y = y };
        _tiles.AddChild(uiTile.Scene);
        var template = BackgroundAssets.GetCombatDCTileTemplate(_gameObject.TierDC);
        uiTile.Scene.ChangeBackgroundColor(template.BackgroundString, template.DoorColor, template.LineColor );
        return uiTile;
    }

    private async void OnTileEventTriggeredAsync(object sender, TileEventId id)
    {
        // start of the event, prevent further inputs
        _processingEvent = true;
        
        switch (id)
        {
            case TileEventId.Miner:
                var miner = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_MINER).Instantiate<MinerUI>();
                _popup.AddChild(miner);
                miner.SetUIValues();
                await ToSignal(miner, "tree_exited");
                SetCrawlValues();
                _dungeon.Current.EventTriggered = false;
                break;

            case TileEventId.Item:
            case TileEventId.SpecialItem:
            case TileEventId.PotOfGreed:
                await ShowRewardScreen(id);
                ResetUIAfterItem();
                break;

            case TileEventId.BossDialog:
                string dialogScene = "res://dungeon_crawling/scenes/crawl_ui/DC_BOSS_CUTSCENE.tscn";

                var dScene = ResourceLoader.Load<PackedScene>(dialogScene).Instantiate<DC_BOSS_CUTSCENE>();
                _popup.AddChild(dScene);
                dScene.Start(_dungeon.Current.Entity, _dungeon.Current.EntityImage);

                await ToSignal(dScene, "tree_exited");

                var pFlags = _gameObject.ProgressFlagObject;
                pFlags.DCCutsceneSeen.Add(true);
                _currentScene.Scene.TurnOffGraphic();
                PersistentGameObjects.Save();
                break;
            case TileEventId.SpecialBossEncounter:
            case TileEventId.SpecialEncounter:
            case TileEventId.Encounter:
                bool doNotDoEncounter = false;

                if (id == TileEventId.SpecialBossEncounter)
                {
                    _dungeon.Current.EventTriggered = false;

                    var bossPrompt = ResourceLoader.Load<PackedScene>(Scenes.YES_NO_POPUP).Instantiate<AscendedYesNoWindow>();
                    _popup.AddChild(bossPrompt);
                    bossPrompt.SetDialogMessage("You sense a dangerous presence beyond the door. Do you proceed?");
                    bossPrompt.AnswerSelected += (sender, args) =>
                    {
                        doNotDoEncounter = !args;
                    };

                    await ToSignal(bossPrompt, "tree_exited");
                }

                if (doNotDoEncounter)
                    break;

                var combatScene = ResourceLoader.Load<PackedScene>(Scenes.BATTLE_SCENE).Instantiate<BattleEnemyScene>();
                var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();

                this.AddChild(transition);

                _encounterSfxPlayer.Play();

                transition.PlayFadeIn();
                await ToSignal(transition.Player, "animation_finished");

                SetEncounterVisibility(false);

                this.AddChild(combatScene);
                bool retreat = false;
                combatScene.BackToHome += (sender, args) => 
                {
                    retreat = true;
                };

                combatScene.SetupForDungeonCrawlEncounter(_battlePlayers, (id == TileEventId.SpecialEncounter) || (id == TileEventId.SpecialBossEncounter), (id == TileEventId.SpecialBossEncounter));

                transition.PlayFadeOut();
                await ToSignal(transition.Player, "animation_finished");
                await ToSignal(combatScene, "tree_exited");

                if (retreat)
                {
                    _prematurelyLeave = true;
                    _OnRetreatButtonPressed();
                }
                else
                {
                    int tier = _gameObject.TierDC;
                    if(tier % 50 == 0)
                    {
                        _gameObject.MusicPlayer.PlayMusic(MusicAssets.DC_BOSS_PRE);
                    }
                    else
                    {
                        _gameObject.MusicPlayer.PlayMusic(MusicAssets.GetDungeonTrackDC(_gameObject.TierDC));
                    }
                    _dungeon.ProcessEncounter();
                    SetEncounterVisibility(true);
                    SetCrawlValues();
                    _currentScene.Scene.TurnOffGraphic(); // <-- turnoff when finished
                    PersistentGameObjects.Save();
                }
                _dungeon.Current.EventTriggered = true;
                break;

            case TileEventId.Heal:
                // heal some amount of HP/MP
                foreach(var player in _battlePlayers)
                {
                    double percent = player.MaxHP * 0.45;
                    int hp = (int)(player.HP + percent);
                    player.HP = player.MaxHP;
                }

                _healSfxPlayer.Play();
                SetCrawlValues();
                _currentScene.Scene.TurnOffGraphic(); // <-- turnoff when finished
                PersistentGameObjects.Save();
                break;

            case TileEventId.Orb:
                _gameObject.Orbs++;
                _itemSfxPlayer.Play();
                _currentScene.Scene.TurnOffGraphic();
                SetCrawlValues();
                break;

            case TileEventId.Portal:
                _dungeon.Current.EventTriggered = false;

                var popupWindow = ResourceLoader.Load<PackedScene>(Scenes.YES_NO_POPUP).Instantiate<AscendedYesNoWindow>();
                _popup.AddChild(popupWindow);
                popupWindow.SetDialogMessage("Teleport?");
                popupWindow.AnswerSelected += (sender, args) => 
                {
                    if (args)
                    {
                        // spawn yes/no box
                        var tile = _dungeon.Current;
                        int x = tile.TPLocation[0];
                        int y = tile.TPLocation[1];

                        _currentScene = _uiTiles[x, y];

                        var tween = CreateTween();
                        tween.TweenProperty(_player, "position", _currentScene.Scene.Position, 0.25);
                        tween.Finished += () =>
                        {
                            _processingEvent = false;
                            _dungeon.MoveDirection(x, y, true);
                            _crawlUI.SetCoordinates(x, y);
                        };

                        _dungeon.Current.EventTriggered = false;
                    }
                };
                await ToSignal(popupWindow, "tree_exited");
                SetCrawlValues();
                break;

            case TileEventId.Fountain:
                var fountain = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_FOUNTAIN).Instantiate<FountainOfBuce>();
                _popup.AddChild(fountain);
                await ToSignal(fountain, "tree_exited");
                SetCrawlValues();
                _dungeon.Current.EventTriggered = false;
                break;

            case TileEventId.Exit:
                // handle dungeon end stuff (do yes/no box)
                if(_dungeon.CanLeave)
                {
                    _endingScene = true;
                    _floorExitScene.Visible = true;
                    SetEncounterVisibility(false, true);
                    _floorExitScene.Continue.Visible = (_gameObject.MaxTierDC + 1 < _gameObject.TierDCCap);
                    _floorExitScene.EndOfBattleLabel.Text = "Ascend?";
                    _floorExitScene.Stay.Visible = true;
                    _floorExitScene.Retry.Visible = false;
                }
                _dungeon.Current.EventTriggered = false;
                break;
        }


        // on completion of the event
        _processingEvent = false;
    }

    private async Task ShowRewardScreen(TileEventId id)
    {
        var reward = ResourceLoader.Load<PackedScene>(Scenes.REWARDS).Instantiate<RewardScreen>();
        _itemSfxPlayer.Play();
        _crawlUI.Visible = false;
        _popup.AddChild(reward);

        if (id == TileEventId.PotOfGreed)
            reward.InitializePotOfGreedRewards();
        else if (id == TileEventId.SpecialItem)
            reward.InitializeDungeonCrawlSpecialItems();
        else if (id == TileEventId.Item)
            reward.RandomizeDungeonCrawlRewards();
        else
            reward.InitializeDungeonCrawlTierRewards();

        await ToSignal(reward, "tree_exited");
    }

    private void ResetUIAfterItem()
    {
        SetCrawlValues();
        _crawlUI.Visible = true;
        PersistentGameObjects.Save();
        _currentScene.Scene.TurnOffGraphic();
    }

    private async void _OnContinueToNextFloor()
    {
        _floorExitScene.Visible = false;
        await ShowRewardScreen(TileEventId.Exit);

        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();

        AddChild(transition);
        transition.PlayFadeIn();

        await ToSignal(transition.Player, "animation_finished");

        int tier = _gameObject.TierDC;
        int maxTier = _gameObject.MaxTierDC;

        if (tier == maxTier && tier + 1 < _gameObject.TierDCCap) 
        {
            _gameObject.MaxTierDC++;
            _gameObject.TierDC++;
        }
        else
        {
            _gameObject.TierDC++;
        }

        SetEncounterVisibility(true, true);

        foreach(var member in _battlePlayers)
            member.HP = member.MaxHP;

        StartDungeon();
        _crawlUI.Visible = true;
        transition.PlayFadeOut();
        await ToSignal(transition.Player, "animation_finished");

        _endingScene = false;

        PersistentGameObjects.Save();
    }

    private void _OnStayButtonPressed()
    {
        _endingScene = false;
        _floorExitScene.Stay.Visible = false;
        _floorExitScene.Visible = false;
        SetEncounterVisibility(true, true);
    }

    private async void _OnRetreatButtonPressed()
    {
        if(_floorExitScene.Continue.Visible && !_prematurelyLeave)
        {
            int tier = _gameObject.TierDC;
            int maxTier = _gameObject.MaxTierDC;

            if (tier == maxTier && tier + 1 < _gameObject.TierDCCap)
            {
                _gameObject.MaxTierDC++;
                _gameObject.TierDC++;
            }
            else
            {
                _gameObject.TierDC++;
            }

            _crawlUI.Visible = false;
            _floorExitScene.Visible = false;
            var rewards = ResourceLoader.Load<PackedScene>(Scenes.REWARDS).Instantiate<RewardScreen>();
            _popup.AddChild(rewards);
            rewards.InitializeDungeonCrawlTierRewards();
            _itemSfxPlayer.Play();
            await ToSignal(rewards, "tree_exited");
        }

        SetEncounterVisibility(false);
        PersistentGameObjects.Save();
        TransitionScenes(Scenes.MAIN, _audioStreamPlayer);
    }

    private void SetEncounterVisibility(bool visible, bool keepCamera = false)
    {
        _tiles.Visible = visible;
        _camera.Enabled = visible;
        _player.Visible = visible;
        _retreat.Visible = visible;
        if (!keepCamera)
        {
            _crawlUI.Visible = visible;
            _popup.Visible = visible;
        }
    }
}
