using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.player_combat_elements
{
    public class WeaponGrid
    {
        public List<Weapon> Weapons { get; set; }
        public WeaponGrid()
        {
            Weapons = new List<Weapon>();
        }

        public int GetWeaponCountThatMatchesElement(Elements element)
        {
            int count = 0;

            foreach (var weapon in Weapons)
            {
                if (weapon.Element == element)
                {
                    count++;
                }
            }

            return count;
        }

        public long GetHP()
        {
            return Weapons.Sum(w => w.HP);
        }

        public long GetAttack()
        {
            return Weapons.Sum(w => w.Attack);
        }
    }
}
