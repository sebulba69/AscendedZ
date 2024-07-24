using AscendedZ;
using AscendedZ.currency.rewards;
using AscendedZ.game_object;
using Godot;
using System;

public partial class FountainOfBuce : CenterContainer
{
	private Button _offerBuceOrbsBtn, _backBtn;
	private Label _orbCount;
	private Label _morbis;
	private GameObject _gameObject;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_offerBuceOrbsBtn = GetNode<Button>("%OfferBuceOrbsBtn");
        _backBtn = GetNode<Button>("%BackBtn");
		_orbCount = GetNode<Label>("%OrbCountLabel");
		_morbis = GetNode<Label>("%Morbis");

		_backBtn.Pressed += QueueFree;

		_gameObject = PersistentGameObjects.GameObjectInstance();

		var currency = _gameObject.MainPlayer.Wallet.Currency;
		if (currency.ContainsKey(SkillAssets.MORBIS))
		{
			var morbis = currency[SkillAssets.MORBIS];
			_morbis.Text = morbis.Amount.ToString();
		}
		else
		{
			_morbis.Text = 0.ToString();
		}

		_orbCount.Text = $"Orbs: {_gameObject.Orbs}";

		_offerBuceOrbsBtn.Pressed += () => 
		{
			int orbs = _gameObject.Orbs;
			if(orbs - 3 >= 0)
			{
				_gameObject.Orbs-=3;
				var morbis = new Morbis() { Amount = 1 };
				if (!currency.ContainsKey(morbis.Name))
					currency.Add(morbis.Name, morbis);
				else
					currency[morbis.Name].Amount++;
			}

            _orbCount.Text = $"Orbs: {_gameObject.Orbs}";
            _morbis.Text = currency[SkillAssets.MORBIS].Amount.ToString();
        };
    }
}
