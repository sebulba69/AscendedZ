using AscendedZ;
using AscendedZ.currency;
using Godot;
using System;

public partial class CurrencyDisplay : GridContainer
{
	private TextureRect _icon;
	private Label _currentyAmount;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_icon = this.GetNode<TextureRect>("Icon");
		_currentyAmount = this.GetNode<Label>("CurrencyAmount");
	}

	public void SetCurrencyToDisplay(string icon, int amount)
	{
        _icon.Call("SetIcon", icon);
		_currentyAmount.Text = amount.ToString();
    }
}
