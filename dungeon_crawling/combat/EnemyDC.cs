using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat
{
    /// <summary>
    /// DC stands for dungoen crawl
    /// </summary>
    public class EnemyDC
    {
        private BigInteger _baseHP = 4;

        private BigInteger _maxHP;

        public BigInteger HP { get; set; }
        public string Image { get; set; }

        public EnemyDC(int tier)
        {
            _maxHP = _baseHP + Equations.GetDungeonCrawlingHPBoost(tier);
            HP = _maxHP;
        }
    }
}
