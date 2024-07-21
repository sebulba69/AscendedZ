﻿using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public class EyeSkill : ISkill
    {
        public SkillId Id => SkillId.Eye;
        public string BaseName { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public string Name => BaseName;
        public BattleResultType EyeType { get; set; }

        public EyeSkill()
        {
            Level = 1;
        }

        public BattleResult ProcessSkill(BattleEntity target)
        {
            BattleResult result = new BattleResult()
            {
                SkillUsed = this,
                Target = null,
                ResultType = EyeType
            };

            return result;
        }

        public BattleResult ProcessSkill(List<BattleEntity> targets)
        {
            throw new NotImplementedException("This should not be used as an all hit skill ever.");
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
            return new EyeSkill()
            {
                BaseName = this.BaseName,
                TargetType = this.TargetType,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                EyeType = EyeType
            };
        }
    }
}
