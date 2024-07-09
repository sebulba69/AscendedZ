using AscendedZ;
using AscendedZ.game_object;
using AscendedZ.screens;
using Godot;
using System;

public partial class DungeonCrawlMainScreen : Transitionable2DScene
{
    private CenterContainer _root;
    private VBoxContainer _mainContainer;
	private MainPlayerContainer _mainPlayerContainer;
	private AudioStreamPlayer _audioStreamPlayer;
	private Label _tooltip;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        _root = this.GetNode<CenterContainer>("%CenterContainer");
        _mainContainer = this.GetNode<VBoxContainer>("%MainContainer");
        _mainPlayerContainer = this.GetNode<MainPlayerContainer>("%MainPlayerContainer");
        _audioStreamPlayer = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        _tooltip = this.GetNode<Label>("%Tooltip");
		_mainPlayerContainer.InitializePlayerInformation(gameObject);

        _audioStreamPlayer.VolumeDb = -80;
		gameObject.MusicPlayer.SetStreamPlayer(_audioStreamPlayer);
		gameObject.MusicPlayer.PlayMusic(MusicAssets.OW_DC);
        this.GetTree().CreateTween().TweenProperty(_audioStreamPlayer, "volume_db", -10, 0.5);

        Button menuButton = this.GetNode<Button>("%MenuButton");
        Button armoryButton = this.GetNode<Button>("%ArmoryBtn");
        Button weaponShopBtn = this.GetNode<Button>("%WeaponShopBtn");
        Button embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button endlessDungeonButton = this.GetNode<Button>("%ReturnButton");

        menuButton.MouseEntered += () => { _tooltip.Text = "Save your game or quit to Title."; };
        armoryButton.MouseEntered += () => { _tooltip.Text = "Equip your Bucelous Weaponodus for maximum power."; };
        weaponShopBtn.MouseEntered += () => { _tooltip.Text = "Pull weapons from the randomized Bucilicus."; };
        embarkButton.MouseEntered += () => { _tooltip.Text = "Journey into the Labyribuce."; };
		endlessDungeonButton.MouseEntered += () => { _tooltip.Text = "Resume your journey and Ascend."; };

        menuButton.Pressed += () => DisplayScene(Scenes.MENU);
        armoryButton.Pressed += () => DisplayScene(Scenes.DUNGEON_CRAWL_ARMORY);
        weaponShopBtn.Pressed += () => DisplayScene(Scenes.DUNGEON_WEAPON_GACHA_SCREEN);
        embarkButton.Pressed += () => TransitionScenes(Scenes.DUNGEON_CRAWL, _audioStreamPlayer);
		endlessDungeonButton.Pressed += () => TransitionScenes(Scenes.MAIN, _audioStreamPlayer);

        DoEmbarkButtonCheck(gameObject);
    }

    private async void DisplayScene(string packedScenePath)
    {
        _mainContainer.Visible = false;

        var instanceOfPackedScene = ResourceLoader.Load<PackedScene>(packedScenePath).Instantiate();
        _root.AddChild(instanceOfPackedScene);

        await ToSignal(instanceOfPackedScene, "tree_exited");

        _mainContainer.Visible = true;
        _mainPlayerContainer.UpdateCurrencyDisplay();
        DoEmbarkButtonCheck(PersistentGameObjects.GameObjectInstance());
    }

    private void DoEmbarkButtonCheck(GameObject gameObject)
    {
        var dPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer.DungeonPlayer;

        var embarkButton = this.GetNode<Button>("%EmbarkButton");
        if (dPlayer.PrimaryWeapon != null)
        {
            embarkButton.Visible = true;
        }
    }
}
