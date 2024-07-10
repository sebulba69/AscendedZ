using Godot;
using System;

public partial class DungeonCrawlUI : PanelContainer
{
	private Label _tier, _hp, _dellencoin, _reserves;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tier = this.GetNode<Label>("%TierLabel");
		_hp = this.GetNode<Label>("%HPLabel");
		_dellencoin = this.GetNode<Label>("%DellencoinLabel");
		_reserves = this.GetNode<Label>("%ReservesLabel");
	}

	public void SetValues(int tier, long hp, long maxHP, int dellencoin, string reserveCount)
	{
		_tier.Text = $"Tier: {tier}";
		_hp.Text = $"{hp}/{maxHP} HP";
		_dellencoin.Text = $"D$ {dellencoin}";
		_reserves.Text = reserveCount;
	}
}
