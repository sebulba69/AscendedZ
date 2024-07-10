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

        public void Add(Weapon weapon)
        {
            if (Weapons.Count == 9)
                return;

            weapon.Equipped = true;
            Weapons.Add(weapon);
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

        public long GetHP(Weapon primaryWeapon) 
        {
            double percentage = 0;
            long hp = GetHP();

            foreach(var weapon in Weapons)
            {
                if(weapon.Element == primaryWeapon.Element)
                {
                    percentage += 0.15;
                }
            }

            hp = hp + (long)(hp * percentage);

            return hp;
        }

        public long GetAttack(Weapon primaryWeapon)
        {
            double percentage = 0;
            long atk = GetAttack();

            foreach (var weapon in Weapons)
            {
                if (weapon.Element == primaryWeapon.Element)
                {
                    percentage += 0.15;
                }
            }

            atk = atk + (long)(atk * percentage);

            return atk;
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
