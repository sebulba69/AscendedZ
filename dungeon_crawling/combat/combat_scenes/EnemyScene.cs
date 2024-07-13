using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.dungeon_crawling.combat.combat_scenes;
using AscendedZ.effects;
using Godot;
using System;

public partial class EnemyScene : EntityScene
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("EffectPlayed");

        var hp = this.GetNode<ProgressBar>("%HP");
        var hpl = this.GetNode<Label>("%HPAmount");
        var enemyPic = this.GetNode<TextureRect>("%EnemyPic");
        var statuses = this.GetNode<HBoxContainer>("%StatusGrid");

        var effect = this.GetNode<EffectAnimation>("%EffectSprite");
        var shakeSfx = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        var effectContainer = this.GetNode<CenterContainer>("%EffectContainer");
        ComposeUI(hp, hpl, statuses, effect, shakeSfx, enemyPic, effectContainer);
    }
}
