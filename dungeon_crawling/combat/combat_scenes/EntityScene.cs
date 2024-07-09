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
            ProgressBar hp, Label hpLabel, GridContainer statusGrid,
            EffectAnimation effectAnimation, AudioStreamPlayer shakeSfx, TextureRect entityPicture)
        {
            _shakeParameters = new ShakeParameters();
            _randomNumberGenerator = new RandomNumberGenerator();
            _randomNumberGenerator.Randomize();
            _hp = hp;
            _hpl = hpLabel;
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

        public void InitializeEntityValues(long hp, string pic)
        {
            _hp.Value = 100;
            _hpl.Text = hp.ToString();
            _entityPicture.Texture = ResourceLoader.Load<Texture2D>(pic);
        }

        public void UpdateDisplay(BDCUpdateWrapper wrapper)
        {
            _hp.Value = (double)wrapper.HPPercentage;
            _hpl.Text = wrapper.HP.ToString();

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

        private async Task PlayEffect(string effectName)
        {
            _effect.Visible = true;
            _effect.PlayAnimation(effectName);

            await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

            _effect.Visible = false;
        }
    }
}
