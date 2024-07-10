using Godot;
using System;

public partial class GBSkillList : PanelContainer
{
	private Button _attackButton;

	public EventHandler DoPlayerAttack;

	public override void _Ready()
	{
		_attackButton = this.GetNode<Button>("%AttackButton");
		_attackButton.Pressed += _OnAttackButtonPressed;
	}

    public void SetUIForPlayerTurn()
	{
		_attackButton.Disabled = false;
	}

	private void _OnAttackButtonPressed()
	{
		_attackButton.Disabled = true;
		DoPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
}
