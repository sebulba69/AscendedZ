using AscendedZ;
using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class DC_BOSS_CUTSCENE : Control
{
	private TextureRect _bossPic;
	private AscendedTextbox _bossTextBox;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_bossPic = GetNode<TextureRect>("%BossPic");
		_bossTextBox = GetNode<AscendedTextbox>("%AscendedTextbox");
	}

	public async void Start(string bossName, string bossImage)
	{
		_bossPic.Texture = ResourceLoader.Load<Texture2D>(bossImage);
		GD.Print(bossName + ", " + bossImage);
		string[] dialog = DialogScenes.DCBossDialog[bossName];
        foreach(var text in dialog)
		{
            _bossTextBox.DisplayText(text);
            await ToSignal(_bossTextBox, "ReadyForMoreDialogEventHandler");
        }

		QueueFree();
	}
}
