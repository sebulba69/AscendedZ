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
        public int HealPercentage { get; set; }

        public string GetBattleDisplayString()
        {
            return $"{this.Name} (+{this.HealPercentage}% HP)";
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
