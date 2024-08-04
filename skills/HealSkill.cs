using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public class HealSkill : ISkill
    {
        public SkillId Id => SkillId.Healing;
        private int _level = 0;
        private string _baseName;
        private int _tier = 1;

        public string Name
        {
            get
            {
                if (_level == 0)
                    return _baseName;
                else
                    if (_level < int.MaxValue - 1)
                    return $"{_baseName} +{_level}";
                else
                    return $"{_baseName} +MAX";
            }
        }

        public string BaseName { get => _baseName; set => _baseName = value; }
        public int Level { get => _level; set => _level = value; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get ; set; }
        public int HealAmount { get; set; }
        public int Tier { get => _tier; set => _tier = value; }

        public string GetBattleDisplayString()
        {
            return $"{this.Name} ({this.HealAmount}HP)";
        }

        public void LevelUp()
        {
            int boost = (Level + 1) * 2;
            
            try
            {
                _level++;
            }
            catch (Exception)
            {
                _level = int.MaxValue - 1;
            }

            try
            {
                this.HealAmount += boost;
            }
            catch (Exception)
            {
                this.HealAmount = int.MaxValue - 1;
            }
        }

        public string GetUpgradeString()
        {
            return $"{GetBattleDisplayString()} → {this.HealAmount + (Level + 1) * 2}";
        }

        public BattleResult ProcessSkill(BattleEntity user, BattleEntity target)
        {
            return target.ApplyHealingSkill(this);
        }

        public BattleResult ProcessSkill(BattleEntity user, List<BattleEntity> targets)
        {
            BattleResult all = targets[0].ApplyHealingSkill(this);
            all.Target = null;
            all.Results.Add(all.ResultType);
            all.AllHPChanged.Add(all.HPChanged);
            all.Targets.Add(targets[0]);

            for(int i = 1; i < targets.Count; i++)
            {
                BattleResult result = targets[i].ApplyHealingSkill(this);
                all.AllHPChanged.Add(result.HPChanged);
                all.Targets.Add(targets[i]);
                all.Results.Add(result.ResultType);
            }

            return all;
        }

        public override string ToString()
        {
            return this.GetBattleDisplayString();
        }

        public ISkill Clone()
        {
            return new HealSkill()
            {
                BaseName = this.BaseName,
                Level = Level,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                HealAmount = this.HealAmount,
                Tier = this.Tier
            };
        }
    }
}
