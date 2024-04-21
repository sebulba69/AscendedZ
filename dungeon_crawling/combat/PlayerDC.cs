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
        public PlayerDC()
        {
            if (Stats == null)
                Stats = new StatsDC();
        }
    }
}
