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
    public class BEnemyDC : BDCEntity
    {
        private BigInteger _baseHP = 4;
        
        public BEnemyDC(int tier, StatsDC statsDC) : base(statsDC)
        {
        }
    }
}
