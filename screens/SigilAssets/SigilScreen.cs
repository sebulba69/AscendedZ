using AscendedZ;
using AscendedZ.currency;
using AscendedZ.entities;
using Godot;
using System;

public partial class SigilScreen : Control
{
	private readonly string SGL_LVL_PATH = "res://sigils/SigilUpgradeBar.tscn";

	private VBoxContainer _sigilVBox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sigilVBox = this.GetNode<VBoxContainer>("%SigilVBox");

		Button unlockButton = this.GetNode<Button>("%UnlockButton");
		Button backButton = this.GetNode<Button>("%BackButton");

		unlockButton.ButtonDown += () => 
		{
			GameObject gameObject = PersistentGameObjects.Instance();
			MainPlayer player = gameObject.MainPlayer;
			Currency keys = player.Wallet.Currency[ArtAssets.SIGILKEY_ICON];
			if (player.LockedSigils.Count > 0 && keys.Amount > 0)
            {
				keys.Amount--;
				// player.LockedSigils[]
            }
			// SigilUpgradeBar sigilUpgradeBar = ResourceLoader.Load<SigilUpgradeBar>(SGL_LVL_PATH);
			// sigilUpgradeBar
			// sigilUpgradeBar.Initialize();
		};
    }
}
