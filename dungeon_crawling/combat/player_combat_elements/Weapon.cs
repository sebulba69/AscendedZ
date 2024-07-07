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
        private const int LEVEL_CAP = 999;

        public bool Equipped { get; set; }
        public int Level { get; set; }
        public string Icon { get; set; }
        public WeaponType Type { get; set; }
        public Elements Element { get; set; }
        public long HP { get; set; }
        public long Attack { get; set; }
        public int HitRate { get; set; } // the number of hits you get
        public double CritChance { get; set; }

        public Weapon()
        {
            Level = 1;
            HitRate = 1;
            CritChance = 0.15;
            Equipped = false;
        }
    }
}
