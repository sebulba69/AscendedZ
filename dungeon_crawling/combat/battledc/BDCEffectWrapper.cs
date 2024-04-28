using AscendedZ.dungeon_crawling.combat.skillsdc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public class BDCEffectWrapper
    {
        public BigInteger HPChanged { get; set; }
        public SkillDC Skill { get; set; }
        public string Result { get; set; }
    }
}
