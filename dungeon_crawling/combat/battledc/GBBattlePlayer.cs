using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public class GBBattlePlayer : GBEntity
    {
        public string Name { get; set; }
        public long Attack { get; set; }
        public Elements Element { get; set; }
        public int HitRate { get; set; }
        public double CritChance { get; set; }
    }
}
