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
    private ShakeParameters _shakeParameters = new ShakeParameters();
    private Sprite2D _currentBlinker;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("EffectPlayed");

        var hp = this.GetNode<ProgressBar>("%HP");
        var hpl = this.GetNode<Label>("%HPAmount");
        var playerPic = this.GetNode<TextureRect>("%PlayerPic");
        var statuses = this.GetNode<HBoxContainer>("%StatusGrid");

        var effect = this.GetNode<EffectAnimation>("%EffectSprite");
        var shakeSfx = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        var effectContainer = this.GetNode<CenterContainer>("%EffectContainer");
        ComposeUI(hp, hpl, statuses, effect, shakeSfx, playerPic, effectContainer);
    }

    public override void InitializeEntityValues(GBEntity entity)
    {
        entity.SetCurrent += SetCurrent;
        base.InitializeEntityValues(entity);
    }

    public void SetCurrent(bool isCurrent)
    {
        this.GetNode<Sprite2D>("%CurrentBlinker").Visible = isCurrent;
    }
}
