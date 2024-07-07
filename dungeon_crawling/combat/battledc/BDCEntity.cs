using AscendedZ.dungeon_crawling.combat.skillsdc;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public class BDCEntity
    {
        public string Image { get; set; }
        protected StatsDC _statsDC;
        public StatsDC Stats { get => _statsDC; }

        public BDCEntity(StatsDC statsDC)
        {
            _statsDC = statsDC;
        }
    }
}
