using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public class HealSkill : ISkill
    {
        public SkillId Id => SkillId.Healing;

        public string Name { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get ; set; }
        public int HealAmount { get; set; }

        public ISkill Clone()
        {
            return new HealSkill()
            {
                Name = this.Name,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                HealAmount = this.HealAmount
            };
        }

        public string GetBattleDisplayString()
        {
            return $"{this.Name} (+{this.HealAmount} HP)";
        }

        public BattleResult ProcessSkill(BattleEntity target)
        {
            return target.ApplyHealingSkill(this);
        }

        public override string ToString()
        {
            return this.GetBattleDisplayString();
        }
    }
}
