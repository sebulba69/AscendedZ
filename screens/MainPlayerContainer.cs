using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.game_object;
using Godot;
using System;

public partial class MainPlayerContainer : CenterContainer
{
	public void InitializePlayerInformation(GameObject gameObject, bool isDungeonCrawling = false)
	{
        TextureRect playerPicture = this.GetNode<TextureRect>("%PlayerPicture");
        Label playerName = this.GetNode<Label>("%PlayerNameLabel");

        MainPlayer player = gameObject.MainPlayer;
        playerPicture.Texture = ResourceLoader.Load<Texture2D>(player.Image);
        int tier = (isDungeonCrawling) ? gameObject.MaxTierDC : gameObject.MaxTier;
        playerName.Text = $"[T. {tier}] {player.Name}";

        UpdateCurrencyDisplay();
    }

    public void UpdatePlayerPic(string pic)
    {
        GetNode<TextureRect>("%PlayerPicture").Texture = ResourceLoader.Load<Texture2D>(pic);
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
            var display = ResourceLoader.Load<PackedScene>(Scenes.CURRENCY_DISPLAY).Instantiate<CurrencyDisplay>();
            currencyDisplay.AddChild(display);
            var currency = wallet.Currency[key];
            display.SetCurrencyToDisplay(currency.Icon, currency);
        }
    }
}
