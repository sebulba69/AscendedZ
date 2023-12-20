using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.entities;
using AscendedZ.sigils;
using Godot;
using System;

public partial class SigilUpgradeBar : HBoxContainer
{
	private readonly string READY_REMOVE = "ReadyToRemoveFromUI";

	private int _sigilIndex;
	private Button _levelUpButton, _deleteButton;
	private ProgressBar _xpProgressBar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.AddUserSignal(READY_REMOVE);
		
		_xpProgressBar = this.GetNode<ProgressBar>("%XPProgressBar");

		_levelUpButton = this.GetNode<Button>("%LevelUpButton");
		_deleteButton = this.GetNode<Button>("%DeleteButton");

		_levelUpButton.Pressed += _OnLevelUpButtonClicked;
		_deleteButton.Pressed += _OnDeleteButtonClicked;
	}

	public void Initialize(int sigilIndex)
	{
		_sigilIndex = sigilIndex;
		GameObject gameObject = PersistentGameObjects.Instance();
		BossSigil sigil = gameObject.MainPlayer.LockedSigils[_sigilIndex];
	}

	private void _OnLevelUpButtonClicked()
	{
		GameObject gameObject = PersistentGameObjects.Instance();
		MainPlayer player = gameObject.MainPlayer;
		Wallet wallet = player.Wallet;
		BossSigil sigil = player.LockedSigils[_sigilIndex];
		string auraKey = "Sigil Aura";

		Currency sigilAura = wallet.Currency[auraKey];
		if(sigilAura.Amount - 1 > 0)
		{
			sigilAura.Amount--;
			sigil.CurrentXP++;
			if(sigil.CurrentXP == sigil.XPRequiredForUse)
			{
				player.LockedSigils.Remove(sigil);
				player.UnlockedSigils.Add(sigil);
				this.EmitSignal(READY_REMOVE);
			}
		}
	}

	private void _OnDeleteButtonClicked()
	{
		MainPlayer player = PersistentGameObjects.Instance().MainPlayer;
		BossSigil sigil = player.LockedSigils[_sigilIndex];
		player.LockedSigils.Remove(sigil);
		this.EmitSignal(READY_REMOVE);
	}
}
