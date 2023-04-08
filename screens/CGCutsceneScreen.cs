using Godot;
using System;

public partial class CGCutsceneScreen : Node2D
{
    private AscendedTextbox _ascendedTextbox;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddUserSignal("CutsceneEndedEventHandler");

        _ascendedTextbox = this.GetNode<AscendedTextbox>("CGBackground/AscendedTextbox");
        _ascendedTextbox.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogRequested"));
    }

    public async void StartCutscene(string[] dialog)
    {
        foreach(string text in dialog)
        {
            _ascendedTextbox.DisplayText(text);
            await ToSignal(_ascendedTextbox, "ReadyForMoreDialogEventHandler");
        }

        this.EmitSignal("CutsceneEndedEventHandler");
        this.QueueFree();
    }

    /// <summary>
    /// Automatically end the cutscene
    /// </summary>
    private void _OnSkipDialogRequested()
    {
        this.EmitSignal("CutsceneEndedEventHandler");
        this.QueueFree();
    }
}
