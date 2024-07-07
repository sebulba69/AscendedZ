using AscendedZ;
using AscendedZ.dungeon_crawling.scenes.weapon_gacha;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class WeaponGachaScreen : Control
{
	private GridContainer _weaponGachaDisplay;
	private Button _claimBtn, _rollOneBtn, _rollTenBtn, _levelShopBtn, _backBtn;
	private TextEdit _ownedDellencoin;
	private WeaponGachaObject _weaponGachaObject;
	private Label _shopLevel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_weaponGachaDisplay = this.GetNode<GridContainer>("%GachaWeaponDisplay");
		_shopLevel = this.GetNode<Label>("%ShopLevelLabel");
        _claimBtn = this.GetNode<Button>("%ClaimBtn");
        _rollOneBtn = this.GetNode<Button>("%RollOneBtn");
        _rollTenBtn = this.GetNode<Button>("%RollTenBtn");
		_levelShopBtn = this.GetNode<Button>("%LevelShopBtn");
        _backBtn = this.GetNode<Button>("%BackBtn");
		_ownedDellencoin = this.GetNode<TextEdit>("%OwnedDellencoin");

		var go = PersistentGameObjects.GameObjectInstance();
		var wallet = go.MainPlayer.Wallet;
		var gbPlayer = go.MainPlayer.DungeonPlayer;

		_weaponGachaObject = new WeaponGachaObject(gbPlayer, wallet);

		_rollOneBtn.Pressed += RollOneBtnPressed;
		_rollTenBtn.Pressed += RollTenBtnPressed;
		_claimBtn.Pressed += ClaimBtnPressed;
		_backBtn.Pressed += () => this.QueueFree();

        _ownedDellencoin.Text = $"{_weaponGachaObject.GetDellenCoinsOwned()} D$";

        ResetButtons();
    }

	private void ResetButtons()
	{
		_rollOneBtn.Disabled = !_weaponGachaObject.IsSpaceForWeapons(1);
        _rollTenBtn.Disabled = !_weaponGachaObject.IsSpaceForWeapons(10);
		_levelShopBtn.Disabled = !_weaponGachaObject.CanPlayerAffordWeapon(100);

        _claimBtn.Disabled = true;
		_shopLevel.Text = $"Shop Lvl: {_weaponGachaObject.GetShopLevel()}";
    }

	private void RollOneBtnPressed()
	{
		if (_weaponGachaObject.CanPlayerAffordWeapon(1))
		{
            _rollOneBtn.Disabled = true;
            _rollTenBtn.Disabled = true;
            _claimBtn.Disabled = false;

            GenerateWeapons(1);
        }
	}

    private void RollTenBtnPressed()
    {
		if (_weaponGachaObject.CanPlayerAffordWeapon(10))
		{
            _rollOneBtn.Disabled = true;
            _rollTenBtn.Disabled = true;
            _claimBtn.Disabled = false;

            GenerateWeapons(10);
        }
    }

	private void LevelUpShopPressed()
	{
		if (_weaponGachaObject.CanPlayerAffordWeapon(100))
		{
			_weaponGachaObject.LevelUpShop();
			ResetButtons();
		}
	}

	private void ClaimBtnPressed()
	{
		_weaponGachaObject.ClaimWeapons();
		_weaponGachaDisplay.GetChildren().Clear();
		ResetButtons();

		PersistentGameObjects.Save();
	}

    private async void GenerateWeapons(int number)
    {
        var weapons = _weaponGachaObject.GenerateWeapons(number);
        _ownedDellencoin.Text = $"{_weaponGachaObject.GetDellenCoinsOwned()} D$";
        foreach (var weapon in weapons)
        {
            var display = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_WEAPON_DISPLAY).Instantiate<WeaponDisplay>();
            _weaponGachaDisplay.AddChild(display);
            display.Initialize(weapon);
            await Task.Delay(500);
        }
    }
}
