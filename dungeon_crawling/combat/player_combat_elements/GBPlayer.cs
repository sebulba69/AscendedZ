using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.player_combat_elements
{
    public class GBPlayer
    {
        public Weapon PrimaryWeapon { get; set; }
        public WeaponGrid WeaponGrid { get; set; }

        public GBPlayer() 
        {
            WeaponGrid = new WeaponGrid();
        }
    }
}
