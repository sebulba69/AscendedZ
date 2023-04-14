using AscendedZ;
using AscendedZ.battle;
using AscendedZ.effects;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;

public partial class EntityDisplayBox : PanelContainer
{
    private TextureProgressBar _hp;
    private Sprite2D _effect;
    private AudioStreamPlayer _shakeSfx;
    private Vector2 _originalPosition;
    private ColorRect _activePlayerTag;
    private HBoxContainer _statuses;

    private string debugName;

    private float _x;

    // screen shake
    private ShakeParameters _shakeParameters;
    private RandomNumberGenerator _randomNumberGenerator;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("EffectPlayed");

        _shakeParameters = new ShakeParameters();
        _randomNumberGenerator = new RandomNumberGenerator();
        _randomNumberGenerator.Randomize();

        _hp = this.GetNode<TextureProgressBar>("ContainerForCharStuff/HP");
        _effect = this.GetNode<Sprite2D>("%EffectSprite");
        _shakeSfx = this.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _statuses = this.GetNode<HBoxContainer>("%Statuses");
    }

    /// <summary>
    /// We're going to use Process to reset the strength of our Screen Shake back to 0.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta)
    {
        if(_shakeParameters.ShakeValue > 0)
        {
            _shakeParameters.ShakeValue = (float)Mathf.Lerp((double)_shakeParameters.ShakeValue, 0, (double)_shakeParameters.ShakeDecay * delta);
            float x = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            float y = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            this.Position = new Vector2(_x + x, y);
        }
        else
        {
            _x = this.Position.X;
        }
    }

    public void InstanceEntity(EntityWrapper wrapper)
    {
        debugName = wrapper.BattleEntity.Name;

        Label name = this.GetNode<Label>("ContainerForCharStuff/CenterContainer/NameLabel");
        Label res = this.GetNode<Label>("ContainerForCharStuff/PanelContainer/CenterContainer/ResistanceLabel");
        TextureRect picture = this.GetNode<TextureRect>("ContainerForCharStuff/Picture");

        BattleEntity entity = wrapper.BattleEntity;

        _hp.MaxValue = entity.MaxHP;
        _hp.Value = _hp.MaxValue;

        name.Text = entity.Name;
        res.Text = entity.Resistances.GetResistanceString();
        picture.Texture = ResourceLoader.Load<Texture2D>(entity.Image);
    }

    public void UpdateEntityDisplay(EntityWrapper wrapper)
    {
        var battleEntity = wrapper.BattleEntity;


        // ... change hp status ... //
        // set HP values
        _hp.Value = battleEntity.HP;

        // ... change active status ... //
        // change active status if it's a player (players have the graphic, enemies don't)
        if (battleEntity.GetType().Equals(typeof(BattlePlayer)))
        {
            ColorRect activePlayerTag = this.GetNode<ColorRect>("%ActivePlayerTag");
            if (activePlayerTag.Visible != battleEntity.IsActive)
                activePlayerTag.Visible = battleEntity.IsActive;
        }


        // ... show statuses ... //
        // clear old statuses
        foreach (var child in _statuses.GetChildren())
            _statuses.RemoveChild(child);

        // place our new, updated statuses on scren
        var entityStatuses = battleEntity.StatusHandler.Statuses;
        foreach (var status in entityStatuses)
        {
            StatusIconWrapper statusIconWrapper = status.CreateIconWrapper();
            var statusIcon = ResourceLoader.Load<PackedScene>(Scenes.STATUS).Instantiate();
            _statuses.AddChild(statusIcon);

            statusIcon.Call("SetIcon", statusIconWrapper);
        }
    }

    public async void UpdateBattleEffects(BattleEffectWrapper effectWrapper)
    {
        BattleResult result = effectWrapper.Result;

        if(result.SkillUsed != null)
        {
            // if we're the skill user, we want to play the startup animation
            if (effectWrapper.IsEntitySkillUser)
            {
                string startupAnimationString = result.SkillUsed.StartupAnimation;
                if (!string.IsNullOrEmpty(startupAnimationString))
                {
                    // wait for play effect to finish before proceeding
                    await PlayEffect(startupAnimationString);
                }
            }
            else
            {
                string endupAnimationString = result.SkillUsed.EndupAnimation;
                bool isHPGainedFromMove = (result.ResultType == BattleResultType.HPGain || result.ResultType == BattleResultType.Dr);

                if (!string.IsNullOrEmpty(endupAnimationString))
                {
                    await PlayEffect(endupAnimationString);
                }

                if((int)(result.ResultType) < (int)BattleResultType.StatusApplied)
                {
                    // play damage sfx
                    if (!isHPGainedFromMove)
                    {
                        _shakeParameters.ShakeValue = _shakeParameters.ShakeStrength;
                        _shakeSfx.Play();
                    }
   
                    // play damage number
                    var dmgNumber = ResourceLoader.Load<PackedScene>(Scenes.DAMAGE_NUM).Instantiate();
                    dmgNumber.Call("SetDisplayInfo", result.HPChanged, isHPGainedFromMove, result.GetResultString());

                    CenterContainer effectContainer = this.GetNode<CenterContainer>("CenterContainer");
                    effectContainer.AddChild(dmgNumber);
                }
            }
        }

        // for some reason, we can't seem to emit the signal if we don't await at least once in this thread
        // this seems to be a Godot quirk, not sure why this is the case
        await Task.Delay(100);
        this.EmitSignal("EffectPlayed");
    }

    private async Task PlayEffect(string effectName)
    {
        _effect.Visible = true;

        _effect.Call("PlayAnimation", effectName);
        await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

        _effect.Visible = false;
    }
}
