using AscendedZ;
using AscendedZ.dungeon_crawling.scenes.minion_hut;
using AscendedZ.game_object;
using Godot;
using System;

public partial class MinionHut : CenterContainer
{
	private TextureRect _previewMinionImage;
	private WeaponDisplay _weaponDisplay;
	private ItemList _collectedMinions;
	private Button _summonBtn, _boostBtn, _equipBtn, _unequipBtn, _deleteBtn, _backBtn;

	private MinionHutObject _minionHutObject;

	private int _selected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_previewMinionImage = this.GetNode<TextureRect>("%MinionImage");
		_weaponDisplay = this.GetNode<WeaponDisplay>("%WeaponDisplay");
		_collectedMinions = this.GetNode<ItemList>("%CollectedMinions");
		_summonBtn = this.GetNode<Button>("%SummonMinionBtn");
		_boostBtn = this.GetNode<Button>("%BoostMinionBtn");
        _equipBtn = this.GetNode<Button>("%EquipBtn");
        _unequipBtn = this.GetNode<Button>("%UnequipBtn");
        _deleteBtn = this.GetNode<Button>("%DeleteBtn");
		_backBtn = this.GetNode<Button>("%BackBtn");

		_collectedMinions.ItemSelected += _OnMinionSelected;

		_summonBtn.Pressed += _OnSummonMinionPressed;
		_boostBtn.Pressed += _OnBoostMinionPressed;
		_equipBtn.Pressed += _OnEquipMinionPressed;
		_unequipBtn.Pressed += _OnUnequipMinionPressed;
        _deleteBtn.Pressed += _OnDeleteMinionPressed;
		_backBtn.Pressed += () => { this.QueueFree(); };
		
		_selected = -1;

        _minionHutObject = new MinionHutObject();
		PopulateMinionList();
    }

	private void PopulateMinionList()
	{
		_collectedMinions.Clear();

        var minions = _minionHutObject.GetMinions();
		foreach (var minion in minions)
        {
			string weaponString = minion.Weapon.GetMinionString();

			if(minion.Equipped)
				weaponString = "[E] " + weaponString;

            _collectedMinions.AddItem(weaponString, SkillAssets.GenerateIcon(minion.Weapon.Icon));
        }

		if (_selected >= minions.Count)
			_selected = minions.Count - 1;
		else if(minions.Count == 0)
			_selected = -1;
	}

    private void _OnMinionSelected(long index)
    {
		_selected = (int)index;
		var minion = _minionHutObject.GetMinion(_selected);
        _previewMinionImage.Texture = ResourceLoader.Load<Texture2D>(minion.Image);
		_weaponDisplay.Visible = true;
		_weaponDisplay.Initialize(minion.Weapon);
    }

    private void _OnSummonMinionPressed()
    {
        _minionHutObject.MakeMinion();
        PopulateMinionList();
        PersistentGameObjects.Save();
    }

	private void _OnBoostMinionPressed()
	{
        if (_selected > -1)
        {
			_minionHutObject.UpgradeMinion(_selected);
			_OnMinionSelected(_selected);
            PopulateMinionList();
            PersistentGameObjects.Save();
        }
    }

	private void _OnEquipMinionPressed()
	{
		if(_selected > -1)
		{
            _minionHutObject.EquipMinion(_selected);
            PopulateMinionList();
            PersistentGameObjects.Save();
        }
	}

    private void _OnUnequipMinionPressed()
    {
        if (_selected > -1)
        {
            _minionHutObject.UnequipMinion(_selected);
            PopulateMinionList();
            PersistentGameObjects.Save();
        }
    }

    private void _OnDeleteMinionPressed()
	{
		if(_selected > -1)
		{
			_minionHutObject.DeleteMinion(_selected);
            PopulateMinionList();
			PersistentGameObjects.Save();
        }
	}
}
