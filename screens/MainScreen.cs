using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;

public partial class MainScreen : Node2D
{
    private CenterContainer _root;
    private VBoxContainer _mainUIContainer;
    private Label _tooltip;
    private AudioStreamPlayer _audioPlayer;
    private PanelContainer _musicSelectContainer;
    private bool _checkBoxPressed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _root = this.GetNode<CenterContainer>("CenterContainer");
        _mainUIContainer = this.GetNode<VBoxContainer>("%MainContainer");
        _tooltip = this.GetNode<Label>("%Tooltip");
        _audioPlayer = this.GetNode<AudioStreamPlayer>("%MusicPlayer");
        _musicSelectContainer = this.GetNode<PanelContainer>("%MusicSelectContainer");

        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        TextureRect background = this.GetNode<TextureRect>("%Background");
        background.Texture = ResourceLoader.Load<Texture2D>(BackgroundAssets.GetBackground(gameObject.MaxTier));

        // backwards compatibility line
        foreach(var member in gameObject.MainPlayer.Party.Party)
        {
            if(member != null)
            {
                if (!gameObject.MainPlayer.ReserveMembers.Contains(member))
                {
                    member.IsInParty = true;
                    gameObject.MainPlayer.ReserveMembers.Add(member);
                }
            }
        }

        InitializeMusicButton(gameObject);
        InitializePlayerInformation(gameObject);
        InitializeButtons(gameObject);
    }

    #region Setup functions
    private void InitializeMusicButton(GameObject gameObject)
    {
        OptionButton musicOptionsButton = this.GetNode<OptionButton>("%MusicOptionsButton");
        CheckBox checkBox = this.GetNode<CheckBox>("%CheckBox");

        MusicObject musicPlayer = gameObject.MusicPlayer;
        List<string> overworldTracks = MusicAssets.GetOverworldTrackKeys();

        musicPlayer.SetStreamPlayer(_audioPlayer);

        int indexOfSongToDisplay = 0;
        for (int i = 0; i < overworldTracks.Count; i++)
        {
            string track = overworldTracks[i];
            if (track.Equals(gameObject.MusicPlayer.OverworldThemeCustom))
                indexOfSongToDisplay = i;

            musicOptionsButton.AddItem(track);
        }

        musicOptionsButton.Select(indexOfSongToDisplay);

        musicOptionsButton.ItemSelected += (long index) =>
        {
            musicPlayer.OverworldThemeCustom = musicOptionsButton.GetItemText((int)index);
            PersistentGameObjects.Save();

            musicPlayer.PlayMusic(MusicAssets.GetOverworldTrackPath(musicPlayer.OverworldThemeCustom));
        };

        checkBox.Toggled += (bool state) => 
        {
            musicPlayer.ResetAllTracksAfterBoss();
            musicPlayer.IsMusicCustom = state;
            SwapOverworldTracks(musicPlayer);
        };

        SwapOverworldTracks(musicPlayer);
    }

    private void SwapOverworldTracks(MusicObject musicPlayer)
    {
        OptionButton musicOptionsButton = this.GetNode<OptionButton>("%MusicOptionsButton");
        CheckBox checkBox = this.GetNode<CheckBox>("%CheckBox");

        string track;
        if (musicPlayer.IsMusicCustom)
        {
            musicOptionsButton.Visible = true;
            checkBox.Text = "Normal";
            checkBox.ButtonPressed = true;
            track = MusicAssets.GetOverworldTrackPath(musicPlayer.OverworldThemeCustom);
        }
        else
        {
            musicOptionsButton.Visible = false;
            checkBox.Text = "Custom";
            checkBox.ButtonPressed = false;
            track = MusicAssets.GetOverworldTrackPath(musicPlayer.OverworldTheme);
        }

        musicPlayer.PlayMusic(track);
    }

    private void InitializePlayerInformation(GameObject gameObject)
    {
        TextureRect playerPicture = this.GetNode<TextureRect>("%PlayerPicture");
        Label playerName = this.GetNode<Label>("%PlayerNameLabel");

        MainPlayer player = gameObject.MainPlayer;
        playerPicture.Texture = ResourceLoader.Load<Texture2D>(player.Image);
        playerName.Text = $"[T. {gameObject.MaxTier}] {player.Name}";

        if(!gameObject.UpgradeShardsUnlocked && gameObject.MaxTier > 20)
        {
            gameObject.UpgradeShardsUnlocked = true;
            UpgradeShard upgradeShard = new UpgradeShard() { Amount = 0 };
            player.Wallet.Currency.Add(upgradeShard.Name, upgradeShard);

            PersistentGameObjects.Save();
        }
        
        UpdateCurrencyDisplay();
    }

    private void InitializeButtons(GameObject gameObject)
    {
        int tier = gameObject.MaxTier;
        
        Button menuButton = this.GetNode<Button>("%MenuButton");
        Button embarkButton = this.GetNode<Button>("%EmbarkButton");
        Button recruitButton = this.GetNode<Button>("%RecruitButton");
        Button upgradeButton = this.GetNode<Button>("%UpgradePartyButton");
        Button fuseButton = this.GetNode<Button>("%FuseButton");
        Button questButton = this.GetNode<Button>("%QuestButton");

        string upgradeText = "Upgrade";
        var progressFlagObject = gameObject.ProgressFlagObject;

        if (tier > TierRequirements.UPGRADE_SCREEN)
        {
            upgradeButton.Visible = true;

            if(tier > TierRequirements.QUESTS_PARTY_MEMBERS_UPGRADE
                && !progressFlagObject.AscensionViewed)
            {
                upgradeText += " (!)";
            }

            upgradeButton.Text = upgradeText;
        }

        string recruitText = "Recruit";
        if (tier > TierRequirements.QUESTS)
        {
            questButton.Visible = true;
            if (!progressFlagObject.CustomPartyMembersViewed)
            {
                recruitText += " (!)";
            }

            recruitButton.Text = recruitText;
        }

        if (tier > TierRequirements.FUSE)
            fuseButton.Visible = true;

        menuButton.Pressed += _OnMenuButtonPressed;
        embarkButton.Pressed += _OnEmbarkButtonPressed;
        recruitButton.Pressed += _OnRecruitButtonPressed;
        upgradeButton.Pressed += _OnUpgradeButtonPressed;
        fuseButton.Pressed += _OnFuseButtonPressed;
        questButton.Pressed += _OnQuestButtonPressed;

        menuButton.MouseEntered += () => { _tooltip.Text = "Save your game or quit to Title."; };
        embarkButton.MouseEntered += () => { _tooltip.Text = "Enter the Endless Dungeon with your party."; };
        recruitButton.MouseEntered += () => { _tooltip.Text = "Recruit Party Members to be used in battle."; };
        questButton.MouseEntered += () => { _tooltip.Text = "Get quests to earn Vorpex."; };
        upgradeButton.MouseEntered += () => { _tooltip.Text = "Upgrade Party Members with Vorpex."; };
        fuseButton.MouseEntered += () => { _tooltip.Text = "Combine Party Members to create new ones and transfer skills."; };
    }
    #endregion

    private void UpdateCurrencyDisplay()
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        var currencyDisplay = this.GetNode("%Currency");
        var embarkButton = this.GetNode<Button>("%EmbarkButton");
        var wallet = gameObject.MainPlayer.Wallet;

        foreach (var child in currencyDisplay.GetChildren())
        {
            currencyDisplay.RemoveChild(child);
        }

        foreach (var key in wallet.Currency.Keys)
        {
            var display = ResourceLoader.Load<PackedScene>(Scenes.CURRENCY_DISPLAY).Instantiate();
            currencyDisplay.AddChild(display);
            var currency = wallet.Currency[key];
            display.Call("SetCurrencyToDisplay", currency.Icon, currency.Amount);
        }

        if (gameObject.PartyMemberObtained)
            embarkButton.Visible = true;
    }
    private void _OnMenuButtonPressed()
    {
        _mainUIContainer.Visible = false;
        var instanceOfPackedScene = ResourceLoader.Load<PackedScene>(Scenes.MENU).Instantiate();

        _root.AddChild(instanceOfPackedScene);
        instanceOfPackedScene.Connect("EndMenuScene", new Callable(this, "_OnMenuClosed"));
    }

    private void _OnEmbarkButtonPressed()
    {
        DisplayScene(Scenes.MAIN_EMBARK);
    }

    private void _OnRecruitButtonPressed()
    {
        DisplayScene(Scenes.MAIN_RECRUIT);
    }

    private void _OnUpgradeButtonPressed()
    {
        DisplayScene(Scenes.UPGRADE);
    }

    private void _OnFuseButtonPressed()
    {
        DisplayScene(Scenes.FUSION);
    }

    private void _OnQuestButtonPressed()
    {
        DisplayScene(Scenes.QUEST);
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
        _musicSelectContainer.Visible = false;

        var instanceOfPackedScene = ResourceLoader.Load<PackedScene>(packedScenePath).Instantiate();
        _root.AddChild(instanceOfPackedScene);

        await ToSignal(instanceOfPackedScene, "tree_exited");

        _mainUIContainer.Visible = true;
        _musicSelectContainer.Visible = true;

        var pFlags = PersistentGameObjects.GameObjectInstance().ProgressFlagObject;

        if (pFlags.CustomPartyMembersViewed)
            this.GetNode<Button>("%RecruitButton").Text = "Recruit";

        if (pFlags.AscensionViewed)
            this.GetNode<Button>("%UpgradePartyButton").Text = "Upgrade";

        UpdateCurrencyDisplay();
    }
}
