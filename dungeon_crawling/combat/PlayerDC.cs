using AscendedZ.dungeon_crawling.combat.armordc;
using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.dungeon_crawling.combat.skillsdc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat
{
    public class PlayerDC
    {
        public StatsDC Stats { get; set; }

        public ArmorSetDC ArmorSet { get; set; }

        public List<ArmorDC> Reserves { get; set; }

        public PlayerDC()
        {
            if(Stats == null)
            {
                Stats = new StatsDC()
                 {
                     Level = 1,
                     HP = 10,
                     MP = 10,
                     AP = 1,
                     AttackRate = 1,
                     CriticalRate = 10,
                     HealAmount = 2
                 };
            }

            if (ArmorSet == null)
                ArmorSet = new ArmorSetDC();

            if(Reserves == null)
                Reserves = new List<ArmorDC>();
        }

        /// <summary>
        /// Equip armor from Reserves to ArmorSet
        /// </summary>
        /// <param name="armor"></param>
        public void EquipArmor(ArmorDC armor)
        {
            Reserves.Remove(armor);

            int piece = (int)armor.Piece;
            if (ArmorSet.Armor[piece] != null)
            {
                ArmorDC armorDC = ArmorSet.Armor[piece];
                Reserves.Add(armorDC);
            }

            ArmorSet.Armor[piece] = armor;
        }

        public BPlayerDC MakeBattlePlayerDC()
        {
            StatsDC stats = new StatsDC();
            stats.ApplyStats(Stats);
            stats.ApplyStats(ArmorSet.GetAllStats());

            return new BPlayerDC(stats);
        }
    }
}
