using AscendedZ.json_interface_converters;
using AscendedZ.skills;
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
        public Elements Element { get; set; } // will not be used if the id isn't elemental
        public string Name { get; set; }
        public TargetTypes TargetType { get; set; }
        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger Value { get; set; }
        public BigInteger Level { get; set; } = new BigInteger(1);

        public void LevelUp()
        {
            Level++;
            Value += ((Level/2) + 1);
        }

        public override string ToString()
        {
            return $"[{Value}] {Name} {Level}";
        }

        public SkillDC Clone()
        {
            return new SkillDC()
            {
                Id = Id,
                TargetType = TargetType,
                Name = Name,
                Element = Element,
                StartupAnimation = StartupAnimation,
                EndupAnimation = EndupAnimation,
                Value = Value,
                Level = Level
            };
        }
    }
}
