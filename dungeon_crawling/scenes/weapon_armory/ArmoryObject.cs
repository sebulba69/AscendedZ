using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using AscendedZ.game_object;
using Godot;
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
        private List<Weapon> _fullWeaponList;

        private Weapon _smeltSelect;

        public ArmoryObject(GBPlayer player)
        {
            _player = player;
            _fullWeaponList = new List<Weapon>();

            SetupWeaponList();

            foreach(var weapon in _fullWeaponList)
            {
                weapon.SmeltInto = false;
            }
        }

        public void SetupWeaponList()
        {
            _fullWeaponList.Clear();

            if (_player.PrimaryWeapon != null)
            {
                _fullWeaponList.Add(_player.PrimaryWeapon);
            }

            _fullWeaponList.AddRange(_player.WeaponGrid.Weapons);
            _fullWeaponList.AddRange(_player.Reserves.Reserves.FindAll(x => !x.Equipped));
        }

        public void ChangeGridStatus(int index)
        {
            var reserve = _fullWeaponList[index];
            var grid = _player.WeaponGrid;

            if (reserve.Equipped)
            {
                if(!reserve.PrimaryWeapon)
                {
                    if (_smeltSelect == reserve)
                        _smeltSelect = null;

                    reserve.SmeltInto = false;
                    grid.Weapons.Remove(reserve);
                    reserve.Equipped = false;
                }
            }
            else
            {
                grid.Add(reserve);
            }

            PersistentGameObjects.Save();
        }

        public void SetPrimaryWeapon(int index)
        {
            var equipped = _fullWeaponList[index];
            if (!equipped.PrimaryWeapon && equipped.Equipped)
            {
                if (_player.PrimaryWeapon != null)
                {
                    _player.PrimaryWeapon.PrimaryWeapon = false;
                    _player.WeaponGrid.Weapons.Add(_player.PrimaryWeapon);
                }
 
                equipped.PrimaryWeapon = true;
                _player.WeaponGrid.Weapons.Remove(equipped);
                _player.PrimaryWeapon = equipped;

                PersistentGameObjects.Save();
            }
        }

        public void SetSmelt(int index) 
        {
            var select = _fullWeaponList[index];
            if (select.Equipped)
            {
                if (_smeltSelect != null)
                    _smeltSelect.SmeltInto = false;

                _smeltSelect = select;
                if(_smeltSelect.SmeltInto)
                    _smeltSelect.SmeltInto = false;
                else
                    _smeltSelect.SmeltInto = true;
            }
        }

        public void SmeltReserveWeapon(int rIndex)
        {
            var reserve = _fullWeaponList[rIndex];
            if(_smeltSelect != null && !reserve.PrimaryWeapon && !reserve.Equipped)
            {
                _player.Reserves.Remove(reserve);
                _smeltSelect.AddXP(1);
                SetupWeaponList();
                PersistentGameObjects.Save();
            }
        }

        public long GetTotalHP()
        {
            return _player.GetTotalHP();
        }

        public long GetTotalAtk()
        {
            return _player.GetTotalAtk();
        }

        public Weapon GetPrimaryWeapon()
        {
            return _player.PrimaryWeapon;
        }

        public List<Weapon> GetWeaponList()
        {
            return _fullWeaponList;
        }
    }
}
