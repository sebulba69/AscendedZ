using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.scenes.weapon_armory
{
    public class ArmoryObject
    {
        private GBPlayer _player;

        public ArmoryObject(GBPlayer player)
        {
            _player = player;
        }

        public void ChangeGridStatus(int index)
        {
            var reserve = GetReserves()[index];
            var grid = _player.WeaponGrid;

            if (reserve.Equipped)
            {
                if(!reserve.PrimaryWeapon)
                {
                    grid.Weapons.Remove(reserve);
                    reserve.Equipped = false;
                }
            }
            else
            {
                grid.Weapons.Add(reserve);
                reserve.Equipped = true;
            }
        }


        public List<Weapon> GetEquipped()
        {
            return _player.Reserves.Reserves.FindAll(x => x.Equipped);
        }

        public List<Weapon> GetReserves()
        {
            return _player.Reserves.Reserves;
        }
    }
}
