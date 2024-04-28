using AscendedZ;
using AscendedZ.battle;
using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.effects;
using AscendedZ.entities;
using AscendedZ.statuses;
using Godot;
using System;
using System.Numerics;
using System.Threading.Tasks;
using static Godot.HttpRequest;
using Vector2 = Godot.Vector2;

public partial class PlayerScene : Control
{

    private EffectAnimation _effect;
    private AudioStreamPlayer _shakeSfx;
    private Vector2 _originalPosition;
    private Label _resistances;
    private float _x;

    private ProgressBar _hp, _mp;
	private Label _hpl, _mpl;

    private GridContainer _statuses;

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

        _hp = this.GetNode<ProgressBar>("%HP");
		_mp = this.GetNode<ProgressBar>("%MP");
        _hpl = this.GetNode<Label>("%HPAmount");
        _mpl = this.GetNode<Label>("%MPAmount");

        _statuses = this.GetNode<GridContainer>("%StatusGrid");

        _effect = this.GetNode<EffectAnimation>("%EffectSprite");
        _shakeSfx = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        _x = -1;
        _originalPosition = new Vector2(this.Position.X, 0);
    }

    /// <summary>
    /// We're going to use Process to reset the strength of our Screen Shake back to 0.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta)
    {
        if (_shakeParameters.ShakeValue > 0)
        {
            _shakeParameters.ShakeValue = (float)Mathf.Lerp((double)_shakeParameters.ShakeValue, 0, (double)_shakeParameters.ShakeDecay * delta);
            float x = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            float y = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            this.Position = new Vector2(_x + x, y);
        }
        else
        {
            _x = this.Position.X;
            this.Position = new Vector2(_x, _originalPosition.Y);
        }
    }

    public void Initialize(BigInteger hp, BigInteger mp, string picture)
    {
        _hp.Value = 100;
        _mp.Value = 100;

        _hpl.Text = hp.ToString();
        _mpl.Text = mp.ToString();

        var playerPic = this.GetNode<TextureRect>("%PlayerPic");
        playerPic.Texture = ResourceLoader.Load<Texture2D>(picture);
    }

    public void UpdateDisplay(BDCUpdateWrapper wrapper)
    {
        _hp.Value = (double)wrapper.HPPercentage;
        _mp.Value = (double)wrapper.MPPercentage;

        _hpl.Text = wrapper.HP.ToString();
        _mpl.Text = wrapper.MP.ToString();

        // ... show statuses ... //
        // clear old statuses
        foreach (var child in _statuses.GetChildren())
        {
            _statuses.RemoveChild(child);
            child.QueueFree();
        }

        /* To be filled out later
        // place our new, updated statuses on scren
        var entityStatuses = wrapper.Statuses;

        foreach (var status in entityStatuses)
        {
            StatusIconWrapper statusIconWrapper = status.CreateIconWrapper();
            var statusIcon = ResourceLoader.Load<StatusIcon>(Scenes.STATUS);
            _statuses.AddChild(statusIcon);
            statusIcon.SetIcon(statusIconWrapper);
        }*/
    }

    public async void UpdateBattleEffects(BDCEffectWrapper bDCEffectWrapper)
    {
        string startupAnimationString = bDCEffectWrapper.Skill.StartupAnimation;
        string endupAnimationString = bDCEffectWrapper.Skill.EndupAnimation;

        if (!string.IsNullOrEmpty(startupAnimationString))
        {
            // wait for play effect to finish before proceeding
            await PlayEffect(startupAnimationString);
        }

        // play damage number
        var dmgNumber = ResourceLoader.Load<DamageNumber>(Scenes.DAMAGE_NUM);

        dmgNumber.SetDisplayInfo(bDCEffectWrapper.HPChanged, bDCEffectWrapper.HPChanged != 0, bDCEffectWrapper.Result);

        CenterContainer effectContainer = this.GetNode<CenterContainer>("%EffectContainer");
        effectContainer.AddChild(dmgNumber);

        // for some reason, we can't seem to emit the signal if we don't await at least once in this thread
        // this seems to be a Godot quirk, not sure why this is the case
        await Task.Delay(100);
        this.EmitSignal("EffectPlayed");
    }

    private async Task PlayEffect(string effectName)
    {
        _effect.Visible = true;
        _effect.PlayAnimation(effectName);
        
        await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

        _effect.Visible = false;
    }
}
