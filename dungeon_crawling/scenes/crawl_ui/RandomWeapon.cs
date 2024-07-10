using AscendedZ.dungeon_crawling.scenes.weapon_gacha;
using AscendedZ.game_object;
using Godot;
using System;

public partial class RandomWeapon : CenterContainer
{
	private WeaponDisplay _weaponDisplay;
	private WeaponGachaGenerator _weaponGachaGenerator;
	private Button _collectBtn, _backBtn;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_weaponDisplay = this.GetNode<WeaponDisplay>("%WeaponDisplay");
		_collectBtn = this.GetNode<Button>("%CollectBtn");
		_backBtn = this.GetNode<Button>("%Back");
        _weaponGachaGenerator = new WeaponGachaGenerator();

        int tier = PersistentGameObjects.GameObjectInstance().TierDC / 2;
		var dp = PersistentGameObjects.GameObjectInstance().MainPlayer.DungeonPlayer;

        var weapons = _weaponGachaGenerator.GenerateWeapons(1, tier + 1);

		_weaponDisplay.Initialize(weapons[0]);
		_collectBtn.Disabled = dp.Reserves.AreReservesCapped();

		_collectBtn.Pressed += () => 
		{
			dp.Reserves.Add(weapons[0]);
            QueueFree();
		};

		_backBtn.Pressed += () => { QueueFree(); };
	}
}
