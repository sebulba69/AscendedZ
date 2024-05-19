using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.game_object;
using Godot;
using System;

public partial class MainPlayerContainer : CenterContainer
{
	public void InitializePlayerInformation(GameObject gameObject)
	{
        TextureRect playerPicture = this.GetNode<TextureRect>("%PlayerPicture");
        Label playerName = this.GetNode<Label>("%PlayerNameLabel");

        MainPlayer player = gameObject.MainPlayer;
        playerPicture.Texture = ResourceLoader.Load<Texture2D>(player.Image);
        playerName.Text = $"[T. {gameObject.MaxTier}] {player.Name}";

        if (!gameObject.UpgradeShardsUnlocked && gameObject.MaxTier > 20)
        {
            gameObject.UpgradeShardsUnlocked = true;
            UpgradeShard upgradeShard = new UpgradeShard() { Amount = 0 };
            player.Wallet.Currency.Add(upgradeShard.Name, upgradeShard);

            PersistentGameObjects.Save();
        }

        UpdateCurrencyDisplay();
    }

    public void UpdateCurrencyDisplay()
    {
        GameObject gameObject = PersistentGameObjects.GameObjectInstance();

        var currencyDisplay = this.GetNode("%Currency");
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
    }
}
