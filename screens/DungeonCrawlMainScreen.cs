using AscendedZ;
using AscendedZ.game_object;
using AscendedZ.screens;
using Godot;
using System;

public partial class DungeonCrawlMainScreen : Transitionable2DScene
{
	private MainPlayerContainer _mainPlayerContainer;
	private AudioStreamPlayer _audioStreamPlayer;
	private Label _tooltip;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        _mainPlayerContainer = this.GetNode<MainPlayerContainer>("%MainPlayerContainer");
        _audioStreamPlayer = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        _tooltip = this.GetNode<Label>("%Tooltip");
		_mainPlayerContainer.InitializePlayerInformation(gameObject);

        _audioStreamPlayer.VolumeDb = -80;
		gameObject.MusicPlayer.SetStreamPlayer(_audioStreamPlayer);
		gameObject.MusicPlayer.PlayMusic(MusicAssets.OW_DC);
        this.GetTree().CreateTween().TweenProperty(_audioStreamPlayer, "volume_db", -10, 0.5);

        Button embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button endlessDungeonButton = this.GetNode<Button>("%ReturnButton");

        embarkButton.MouseEntered += () => { _tooltip.Text = "Journey into the Labyribuce."; };
		endlessDungeonButton.MouseEntered += () => { _tooltip.Text = "Resume your journey and Ascend."; };

        embarkButton.Pressed += () => TransitionScenes(Scenes.DUNGEON_CRAWL, _audioStreamPlayer);
		endlessDungeonButton.Pressed += () => TransitionScenes(Scenes.MAIN, _audioStreamPlayer);
    }
}
