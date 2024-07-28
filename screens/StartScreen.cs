using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Xml.Linq;

public partial class StartScreen : Transitionable2DScene
{
	private readonly string VERSION = "Pre-Alpha v0.04.32";

	/// <summary>
	/// Node that appears on the starting screen.
	/// This will need to be invisible when the
	/// New Game buttons are visible.
	/// </summary>
	private HBoxContainer _startingButtons;

	/// <summary>
	/// Nodes that will appear if the "New Game" button is
	/// pressed.
	/// </summary>
	private HBoxContainer _newGameControls;

	/// <summary>
	/// Nodes that will appear if "Load Game" is pressed
	/// </summary>
	private HBoxContainer _loadGameControls;

	/// <summary>
	/// For displaying the SaveCache from PersistentGameObject
	/// </summary>
	private ItemList _loadItems;

	private TextureRect _playerPicture;

	/// <summary>
	/// Index for the possible pictures a new player can pick.
	/// </summary>
	private int _pictureIndex;

	private Label _mainTitleLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainTitleLabel = this.GetNode<Label>("%MainAscendedLabel");
		Label versionLabel = this.GetNode<Label>("%VersionLabel");

		// set nodes we'll cycle between based on button clicks
		_startingButtons = this.GetNode<HBoxContainer>("%StartingButtons");
		_newGameControls = this.GetNode<HBoxContainer>("%NewGameButtons");
		_loadGameControls = this.GetNode<HBoxContainer>("%LoadingButtons");

		// buttons from start screen
		Button newGameButton = this.GetNode<Button>("%NewGameButton");
		Button loadGameButton = this.GetNode<Button>("%LoadGameButton");
		Button quitGameButton = this.GetNode<Button>("%QuitGameButton");

		newGameButton.Pressed += _OnNewGameButtonPressed;
        loadGameButton.Pressed += _OnLoadScreenButtonClicked;
        quitGameButton.Pressed += () => { this.GetTree().Quit(); };

        versionLabel.Text = VERSION;

		// assets from newGameScreen
		_pictureIndex = 0;
		_playerPicture = this.GetNode<TextureRect>("%NewPlayerPicture");
		_playerPicture.Texture = ResourceLoader.Load<Texture2D>(CharacterImageAssets.PlayerPics[_pictureIndex]);

		Button newGameBackButton = this.GetNode<Button>("%GoBackButton");
		Button ascendButton = this.GetNode<Button>("%StartNGButton");
		Button leftButton = this.GetNode<Button>("%LeftButton");
		Button rightButton = this.GetNode<Button>("%RightButton");

		newGameBackButton.Pressed += _OnNewGameBackButtonPressed;
		ascendButton.Pressed += _OnAscendButtonPressed;
		leftButton.Pressed += _OnPlayerPicLeftButtonPressed;
		rightButton.Pressed += _OnPlayerPicRightButtonPressed;

        // assets from loadGameScreen
        var saveObject = PersistentGameObjects.SaveObjectInstance();

		_loadItems = this.GetNode<ItemList>("%ItemList");
		Button loadContinueButton = this.GetNode<Button>("%LoadContButton");
		Button loadBackButton = this.GetNode<Button>("%LoadBackButton");
		Button loadDeleteButton = this.GetNode<Button>("%LoadDeleteButton");

		foreach (var item in saveObject.SaveCache)
			_loadItems.AddItem(item.ToString());

		if(_loadItems.ItemCount > 0)
			_loadItems.Select(0);

