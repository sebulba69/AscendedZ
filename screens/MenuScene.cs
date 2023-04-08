using AscendedZ;
using Godot;
using System;

public partial class MenuScene : CenterContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.AddUserSignal("EndMenuScene", new Godot.Collections.Array() 
		{
			new Godot.Collections.Dictionary
			{
				{ "name", "isQuitToTitle" },
				{ "type", (int)Godot.Variant.Type.Bool }
			}
		});

		Button saveButton = this.GetNode<Button>("VBoxContainer/SaveButton");
		Button quitButton = this.GetNode<Button>("VBoxContainer/QuitButton");
		Button backButton = this.GetNode<Button>("VBoxContainer/BackButton");

		Label tooltip = this.GetNode<Label>("%Tooltip");

		saveButton.Pressed += () => 
		{
			PersistentGameObjects.Save();
			tooltip.Text = "Game saved!";

        };

		quitButton.Pressed += _OnQuitButtonPressed;


        backButton.Pressed += () => 
		{
            this.EmitSignal("EndMenuScene", false);
			this.QueueFree();
        };
	}

	private void _OnQuitButtonPressed()
	{
		this.Visible = false;

		var popupWindow = ResourceLoader.Load<PackedScene>(Scenes.YES_NO_POPUP).Instantiate();
        this.GetTree().Root.AddChild(popupWindow);
        popupWindow.Call("SetDialogMessage", "Are you sure you want to save and quit to title?");
		popupWindow.Connect("AnswerSelected", new Callable(this, "_OnDialogResultReceived"));
    }

	private void _OnDialogResultReceived(bool isYesButtonPressed)
	{
		if (isYesButtonPressed)
		{
            PersistentGameObjects.Save();
            this.GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>(Scenes.START).Instantiate());
            this.EmitSignal("EndMenuScene", true);
        }

        this.Visible = true;
    }
}
