using Godot;
using System;

public partial class SigilScreen : Control
{
	private VBoxContainer _sigilVBox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sigilVBox = this.GetNode<VBoxContainer>("%SigilVBox");   
    }
}
