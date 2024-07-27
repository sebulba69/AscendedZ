using AscendedZ;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.upgrade_screen;
using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class UpgradeItem : VBoxContainer
{
	private TextureRect _image;
	private Label _name, _upgradeCost, _refundRewardPC, _refundRewardVC;
	private RichTextLabel _description;
	private Button _upgradeBtn, _refundBtn;
	private bool _mouseOver;

	private UpgradeItemObject _upgradeItemObject;

	public EventHandler UpdatePartyDisplay;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_image = GetNode<TextureRect>("%Picture");
		_name = GetNode<Label>("%Name");
		_upgradeBtn = GetNode<Button>("%UpgradeButton");
		_refundBtn = GetNode<Button>("%RefundButton");
		_upgradeCost = GetNode<Label>("%VCCost");
		_refundRewardPC = GetNode<Label>("%PCCost");
        _refundRewardVC = GetNode<Label>("%VPGain");
		_description = GetNode<RichTextLabel>("%Description");
        _upgradeItemObject = new UpgradeItemObject(PersistentGameObjects.GameObjectInstance());
        _upgradeBtn.Pressed += _OnUpgradeButtonPressed;
		_refundBtn.Pressed += _OnRefundButtonPressed;
	}

	public void Initialize(OverworldEntity entity)
	{
		_upgradeItemObject.Initialize(entity);
		_image.Texture = ResourceLoader.Load<Texture2D>(entity.Image);
		UpdateDisplay();
    }

	public void UpdateDisplay()
	{
		var entity = _upgradeItemObject.Entity;

		_name.Text = entity.DisplayName;
		_upgradeCost.Text = $"{entity.VorpexValue}";
		_refundRewardPC.Text = $"{entity.RefundReward}";
		_refundRewardVC.Text = $"{entity.RefundRewardVC}";
        _description.Text = entity.GetUpgradeString();

		_upgradeBtn.Disabled = entity.IsLevelCapHit;

        _refundBtn.Disabled = !_upgradeItemObject.CanRefund();
    }

	private void _OnUpgradeButtonPressed()
	{
		_upgradeItemObject.Upgrade();
		UpdateDisplay();
        UpdatePartyDisplay?.Invoke(null, EventArgs.Empty);
	}

    private void _OnRefundButtonPressed()
    {
		_upgradeItemObject.Refund();
        UpdatePartyDisplay?.Invoke(null, EventArgs.Empty);
    }
}
