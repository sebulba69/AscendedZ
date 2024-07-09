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
    public class BEnemyDC
    {
        public string Image { get; set; }
        public long HP { get; set; }

        public BEnemyDC(int tier)
        {
            HP = tier * 10;
        }
    }
}