		loadContinueButton.Pressed += _OnLoadSaveFileClicked;
		loadBackButton.Pressed += _OnLoadGameBackButtonPressed;
		loadDeleteButton.Pressed += _OnLoadDeleteButtonPressed;
    }

	private void _OnPlayerPicLeftButtonPressed()
	{
		List<string> pictures = CharacterImageAssets.PlayerPics;

		_pictureIndex--;
		if (_pictureIndex < 0)
			_pictureIndex = 0;

		// set player pic here
		_playerPicture.Texture = ResourceLoader.Load<Texture2D>(pictures[_pictureIndex]);
	}

	private void _OnPlayerPicRightButtonPressed()
	{
		List<string> pictures = CharacterImageAssets.PlayerPics;

		_pictureIndex++;
		if (_pictureIndex == pictures.Count)
			_pictureIndex = pictures.Count - 1;

		// set player pic here
		_playerPicture.Texture = ResourceLoader.Load<Texture2D>(pictures[_pictureIndex]);
	}

	private void _OnNewGameButtonPressed()
	{
		_startingButtons.Visible = false;
		_newGameControls.Visible = true;
	}

	private void _OnLoadScreenButtonClicked()
	{
		_startingButtons.Visible = false;
		_loadGameControls.Visible = true;
	}

	private void _OnNewGameBackButtonPressed()
	{
		_startingButtons.Visible = true;
		_newGameControls.Visible = false;
	}

	private void _OnLoadGameBackButtonPressed()
	{
		_startingButtons.Visible = true;
		_loadGameControls.Visible = false;
	}

	private async void _OnAscendButtonPressed()
	{
		TextEdit playerName = this.GetNode<TextEdit>("CenterContainer/VBoxContainer/NewGameButtons/VBoxContainer/PanelContainer/TextEdit");
		TextureRect playerPicture = this.GetNode<TextureRect>("CenterContainer/VBoxContainer/NewGameButtons/NewPlayerPicture");

        PersistentGameObjects.NewGame(playerName.Text, playerPicture.Texture.ResourcePath);

		// start cutscene
		PackedScene openingCutscene = ResourceLoader.Load<PackedScene>(Scenes.CUTSCENE);
		
		// Must be initialized before we can start manipulating the assets for it.
		var instancedCutscene = openingCutscene.Instantiate();
		this.GetTree().Root.AddChild(instancedCutscene);

		var streamPlayer = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
		streamPlayer.Stream = ResourceLoader.Load<AudioStream>(MusicAssets.FIRST_CUTSCENE);
		streamPlayer.Play();
		instancedCutscene.Call("StartCutscene", DialogScenes.Opening);
		await ToSignal(instancedCutscene, "CutsceneEndedEventHandler");

		// go to main screen
		EnterMainScreen();
	}

	private void _OnLoadSaveFileClicked()
	{
		var saveObject = PersistentGameObjects.SaveObjectInstance();

		if (saveObject.SaveCache.Count > 0)
		{
            if (_loadItems.GetSelectedItems().Length == 0)
                return;

            // There can only be one selected item at a time.
            int selectedIndex = _loadItems.GetSelectedItems()[0];
			var entry = saveObject.SaveCache[selectedIndex];

			// load the selected entry into memory
			PersistentGameObjects.Load(entry);

			// go to main screen
			EnterMainScreen();
		}
	}


    private void _OnLoadDeleteButtonPressed()
    {
        _loadGameControls.Visible = false;
		_mainTitleLabel.Visible = false;

        var popupWindow = ResourceLoader.Load<PackedScene>(Scenes.YES_NO_POPUP).Instantiate();
        this.GetTree().Root.AddChild(popupWindow);
        popupWindow.Call("SetDialogMessage", "Are you sure you want to delete your save data?");
        popupWindow.Connect("AnswerSelected", new Callable(this, "_OnDialogResultReceived"));
    }

	private void _OnDialogResultReceived(bool isYesButtonPressed)
	{
		if (isYesButtonPressed)
		{
            var saveObject = PersistentGameObjects.SaveObjectInstance();

            if (saveObject.SaveCache.Count > 0)
            {
                if (_loadItems.GetSelectedItems().Length == 0)
                    return;

                // There can only be one selected item at a time.
                int selectedIndex = _loadItems.GetSelectedItems()[0];
                PersistentGameObjects.DeleteSaveAtIndex(selectedIndex);

                _loadItems.Clear();
                foreach (var item in saveObject.SaveCache)
                    _loadItems.AddItem(item.ToString());

                if (_loadItems.ItemCount > 0)
                    _loadItems.Select(0);
            }
        }
		_mainTitleLabel.Visible = true;
        _loadGameControls.Visible = true;
    }

    private void EnterMainScreen()
	{
		TransitionScenes(Scenes.MAIN, this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer"));
	}
}
