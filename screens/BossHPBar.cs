using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using Godot;
using System;

public partial class BossHPBar : HBoxContainer
{
	private ProgressBar _hp;
	private Label _barsLabel;
	private BossHP _bossHPBar;

	private readonly string FRONT = "res://FrontBoxBossHP.tres";
	private readonly string BACK = "res://BackBoxBossHP.tres";

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
        UpdateBossHPUI();
    }

	public void UpdateBossHP(int hp)
	{
        _bossHPBar.ChangeHP(hp);
		UpdateBossHPUI();
    }

	private void UpdateBossHPUI()
	{
        BossHPStatus status = _bossHPBar.GetBossHPUIValues();

        _barsLabel.Text = status.NumBars.ToString();
        _hp.MaxValue = status.MaxBarHP;
		_hp.Value = status.CurrentBarHP;
		
		StyleBoxFlat back = ResourceLoader.Load<StyleBoxFlat>(BACK);
		StyleBoxFlat front = ResourceLoader.Load<StyleBoxFlat>(FRONT);

		back.BgColor = status.BG;
		front.BgColor = status.FG;

		_hp.AddThemeStyleboxOverride("background", back);
		_hp.AddThemeStyleboxOverride("fill", front);

    }
}
