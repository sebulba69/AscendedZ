using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using AscendedZ.dungeon_crawling.scenes.weapon_gacha;
using AscendedZ.entities;
using AscendedZ.game_object;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.scenes.minion_hut
{
    public class MinionHutObject
    {
        private WeaponGachaGenerator _weaponGachaGenerator;
        private int _tierDC;
        private GBPlayer _player;
        private Currency _minionShards;
        private Random _random;
        
        private readonly string[] _minionNames = { PartyNames.BuceyRed, PartyNames.BuceyGreen, PartyNames.BuceyBlue };

        public MinionHutObject()
        {
            var go = PersistentGameObjects.GameObjectInstance();
            _tierDC = go.MaxTierDC;
            _player = go.MainPlayer.DungeonPlayer;
            _weaponGachaGenerator = new WeaponGachaGenerator();
            _minionShards = go.MainPlayer.Wallet.Currency[SkillAssets.MINION_SHARD];
            _random = new Random();
        }

        public void MakeMinion()
        {
            if (_minionShards.Amount - 1 >= 0) 
            {
                _minionShards.Amount--;
                string name = _minionNames[_random.Next(_minionNames.Length)];

                GBMinion minion = new GBMinion();
                minion.Image = CharacterImageAssets.GetImagePath(name);
                minion.Weapon = _weaponGachaGenerator.GenerateWeapons(1, _tierDC)[0];
                minion.MaxHP = minion.Weapon.HP;
                minion.HP = minion.MaxHP;

                _player.Minions.Add(minion);
            }
        }

        public void UpgradeMinion(int selected)
        {
            if (_minionShards.Amount - 10 >= 0)
            {
                _minionShards.Amount -= 10;
                var minion = GetMinion(selected);
                minion.LevelUp();
            }
        }

        public void EquipMinion(int selected)
        {
            if (CanEquipMinion())
            {
                var minion = GetMinion(selected);
                minion.Equipped = true;
            }
        }

        public void UnequipMinion(int selected)
        {
            var minion = GetMinion(selected);
            if (minion.Equipped)
                minion.Equipped = false;
        }

        public void DeleteMinion(int selected)
        {
            var minions = GetMinions();
            minions.RemoveAt(selected);
        }

        public GBMinion GetMinion(int selected)
        {
            return _player.Minions[selected];
        }

        public List<GBMinion> GetMinions()
        {
            return _player.Minions;
        }

        private bool CanEquipMinion()
        {
            int equipped = GetMinions().FindAll(x => x.Equipped).Count;
            return equipped < 2;
        }
    }
}
