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

    /// <summary>
    /// Only call this if you are the player class.
    /// </summary>
    /// <param name="activeState"></param>
    public void SetActivePlayer(bool activeState)
    {
        ColorRect activePlayerTag = this.GetNode<ColorRect>("%ActivePlayerTag");
        if(activePlayerTag.Visible != activeState)
            activePlayerTag.Visible = activeState;
    }

    public void ShowStatuses(StatusWrapper statusWrapper)
    {
        foreach (var child in _statuses.GetChildren())
            _statuses.RemoveChild(child);

        foreach (var status in statusWrapper.Statuses)
        {
            StatusIconWrapper wrapper = status.CreateIconWrapper();
            var statusIcon = ResourceLoader.Load<PackedScene>(Scenes.STATUS).Instantiate();
            _statuses.AddChild(statusIcon);

            statusIcon.Call("SetIcon", wrapper);
        }
    }

    public async void PlayEffect(string effectName)
    {
        _effect.Visible = true;

        _effect.Call("PlayAnimation", effectName);
        await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

        _effect.Visible = false;
        this.EmitSignal("EffectPlayed");
    }

    public void PlayScreenShake()
    {
        _shakeParameters.ShakeValue = _shakeParameters.ShakeStrength;
        _shakeSfx.Play();
    }

    public void PlayDamageNumber(int amount, bool isHPGainedFromMove, string resultString)
    {
        var dmgNumber = ResourceLoader.Load<PackedScene>(Scenes.DAMAGE_NUM).Instantiate();
        dmgNumber.Call("SetDisplayInfo", amount, isHPGainedFromMove, resultString);

        CenterContainer effectContainer = this.GetNode<CenterContainer>("CenterContainer");
        effectContainer.AddChild(dmgNumber);
    }

    public void UpdateDisplay(int hp)
    {
        _hp.Value = hp;
    }
}
