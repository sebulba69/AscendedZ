using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.armordc
{
    public class ArmorSetDC
    {
        private ArmorDC[] _armor;

        public ArmorDC[] Armor { get => _armor; set => _armor = value; }

        public ArmorSetDC()
        {
            _armor = new ArmorDC[Enum.GetNames<ArmorPiece>().Length];
        }

        public StatsDC GetAllStats()
        {
            StatsDC stats = new StatsDC();
            foreach(ArmorDC armor in _armor)
            {
                if(armor != null)
                {
                    armor.StatsDC.ApplyStats(stats);
                }
            }
            return stats;
        }
    }
}
