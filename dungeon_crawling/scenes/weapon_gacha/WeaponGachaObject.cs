using AscendedZ.currency;
using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.scenes.weapon_gacha
{
    public class WeaponGachaObject
    {
        private GBPlayer _player;
        private Wallet _wallet;

        private WeaponGachaGenerator _weaponGachaGenerator;
        private List<Weapon> _generated;

        public WeaponGachaObject(GBPlayer player, Wallet wallet)
        {
            _player = player;
            _wallet = wallet;
            _weaponGachaGenerator = new WeaponGachaGenerator();
        }

        public bool CanPlayerAffordWeapon(int number)
        {
            int cost = 10 * number;
            int amount = _wallet.Currency[SkillAssets.DELLENCOIN].Amount;

            return cost <= amount;
        }

        public bool IsSpaceForWeapons(int number)
        {
            var reserves = _player.Reserves;

            if (reserves.AreReservesCapped())
                return false;
            else
                return !reserves.WillAddingGoOverReserveCap(number);
        }

        public List<Weapon> GenerateWeapons(int number) 
        {
            var weapons = _weaponGachaGenerator.GenerateWeapons(number, _player.ShopLevel);
            _generated = weapons;
            _wallet.Currency[SkillAssets.DELLENCOIN].Amount -= _generated.Count * 10;
            return weapons;
        }

        public int GetDellenCoinsOwned()
        {
            return _wallet.Currency[SkillAssets.DELLENCOIN].Amount;
        }

        public void ClaimWeapons()
        {
            foreach(var weapon in _generated)
            {
                _player.Reserves.Add(weapon);
            }
            _generated = null;
        }

        public void LevelUpShop()
        {
            _player.ShopLevel++;
            _wallet.Currency[SkillAssets.DELLENCOIN].Amount -= 1000;
        }

        public int GetShopLevel()
        {
            return _player.ShopLevel;
        }
    }
}
