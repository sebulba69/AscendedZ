using AscendedZ;
using AscendedZ.battle;
using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.dungeon_crawling.combat.combat_scenes;
using AscendedZ.effects;
using AscendedZ.entities;
using AscendedZ.statuses;
using Godot;
using System;
using System.Numerics;
using System.Threading.Tasks;
using static Godot.HttpRequest;
using Vector2 = Godot.Vector2;

public partial class PlayerScene : EntityScene
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("EffectPlayed");

        var hp = this.GetNode<ProgressBar>("%HP");
        var mp = this.GetNode<ProgressBar>("%MP");
        var hpl = this.GetNode<Label>("%HPAmount");
        var mpl = this.GetNode<Label>("%MPAmount");
        var playerPic = this.GetNode<TextureRect>("%PlayerPic");
        var statuses = this.GetNode<GridContainer>("%StatusGrid");

        var effect = this.GetNode<EffectAnimation>("%EffectSprite");
        var shakeSfx = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        ComposeUI(hp, mp, hpl, mpl, statuses, effect, shakeSfx, playerPic);
    }
}
