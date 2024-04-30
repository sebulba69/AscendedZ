﻿using AscendedZ.json_interface_converters;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.skillsdc
{
    public class SkillDC
    {
        /// <summary>
        /// Id is for deserialization
        /// </summary>
        public SkillId Id { get; set; }
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger MPCost { get; set; } = new BigInteger(1);
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger APCost { get; set; } = new BigInteger(1);
        public string Icon { get; set; }
        public Elements Element { get; set; } // will not be used if the id isn't elemental
        public string Name { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger Value { get; set; }
        public BigInteger Level { get; set; } = new BigInteger(1);
        public Status Status { get; set; }
        public void LevelUp()
        {
            if(Value != 0)
            {
                Level++;
                Value += ((Level / 2) + 1);
                MPCost += ((Level / 4) + 1);
            }
        }

        public string GetInfoString()
        {
            StringBuilder infoString = new StringBuilder();

            switch (Id)
            {
                case SkillId.Elemental:
                    infoString.AppendLine($"{Name} Lvl{Level}");
                    infoString.AppendLine($"Deals {Value} Damage");
                    if(Status != null)
                    {
                        infoString.AppendLine($"Applies {Status.Name}");
                    }
                    break;
                case SkillId.Healing:
                    infoString.AppendLine($"{Name} Lvl{Level}");
                    infoString.AppendLine($"Regains {Value} Health");
                    break;
                case SkillId.Status:
                    infoString.AppendLine($"{Name} Lvl{Level}");
                    infoString.AppendLine($"Applies {Status.Name}");
                    break;
            }

            return infoString.ToString();
        }

        public override string ToString()
        {
            return $"[{Value}] {Name} {Level}";
        }
    }
}
