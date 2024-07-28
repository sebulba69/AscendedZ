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
        private bool _isRemoveStatusSkill = false;

        public SkillId Id => SkillId.Status;
        public string BaseName { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public Status Status { get; set; }
        public string Name => BaseName;
        public bool IsRemoveStatusSkill { get => _isRemoveStatusSkill; set => _isRemoveStatusSkill = value; }

        public StatusSkill()
        {
            Level = 1;
        }

        public BattleResult ProcessSkill(BattleEntity user, BattleEntity target)
        {
            BattleResult result = new BattleResult() 
            {
                SkillUsed = this,
                Target = target,
                ResultType = (!IsRemoveStatusSkill) ? BattleResultType.StatusApplied : BattleResultType.StatusRemoved
            };

            if (!IsRemoveStatusSkill)
            {
                target.StatusHandler.AddStatus(target, this.Status);
            }
            else 
            {
                target.StatusHandler.RemoveStatus(target, this.Status.Id);
            }
                
            return result;
        }

        public BattleResult ProcessSkill(BattleEntity user, List<BattleEntity> targets)
        {
            BattleResult all = ProcessSkill(user, targets[0]);
            
            all.Target = null;
            all.Results.Add(all.ResultType);

            for(int i = 1; i < targets.Count; i++)
            {
                BattleResult r = ProcessSkill(user, targets[i]);
                all.Results.Add(r.ResultType);
            }

            all.Targets.AddRange(targets);
            return all;
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
            return $"{GetBattleDisplayString()} [NO UPGRADE]";
        }

        public string GetAscendedString(int ascendedLevel)
        {
            return GetBattleDisplayString();
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
                Status = this.Status.Clone(),
                IsRemoveStatusSkill = this.IsRemoveStatusSkill
            };
        }
    }
}
