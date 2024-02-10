﻿using AscendedZ.battle;
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

        public string Name
        {
            get
            {
                if(_level == 0)
                    return _baseName;
                else
                    return $"{_baseName} +{_level}";
            }
        }
        public string BaseName { get => _baseName; set => _baseName = value; }
        public int Level { get => _level; set => _level = value; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get ; set; }
        public int HealAmount { get; set; }

        public string GetBattleDisplayString()
        {
            return $"{this.BaseName} ({this.HealAmount}HP)";
        }

        public void LevelUp()
        {
            int boost = GetBoostValue();

            _level++;
            this.HealAmount += boost;
        }

        public string GetUpgradeString()
        {
            return $"{GetBattleDisplayString()} → {this.HealAmount + GetBoostValue()}";
        }

        private int GetBoostValue()
        {
            double m = 0.01 * Math.Pow((_level - 10), 2) + 1;
            int boost = (int)(Math.Pow(_level, m)) + 1;
            boost = (boost / 4) + 3;
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
                BaseName = this.BaseName,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                HealAmount = this.HealAmount
            };
        }
    }
}
