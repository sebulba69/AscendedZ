using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.game_object;
using Godot;
using System;

public partial class FountainOfBuce : CenterContainer
{
	private Button _offerBuceOrbsBtn, _backBtn;
	private Label _orbCount;
	private Label _morbisLabel;
	private GameObject _gameObject;
	private Currency _morbis;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_offerBuceOrbsBtn = GetNode<Button>("%OfferBuceOrbsBtn");
        _backBtn = GetNode<Button>("%BackBtn");
		_orbCount = GetNode<Label>("%OrbCountLabel");
		_morbisLabel = GetNode<Label>("%Morbis");
		_gameObject = PersistentGameObjects.GameObjectInstance();

        _offerBuceOrbsBtn.Text = $"[{Controls.GetControlString(Controls.CONFIRM)}] {_offerBuceOrbsBtn.Text}";
        _backBtn.Text = $"[{Controls.GetControlString(Controls.BACK)}] {_backBtn.Text}";

        var currency = _gameObject.MainPlayer.Wallet.Currency;
		if (currency.ContainsKey(SkillAssets.MORBIS))
		{
            _morbis = currency[SkillAssets.MORBIS];
			_morbisLabel.Text = _morbis.Amount.ToString();
		}
		else
		{
			_morbisLabel.Text = 0.ToString();
		}

		_orbCount.Text = $"Orbs: {_gameObject.Orbs}";

        _offerBuceOrbsBtn.Pressed += _OnOfferBuceOrbsPressed;
        _backBtn.Pressed += _OnBackButtonPressed;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            _OnOfferBuceOrbsPressed();
        }

        if (@event.IsActionPressed(Controls.BACK))
        {
            _OnBackButtonPressed();
        }
    }

	private void _OnOfferBuceOrbsPressed()
	{
        var currency = _gameObject.MainPlayer.Wallet.Currency;
        int orbs = _gameObject.Orbs;
        if (orbs - 3 >= 0)
        {
            _gameObject.Orbs -= 3;
            var morbis = new Morbis() { Amount = 1 };
            if (!currency.ContainsKey(_morbis.Name))
                currency.Add(_morbis.Name, _morbis);
            else
                currency[_morbis.Name].Amount++;
        }

        _orbCount.Text = $"Orbs: {_gameObject.Orbs}";
        _morbisLabel.Text = _morbis.Amount.ToString();
    }

	private void _OnBackButtonPressed()
	{
		QueueFree();
    }
}
