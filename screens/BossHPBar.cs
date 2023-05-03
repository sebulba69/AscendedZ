using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using Godot;
using System;

public partial class BossHPBar : HBoxContainer
{
	private ProgressBar _hp;
	private Label _barsLabel;
	private BossHP _bossHPBar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hp = this.GetNode<ProgressBar>("%HPBar");
		_barsLabel = this.GetNode<Label>("%NumberOfBarsLabel");
	}

	public void InitializeBossHPBar(int hp)
	{
		_bossHPBar = new BossHP();
		_bossHPBar.InitializeBossHP(hp);
		UpdateBossHP(hp);
    }

	public void UpdateBossHP(int hp)
	{
		_bossHPBar.ChangeHP(hp);
        BossHPStatus status = _bossHPBar.BossHPUIValues;

        _barsLabel.Text = status.NumBars.ToString();
        _hp.MaxValue = status.MaxBarHP;
		_hp.Value = status.CurrentBarHP;
    }
}
