using AscendedZ;
using AscendedZ.battle;
using AscendedZ.effects;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using Godot;
using System.Threading.Tasks;


public partial class EntityDisplayBox : PanelContainer
{
    private readonly PackedScene _damageNumScene = ResourceLoader.Load<PackedScene>(Scenes.DAMAGE_NUM);
    private readonly PackedScene _statusScene = ResourceLoader.Load<PackedScene>(Scenes.STATUS);

    private Sprite2D _effect;
    private AudioStreamPlayer _shakeSfx;
    private Vector2 _originalPosition;
    private HBoxContainer _statuses;
    private Label _resistances;
    private Label _hp;
    private float _x;

    private Texture2D _entityImage;
    private Texture2D _deadImage;

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

        _effect = this.GetNode<Sprite2D>("%EffectSprite");
        _shakeSfx = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        _statuses = this.GetNode<HBoxContainer>("%Statuses");
        _resistances = this.GetNode<Label>("%ResistanceLabel");
        _hp = this.GetNode<Label>("%HPLabel");
        _x = -1;
        _originalPosition = new Vector2(this.Position.X, 0);

        _deadImage = ResourceLoader.Load<Texture2D>("res://entity_pics/dead_entity.png");
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
            this.Position = new Vector2(_x, _originalPosition.Y);
        }
    }

    public void InstanceEntity(EntityWrapper wrapper, bool random=false)
    {
        Label name = this.GetNode<Label>("%NameLabel");
        TextureRect picture = this.GetNode<TextureRect>("%Picture");

        BattleEntity entity = wrapper.BattleEntity;

        if (wrapper.IsBoss)
        {
            var bossHP = this.GetNode<BossHPBar>("%HP");
            bossHP.InitializeBossHPBar(entity.HP);
        }
        else
        {
            var hp = this.GetNode<TextureProgressBar>("%HP");
            hp.MaxValue = entity.MaxHP;
            hp.Value = hp.MaxValue;
        }

        name.Text = entity.Name;
        _hp.Text = $"{entity.HP}/{entity.MaxHP} HP";
        _resistances.Text = entity.Resistances.GetResistanceString();

        _entityImage = ResourceLoader.Load<Texture2D>(entity.Image);
        picture.Texture = _entityImage;

        if (random)
        {
            var randomized = new RandomNumberGenerator();
            randomized.Randomize();
            float hue = randomized.Randf();
            float saturation = randomized.Randf();
            float value = picture.SelfModulate.V;

            picture.SelfModulate = Color.FromHsv(hue, saturation, value);
        }
    }

    public void UpdateEntityDisplay(EntityWrapper wrapper)
    {
        var entity = wrapper.BattleEntity;

        // ... change hp status ... //
        // set HP values
        if (wrapper.IsBoss)
        {
            var bossHP = this.GetNode<BossHPBar>("%HP");
            bossHP.UpdateBossHP(entity.HP);
        }
        else
        {
            var hp = this.GetNode<TextureProgressBar>("%HP");
            hp.Value = entity.HP;
            TextureRect picture = this.GetNode<TextureRect>("%Picture");
            if (entity.HP == 0)
            {
                picture.Texture = _deadImage;
            }
            else
            {
                if(picture.Texture != _entityImage)
                {
                    picture.Texture = _entityImage;
                }
                    
            }

        }
        _resistances.Text = entity.Resistances.GetResistanceString();

        // ... change active status ... //
        // change active status if it's a player (players have the graphic, enemies don't)
        if (entity.GetType().Equals(typeof(BattlePlayer)))
        {
            
            TextureRect activePlayerTag = this.GetNode<TextureRect>("%ActivePlayerTag");
            CenterContainer spacer = this.GetNode<CenterContainer>("%Spacer");
            if (activePlayerTag.Visible != entity.IsActiveEntity)
            {
                activePlayerTag.Visible = entity.IsActiveEntity;
                spacer.Visible = entity.IsActiveEntity;
            }
                
        }

        this.GetNode<Label>("%NameLabel").Text = entity.Name;
        _hp.Text = $"{entity.HP}/{entity.MaxHP} HP";

        // ... show statuses ... //
        // clear old statuses
        foreach (var child in _statuses.GetChildren())
        {
            _statuses.RemoveChild(child);
            child.QueueFree();
        }

        // place our new, updated statuses on scren
        var entityStatuses = entity.StatusHandler.Statuses;

        foreach (var status in entityStatuses)
        {
            StatusIconWrapper statusIconWrapper = status.CreateIconWrapper();
            var statusIcon = _statusScene.Instantiate();
            _statuses.AddChild(statusIcon);

            statusIcon.Call("SetIcon", statusIconWrapper);
        }
    }

    public void SetDescription(string description)
    {
        GetNode<TextureRect>("%Picture").TooltipText = description;
    }

    public async Task UpdateBattleEffects(BattleEffectWrapper effectWrapper)
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
                    if (!isHPGainedFromMove && result.HPChanged > 0)
                    {
                        _shakeParameters.ShakeValue = _shakeParameters.ShakeStrength;
                        _shakeSfx.Play();
                    }
   
                    // play damage number
                    var dmgNumber = _damageNumScene.Instantiate<DamageNumber>();
                    dmgNumber.SetDisplayInfo(result.HPChanged, isHPGainedFromMove, result.GetResultString());

                    CenterContainer effectContainer = this.GetNode<CenterContainer>("%EffectContainer");
                    CallDeferred("add_child", dmgNumber);
                }
            }
        }
    }

    private async Task PlayEffect(string effectName)
    {
        _effect.Visible = true;
        _effect.Call("PlayAnimation", effectName);
        await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

        _effect.Visible = false;
    }
}
