using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public class PassSkill : ISkill
    {
        public SkillId Id => SkillId.Pass;

        public string BaseName { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }

        public string Name => BaseName;

        public BattleResult ProcessSkill(BattleEntity target)
        {
            return new BattleResult() { ResultType = BattleResultType.Pass, SkillUsed = this };
        }

        public string GetBattleDisplayString()
        {
            return this.BaseName;
        }

        public override string ToString()
        {
            return $"{this.BaseName}";
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
            return new PassSkill()
            {
                BaseName = this.BaseName,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = SkillAssets.PASS_ICON
            };
        }

    }
}
