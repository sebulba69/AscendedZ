using AscendedZ.dungeon_crawling.combat.battledc.gbskills;
using AscendedZ.skills;
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
        private const long BASEHP = 1000;
        public int Turns { get; set; }
        public List<GBSkill> Skills { get; set; }

        public BEnemyDC(int tier)
        {
            MaxHP = BASEHP * tier;
            HP = MaxHP;
            Skills=new List<GBSkill>();
        }

        public GBSkill GetSkill()
        {
            return Skills[0];
        }
    }
}
