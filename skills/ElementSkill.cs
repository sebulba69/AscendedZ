using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public partial class ElementSkill : ISkill 
    {
        private int _damage;
        private int _damageModifier = 0;

        public SkillId Id => SkillId.Elemental;
        public string Name { get; set; }
        public TargetTypes TargetType { get; set; }
        public Elements Element { get; set; }

        public int DamageModifier { private get => _damageModifier; set => _damageModifier = value; }

        public int Damage
        {
            get
            {
                return _damage + DamageModifier;
            }
            set
            {
                _damage = value;
            }
        }

        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }

        public BattleResult ProcessSkill(BattleEntity target)
        {
            return target.ApplyElementSkill(this);
        }

        public string GetBattleDisplayString()
        {
            return $"{this.Name} ({this.Damage})";
        }

        public override string ToString()
        {
            return $"[{this.Element.ToString()}] {this.Name} ({this.Damage})";
        }

        public ISkill Clone()
        {
            return new ElementSkill()
            {
                Name = this.Name,
                Damage = this.Damage,
                TargetType = this.TargetType,
                Element = this.Element,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon
            };
        }
    }
}
