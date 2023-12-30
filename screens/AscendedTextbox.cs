using Godot;
using System;

public partial class AscendedTextbox : VBoxContainer
{
    [Signal]
    public delegate void SkipDialogEventHandler();

    /// <summary>
    /// Time between letters showing up in the text.
    /// </summary>
    private const float TIMEOUT = 0.03f;

    /// <summary>
    /// Timer for text display.
    /// </summary>
    private Timer _timer;

    /// <summary>
    /// Textbox for displaying text.
    /// </summary>
    private Label _textbox;

    /// <summary>
    /// Flag preventing you from clicking next if text is displaying.
    /// </summary>
    private bool _canClickNextButton = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("ReadyForMoreDialogEventHandler");
        this.AddUserSignal("SkipDialogEventHandler");

        _timer = GetNode<Timer>("TextTimer");
        _textbox = GetNode<Label>("%TextboxForDialog");

        Button nextButton = GetNode<Button>("MarginContainer/TextControlButtons/NextButton");
        Button ffButton = GetNode<Button>("MarginContainer/TextControlButtons/FastForwardButton");
        Button skipButton = GetNode<Button>("MarginContainer/TextControlButtons/SkipButton");

        nextButton.Connect("pressed",new Callable(this,"_OnNextButtonPressed"));
        ffButton.Connect("pressed",new Callable(this,"_OnFFButtonPressed"));
        skipButton.Connect("pressed",new Callable(this,"_OnSkipButtonPressed"));
    }

    /// <summary>
    /// Display all text in the textbox 1 character at a time.
    /// </summary>
    /// <param name="dialog"></param>
    /// <param name="callbackScene"></param>
    public async void DisplayText(string dialog)
    {
        _textbox.VisibleCharacters = 0;
        _textbox.Text = dialog;

        _canClickNextButton = false;

        int charIndex = 0;

        while(charIndex < _textbox.Text.Length && _textbox.VisibleCharacters < _textbox.Text.Length)
        {
            _textbox.VisibleCharacters++;
            charIndex++;
            _timer.Start(TIMEOUT);
            await ToSignal(_timer, "timeout");
        }

        _canClickNextButton = true;
    }

    /// <summary>
    /// Let parent classes known we're ready to receive more dialog.
    /// </summary>
    private void _OnNextButtonPressed()
    {
        if (_canClickNextButton)
        {
            this.EmitSignal("ReadyForMoreDialogEventHandler");
        }
    }

    private void _OnFFButtonPressed()
    {
        _textbox.VisibleCharacters = _textbox.Text.Length;
        _canClickNextButton = true;
    }

    private void _OnSkipButtonPressed()
    {
        _textbox.VisibleCharacters = _textbox.Text.Length;
        this.EmitSignal("SkipDialogEventHandler");
    }
}
