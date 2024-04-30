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
        var mp = this.GetNode<ProgressBar>("%MP");
        var hpl = this.GetNode<Label>("%HPAmount");
        var mpl = this.GetNode<Label>("%MPAmount");
        var enemyPic = this.GetNode<TextureRect>("%EnemyPic");
        var statuses = this.GetNode<GridContainer>("%StatusGrid");

        var effect = this.GetNode<EffectAnimation>("%EffectSprite");
        var shakeSfx = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        
        ComposeUI(hp, mp, hpl, mpl, statuses, effect, shakeSfx, enemyPic);
    }
}
