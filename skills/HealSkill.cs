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
        private int _level = 0;

        public SkillId Id => SkillId.Healing;

        public string Name { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get ; set; }
        public int HealAmount { get; set; }

        public string GetBattleDisplayString()
        {
            return $"{this.Name} ({this.HealAmount}HP)";
        }

        public void LevelUp()
        {
            int boost = GetBoostValue();
            _level++;

            this.Name = $"{this.Name} +{_level}";
            this.HealAmount += boost;
        }

        public void LevelUpEnemy(int level, int boost)
        {
        }

        public string GetUpgradeString()
        {
            return $"{GetBattleDisplayString} +{GetBoostValue()}";
        }

        private int GetBoostValue()
        {
            double m = 0.01 * Math.Pow((_level - 10), 2) + 1;
            int boost = (int)(Math.Pow(_level, m)) + 1;
            boost = (boost / 4) + 1;
            return boost;
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
                Name = this.Name,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                HealAmount = this.HealAmount
            };
        }
    }
}
