using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;

public partial class StartScreen : Node2D
{

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

		// set nodes we'll cycle between based on button clicks
		_startingButtons = this.GetNode<HBoxContainer>("CenterContainer/VBoxContainer/StartingButtons");
		_newGameControls = this.GetNode<HBoxContainer>("CenterContainer/VBoxContainer/NewGameButtons");
		_loadGameControls = this.GetNode<HBoxContainer>("CenterContainer/VBoxContainer/LoadingButtons");

		// buttons from start screen
		Button newGameButton = this.GetNode<Button>("CenterContainer/VBoxContainer/StartingButtons/GridContainer/NewGameButton");
		Button loadGameButton = this.GetNode<Button>("CenterContainer/VBoxContainer/StartingButtons/GridContainer/LoadGameButton");

		newGameButton.Connect("pressed",new Callable(this,"_OnNewGameButtonPressed"));
		loadGameButton.Connect("pressed",new Callable(this,"_OnLoadScreenButtonClicked"));

		// assets from newGameScreen
		_pictureIndex = 0;
		_playerPicture = this.GetNode<TextureRect>("CenterContainer/VBoxContainer/NewGameButtons/NewPlayerPicture");
		_playerPicture.Texture = ResourceLoader.Load<Texture2D>(ArtAssets.PlayerPics[_pictureIndex]);

		Button newGameBackButton = this.GetNode<Button>("CenterContainer/VBoxContainer/NewGameButtons/VBoxContainer/GoBackButton");
		Button ascendButton = this.GetNode<Button>("CenterContainer/VBoxContainer/NewGameButtons/VBoxContainer/StartNGButton");
		Button leftButton = this.GetNode<Button>("CenterContainer/VBoxContainer/NewGameButtons/LeftButton");
		Button rightButton = this.GetNode<Button>("CenterContainer/VBoxContainer/NewGameButtons/RightButton");

		newGameBackButton.Pressed += _OnNewGameBackButtonPressed;
		ascendButton.Pressed += _OnAscendButtonPressed;
		leftButton.Pressed += _OnPlayerPicLeftButtonPressed;
		rightButton.Pressed += _OnPlayerPicRightButtonPressed;

        // assets from loadGameScreen
        var gameObject = PersistentGameObjects.Instance();

		_loadItems = this.GetNode<ItemList>("CenterContainer/VBoxContainer/LoadingButtons/VBoxContainer/ItemList");
		Button loadContinueButton = this.GetNode<Button>("CenterContainer/VBoxContainer/LoadingButtons/VBoxContainer/GridContainer/LoadContButton");
		Button loadBackButton = this.GetNode<Button>("CenterContainer/VBoxContainer/LoadingButtons/VBoxContainer/GridContainer/LoadBackButton");
		Button loadDeleteButton = this.GetNode<Button>("CenterContainer/VBoxContainer/LoadingButtons/VBoxContainer/GridContainer/LoadDeleteButton");

		foreach (var item in gameObject.SaveCache)
			_loadItems.AddItem(item.ToString());

		loadContinueButton.Pressed += _OnLoadSaveFileClicked;
		loadBackButton.Pressed += _OnLoadGameBackButtonPressed;
		loadDeleteButton.Pressed += _OnLoadDeleteButtonPressed;
    }

	private void _OnPlayerPicLeftButtonPressed()
	{
		List<string> pictures = ArtAssets.PlayerPics;

		_pictureIndex--;
		if (_pictureIndex < 0)
			_pictureIndex = 0;

		// set player pic here
		_playerPicture.Texture = ResourceLoader.Load<Texture2D>(pictures[_pictureIndex]);
	}

	private void _OnPlayerPicRightButtonPressed()
	{
		List<string> pictures = ArtAssets.PlayerPics;

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

		CreateNewMainCharacter(playerName.Text, playerPicture.Texture.ResourcePath);

		// start cutscene
		PackedScene openingCutscene = ResourceLoader.Load<PackedScene>(Scenes.CUTSCENE);
		
		// Must be initialized before we can start manipulating the assets for it.
		var instancedCutscene = openingCutscene.Instantiate();
		this.GetTree().Root.AddChild(instancedCutscene);

		instancedCutscene.Call("StartCutscene", DialogScenes.Opening);
		await ToSignal(instancedCutscene, "CutsceneEndedEventHandler");

		// go to main screen
		EnterMainScreen();
	}

	private void CreateNewMainCharacter(string name, string imagePath)
	{
		// grab a reference to our persistent game object
		GameObject gameObject = PersistentGameObjects.Instance();

        gameObject.MainPlayer = new MainPlayer()
		{
			Name = name,
			Image = imagePath
		};

		Vorpex vorpex = new Vorpex() { Amount = 1 };

        gameObject.MainPlayer
			.Wallet.Currency.Add(vorpex.Name, vorpex);

		gameObject.Tier = 1;
		gameObject.MaxTier = 1;

        PersistentGameObjects.SaveNew();
	}

	private void _OnLoadSaveFileClicked()
	{
		var persistentGameObject = PersistentGameObjects.Instance();

		if (persistentGameObject.SaveCache.Count > 0)
		{
            if (_loadItems.GetSelectedItems().Length == 0)
                return;

            // There can only be one selected item at a time.
            int selectedIndex = _loadItems.GetSelectedItems()[0];
			var entry = persistentGameObject.SaveCache[selectedIndex];

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
            var persistentGameObject = PersistentGameObjects.Instance();

            if (persistentGameObject.SaveCache.Count > 0)
            {
                if (_loadItems.GetSelectedItems().Length == 0)
                    return;

                // There can only be one selected item at a time.
                int selectedIndex = _loadItems.GetSelectedItems()[0];
                PersistentGameObjects.DeleteSaveAtIndex(selectedIndex);

                _loadItems.Clear();
                foreach (var item in persistentGameObject.SaveCache)
                    _loadItems.AddItem(item.ToString());
            }
        }
		_mainTitleLabel.Visible = true;
        _loadGameControls.Visible = true;
    }

    private void EnterMainScreen()
	{
		PackedScene mainScreenScene = ResourceLoader.Load<PackedScene>(Scenes.MAIN);
		this.GetTree().Root.AddChild(mainScreenScene.Instantiate());
		this.QueueFree();
	}
}
