using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public class BDCUpdateWrapper
    {
        public BigInteger HP { get; set; }
        public BigInteger MP { get; set; }
        public BigInteger HPPercentage { get; set; }
        public BigInteger MPPercentage { get; set; }
        public List<Status> Statuses { get; set; } = new();
    }
}
