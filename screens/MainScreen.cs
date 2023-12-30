using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using Godot;
using System;

public partial class MainScreen : Node2D
{
    private CenterContainer _root;
    private VBoxContainer _mainUIContainer;
    private Label _tooltip;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _root = this.GetNode<CenterContainer>("CenterContainer");
        _mainUIContainer = this.GetNode<VBoxContainer>("%MainContainer");

        TextureRect playerPicture = this.GetNode<TextureRect>("%PlayerPicture");
        Label playerName = this.GetNode<Label>("%PlayerNameLabel");
        _tooltip = this.GetNode<Label>("%Tooltip");

        AudioStreamPlayer audioPlayer = this.GetNode<AudioStreamPlayer>("MusicPlayer");
        var gameObject = PersistentGameObjects.Instance();
        gameObject.SetStreamPlayer(audioPlayer);
        gameObject.PlayMusic(MusicAssets.OVERWORLD_1);

        MainPlayer player = PersistentGameObjects.Instance().MainPlayer;
        playerPicture.Texture = ResourceLoader.Load<Texture2D>(player.Image);
        playerName.Text = player.Name;
        UpdateCurrencyDisplay();

        Button menuButton = this.GetNode<Button>("%MenuButton");
        Button embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button recruitButton = this.GetNode<Button>("%RecruitButton");

        menuButton.Pressed += _OnMenuButtonPressed;
        embarkButton.Pressed += _OnEmbarkButtonPressed;
        recruitButton.Pressed += _OnRecruitButtonPressed;

        menuButton.MouseEntered += () => { _tooltip.Text = "Save your game or quit to Title."; };
        embarkButton.MouseEntered += () => { _tooltip.Text = "Enter the Endless Dungeon with your party."; };
        recruitButton.MouseEntered += () => { _tooltip.Text = "Recruit Party Members to be used in battle."; };
    }

    private void UpdateCurrencyDisplay()
    {
        var currencyDisplay = this.GetNode("%Currency");

        foreach (var child in currencyDisplay.GetChildren())
        {
            currencyDisplay.RemoveChild(child);
        }
        var wallet = PersistentGameObjects.Instance().MainPlayer.Wallet;
        
        foreach (var key in wallet.Currency.Keys)
        {
            var display = ResourceLoader.Load<PackedScene>(Scenes.CURRENCY_DISPLAY).Instantiate();
            currencyDisplay.AddChild(display);
            var currency = wallet.Currency[key];
            display.Call("SetCurrencyToDisplay", currency.Icon, currency.Amount);
        }
    }

    private void _OnEmbarkButtonPressed()
    {
        DisplayScene(Scenes.MAIN_EMBARK);
    }

    private void _OnRecruitButtonPressed()
    {
        DisplayScene(Scenes.MAIN_RECRUIT);
    }

    private void _OnMenuButtonPressed()
    {
        _mainUIContainer.Visible = false;
        var instanceOfPackedScene = ResourceLoader.Load<PackedScene>(Scenes.MENU).Instantiate();
        
        _root.AddChild(instanceOfPackedScene);
        instanceOfPackedScene.Connect("EndMenuScene", new Callable(this, "_OnMenuClosed"));
    }

    private void _OnMenuClosed(bool quitToStart)
    {
        if (quitToStart)
        {
            var startScene = ResourceLoader.Load<PackedScene>(Scenes.START).Instantiate();
            _root.AddChild(startScene);
            this.QueueFree();
        }
        else
        {
            _mainUIContainer.Visible = true;
        }
    }

    private async void DisplayScene(string packedScenePath)
    {
        _mainUIContainer.Visible = false;

        var instanceOfPackedScene = ResourceLoader.Load<PackedScene>(packedScenePath).Instantiate();
        _root.AddChild(instanceOfPackedScene);

        await ToSignal(instanceOfPackedScene, "tree_exited");
        _mainUIContainer.Visible = true;

        UpdateCurrencyDisplay();
    }
}
