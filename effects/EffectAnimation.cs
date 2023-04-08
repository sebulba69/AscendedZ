using Godot;
using System;

public partial class EffectAnimation : Sprite2D
{
	private AnimationPlayer _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddUserSignal("EffectAnimationCompletedEventHandler");
		_player = this.GetNode<AnimationPlayer>("AnimationPlayer");
		_player.AnimationFinished += _OnAnimationCompleted;
	}

	public void PlayAnimation(string animationName)
	{
		_player.Play(animationName);
	}

    private void _OnAnimationCompleted(StringName animationName)
    {
		this.EmitSignal("EffectAnimationCompletedEventHandler");
    }
}
