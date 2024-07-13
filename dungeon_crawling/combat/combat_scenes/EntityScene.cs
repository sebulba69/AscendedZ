using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.effects;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;
using Vector2 = Godot.Vector2;

namespace AscendedZ.dungeon_crawling.combat.combat_scenes
{
    public delegate void Shake();

    public partial class EntityScene : Control
    {
        public Shake Shake;

        private EffectAnimation _effect;
        private AudioStreamPlayer _shakeSfx;
        private ProgressBar _hp, _mp;
        private Label _hpl, _mpl;
        private HBoxContainer _statuses;
        private TextureRect _entityPicture;
        private CenterContainer _effectContainer;

        protected void ComposeUI(
            ProgressBar hp, Label hpLabel, HBoxContainer statusGrid,
            EffectAnimation effectAnimation, AudioStreamPlayer shakeSfx, 
            TextureRect entityPicture, CenterContainer effectContainer)
        {
            _hp = hp;
            _hpl = hpLabel;
            _statuses = statusGrid;
            _effect = effectAnimation;
            _shakeSfx = shakeSfx;
            _entityPicture = entityPicture;
            _effectContainer = effectContainer;
        }

        public virtual void InitializeEntityValues(GBEntity entity)
        {
            _hp.Value = entity.GetHPPercentage();
            _hpl.Text = entity.HP.ToString();
            _entityPicture.Texture = ResourceLoader.Load<Texture2D>(entity.Image);

            entity.PlayEffect += PlayEffect;
            entity.UpdateHP += UpdateHPDisplay;
            entity.PlayDamageNumberAnimation += PlayDamageNumberAnimation;
        }

        public void UpdateHPDisplay(int hpPercentage, long hp)
        {
            _hp.Value = hpPercentage;
            _hpl.Text = hp.ToString();
        }

        public async Task PlayEffect(string effectName)
        {
            _effect.Visible = true;
            _effect.PlayAnimation(effectName);

            await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

            _effect.Visible = false;
        }

        public void PlayDamageNumberAnimation(long damage, string resultString)
        {
            // play damage sfx
            Shake();
            _shakeSfx?.Play();

            // play damage number
            var dmgNumber = ResourceLoader.Load<PackedScene>(Scenes.DAMAGE_NUM).Instantiate<DamageNumber>();
            dmgNumber.SetDisplayInfo(new BigInteger(damage), false, resultString);

            _effectContainer.AddChild(dmgNumber);
        }
    }
}
