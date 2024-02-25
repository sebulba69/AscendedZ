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
        private int _level = 0;
        private string _baseName;
        private string _name;
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
            return $"{this.BaseName} ({this.HealAmount}HP)";
        }

        public void LevelUp()
        {
            int boost = GetBoostValue();
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
            return $"{GetBattleDisplayString()} → {this.HealAmount + GetBoostValue()}";
        }

        public string GetAscendedString(int ascendedLevel)
        {
            HealSkill newSkill = (HealSkill)SkillDatabase.GetNextTierOfHealSkill(this.Tier, this.TargetType).Clone();

            if (Tier >= 5)
                newSkill.HealAmount += (ascendedLevel * 2);

            return $"{ToString()} → {newSkill.ToString()}";
        }

        private int GetBoostValue()
        {
            try
            {
                double m = 0.01 * Math.Pow((_level - 10), 2) + 1;
                int boost = (int)(Math.Pow(_level, m)) + 1;
                boost = (boost / 4) + 1;
                return boost;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public BattleResult ProcessSkill(BattleEntity target)
        {
            return target.ApplyHealingSkill(this);
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
