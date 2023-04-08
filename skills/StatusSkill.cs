using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public class StatusSkill : ISkill
    {
        public SkillId Id => SkillId.Status;
        public string Name { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }

        public Status Status { get; set; }

        public BattleResult ProcessSkill(BattleEntity target)
        {
            BattleResult result = new BattleResult() 
            {
                SkillUsed = this,
                Target = target,
                ResultType = BattleResultType.StatusApplied
            };

            target.StatusHandler.AddStatus(this.Status);

            return result;
        }

        public string GetBattleDisplayString()
        {
            return $"{this.Name}";
        }
    }
}
