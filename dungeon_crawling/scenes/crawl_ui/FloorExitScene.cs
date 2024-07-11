using Godot;
using System;

public partial class FloorExitScene : CenterContainer
{
	private Button _backToHomeBtn, _retryFloorBtn, _changePartyBtn, _stayBtn, _continueBtn;
	private Label _endOfBattleLabel;

	public Label EndOfBattleLabel { get => _endOfBattleLabel; }
	public Button Back { get => _backToHomeBtn; }
	public Button Retry { get => _retryFloorBtn; }
	public Button Party { get => _changePartyBtn; }
	public Button Continue { get => _continueBtn; }
	public Button Stay { get => _stayBtn; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_backToHomeBtn = this.GetNode<Button>("%BackToHomeBtn");
		_retryFloorBtn = this.GetNode<Button>("%RetryFloorBtn");
        _changePartyBtn = this.GetNode<Button>("%ChangePartyBtn");
        _stayBtn = this.GetNode<Button>("%StayOnFloor");
        _continueBtn = this.GetNode<Button>("%ContinueBtn");

		_endOfBattleLabel = this.GetNode<Label>("%EndOfBattleLabel");
    }
}
