using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public class StatusSkill : ISkill
    {
        public SkillId Id => SkillId.Status;
        public string BaseName { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public Status Status { get; set; }
        public string Name => BaseName;

        public StatusSkill()
        {
            Level = 1;
        }

        public BattleResult ProcessSkill(BattleEntity target)
        {
            BattleResult result = new BattleResult() 
            {
                SkillUsed = this,
                Target = target,
                ResultType = BattleResultType.StatusApplied
            };

            target.StatusHandler.AddStatus(target, this.Status);

            result.Log.Append($"[color=gold]{target.Name}[/color] now has the status: [color=yellow]{Status.Name}[/color] ● {this.Status.CreateIconWrapper().Description}");
            return result;
        }

        public string GetBattleDisplayString()
        {
            return $"{this.BaseName}";
        }

        public void LevelUp()
        {
        }

        public override string ToString()
        {
            return GetBattleDisplayString();
        }

        public string GetUpgradeString()
        {
            return ToString();
        }

        public ISkill Clone()
        {
            return new StatusSkill()
            {
                BaseName = this.BaseName,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                Status = this.Status.Clone()
            };
        }
    }
}
