using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.effects;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Godot.Vector2;

namespace AscendedZ.dungeon_crawling.combat.combat_scenes
{
    public partial class EntityScene : Control
    {
        private EffectAnimation _effect;
        private AudioStreamPlayer _shakeSfx;
        private Vector2 _originalPosition;
        private float _x;
        private ProgressBar _hp, _mp;
        private Label _hpl, _mpl;
        private GridContainer _statuses;
        private ShakeParameters _shakeParameters;
        private RandomNumberGenerator _randomNumberGenerator;
        private TextureRect _entityPicture;

        protected void ComposeUI(
            ProgressBar hp, ProgressBar mp, 
            Label hpLabel, Label mpLabel, 
            GridContainer statusGrid, EffectAnimation effectAnimation,
            AudioStreamPlayer shakeSfx, TextureRect entityPicture)
        {
            _shakeParameters = new ShakeParameters();
            _randomNumberGenerator = new RandomNumberGenerator();
            _randomNumberGenerator.Randomize();
            _hp = hp;
            _mp = mp;
            _hpl = hpLabel;
            _mpl = mpLabel;
            _statuses = statusGrid;
            _effect = effectAnimation;
            _shakeSfx = shakeSfx;
            _entityPicture = entityPicture;
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

        public void InitializeEntityValues(BigInteger hp, BigInteger mp, string pic)
        {
            _hp.Value = 100;
            _mp.Value = 100;

            _hpl.Text = hp.ToString();
            _mpl.Text = mp.ToString();
            _entityPicture.Texture = ResourceLoader.Load<Texture2D>(pic);
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
}
