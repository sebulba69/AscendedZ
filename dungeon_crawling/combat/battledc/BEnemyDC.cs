using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    /// <summary>
    /// DC stands for dungoen crawl
    /// </summary>
    public class BEnemyDC : GBEntity
    {
        private const long BASEHP = 10;
        private const long GROWTH_RATE = 2;

        public BEnemyDC(int tier)
        {
            MaxHP = BASEHP * (long)Math.Pow(GROWTH_RATE, tier - 1);
            HP = MaxHP;
        }
    }
}
