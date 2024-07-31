using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens
{
    public partial class Transitionable2DScene : Node2D
    {
        protected async void TransitionScenes(string endScene, AudioStreamPlayer audioStreamPlayer)
        {
            var destination = ResourceLoader.Load<PackedScene>(endScene).Instantiate();
            var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
            var root = this.GetTree().Root;

            root.AddChild(transition);

            if(audioStreamPlayer != null)
                this.GetTree().CreateTween().TweenProperty(audioStreamPlayer, "volume_db", -80, 0.5);

            transition.PlayFadeIn();

            await ToSignal(transition.Player, "animation_finished");

            this.Visible = false;

            root.AddChild(destination);
            transition.PlayFadeOut();
            await ToSignal(transition.Player, "animation_finished");
            transition.QueueFree();
            QueueFree();
        }
    }
}
