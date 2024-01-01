using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.json_interface_converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    [JsonConverter(typeof(SkillConverter))]
    public interface ISkill
    {
        /// <summary>
        /// Id is for deserialization
        /// </summary>
        SkillId Id { get; }
        string Name { get; set; }
        TargetTypes TargetType { get; set; }
        string StartupAnimation { get; set; }
        string EndupAnimation { get; set; }
        string Icon { get; set; }
        BattleResult ProcessSkill(BattleEntity target);
        string GetBattleDisplayString();
        void LevelUp();
        string GetUpgradeString();
        ISkill Clone();
    }
}
