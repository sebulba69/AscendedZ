using AscendedZ.dungeon_crawling.combat.battledc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.player_combat_elements
{
    public class GBPlayer
    {
        public int ShopLevel { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Weapon PrimaryWeapon { get; set; }
        public WeaponGrid WeaponGrid { get; set; }
        public WeaponReserves Reserves { get; set; }

        public GBPlayer() 
        {
            WeaponGrid = new WeaponGrid();
            Reserves = new WeaponReserves();
            ShopLevel = 1;
        }

        public GBBattlePlayer MakeGBBattlePlayer()
        {
            GBBattlePlayer gBBattlePlayer = new GBBattlePlayer() 
            {
                Name = Name,
                Image = Image
            };

            return gBBattlePlayer;
        }
    }
}
