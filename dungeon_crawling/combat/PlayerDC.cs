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
        public PlayerDC()
        {
        }

        
        public BPlayerDC MakeBattlePlayerDC()
        {
            StatsDC stats = new StatsDC();
            /*
            stats.ApplyStats(Stats);
            stats.ApplyStats(ArmorSet.GetAllStats());
            */
            return new BPlayerDC(stats);
        }
    }
}
