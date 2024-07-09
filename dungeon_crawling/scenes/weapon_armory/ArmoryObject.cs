using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public void SetPrimaryWeapon(int index)
        {
            var equipped = GetEquipped()[index];
            if (!equipped.PrimaryWeapon)
            {
                if (_player.PrimaryWeapon != null)
                {
                    _player.PrimaryWeapon.PrimaryWeapon = false;
                    _player.WeaponGrid.Weapons.Add(_player.PrimaryWeapon);
                }
 
                equipped.PrimaryWeapon = true;
                _player.WeaponGrid.Weapons.Remove(equipped);
                _player.PrimaryWeapon = equipped;
            }
        }

        public void SmeltReserveWeapon(int rIndex, int eIndex)
        {
            var reserve = GetReserves()[rIndex];
            var equipped = GetEquipped()[eIndex];
            if (!reserve.PrimaryWeapon && !reserve.Equipped)
            {
                _player.Reserves.Remove(reserve);
                equipped.AddXP(1);
            }
        }

        public long GetTotalHP()
        {
            long total = _player.WeaponGrid.GetHP();

            if (_player.PrimaryWeapon != null)
            {
                total += _player.PrimaryWeapon.HP;
            }

            return total;
        }

        public long GetTotalAtk()
        {
            long total = _player.WeaponGrid.GetAttack();

            if (_player.PrimaryWeapon != null)
            {
                total += _player.PrimaryWeapon.Attack;
            }

            return total;
        }

        public Weapon GetPrimaryWeapon()
        {
            return _player.PrimaryWeapon;
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
