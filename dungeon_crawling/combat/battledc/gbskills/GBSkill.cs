using AscendedZ.dungeon_crawling.combat.battledc.gbstatus;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbskills
{
    public enum GBSkillType
    {
        EnemyElement, PlayerStatus
    }

    public class GBSkill
    {
        public GBSkillType Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public long Value { get; set; }
        public GBStatusId Status { get; set; }
        public Elements Element { get; set; }
        public GBTargetType TargetType { get; set; }
    }
}
