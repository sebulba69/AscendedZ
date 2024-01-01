using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public class RetreatSkill : ISkill
    {
        public SkillId Id => SkillId.Retreat;

        public string Name { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }

        public BattleResult ProcessSkill(BattleEntity target)
        {
            return new BattleResult() { ResultType = BattleResultType.Retreat, SkillUsed = this };
        }

        public string GetBattleDisplayString()
        {
            return this.Name;
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }

        public void LevelUp()
        {
        }

        public string GetUpgradeString()
        {
            return ToString();
        }

        public ISkill Clone()
        {
            return new RetreatSkill()
            {
                Name = this.Name,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon
            };
        }

    }
}
