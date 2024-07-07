using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.player_combat_elements
{
   
    public class Weapon
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public WeaponType Type { get; set; }
        public Elements Element { get; set; }
        public long HP { get; set; }
        public long Attack { get; set; }
        public int HitRate { get; set; } // the number of hits you get
        public double CritChance { get; set; }

        // some weapons apply statuses + have status chances
    }
}
