using Godot;
using System;

public partial class AscendedYesNoWindow : CenterContainer
{
	private Label _popupMessage;
	private Button _yesButton;
	private Button _noButton;
	private bool _buttonPressed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_buttonPressed = false;

		this.AddUserSignal("AnswerSelected", new Godot.Collections.Array() 
		{
			new Godot.Collections.Dictionary()
			{
				{ "name", "yesSelectedAsAnswer" },
				{ "type", (int)Godot.Variant.Type.Bool }
			}
		});

		_popupMessage = this.GetNode<Label>("%PopupMessage");
		_yesButton = this.GetNode<Button>("%YesButton");
        _noButton = this.GetNode<Button>("%NoButton");

		_yesButton.Pressed += () => ButtonPressed(true);
		_noButton.Pressed += () => ButtonPressed(false);
	}

	public void SetDialogMessage(string message)
	{
		_popupMessage.Text = message;
	}

	private void ButtonPressed(bool isYesButton)
	{
		_buttonPressed = true;

		this.EmitSignal("AnswerSelected", isYesButton);
		this.QueueFree();
	}
}
