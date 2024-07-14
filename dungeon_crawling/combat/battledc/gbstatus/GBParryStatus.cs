using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbstatus
{
    public class GBParryStatus : GBStatus
    {
        public GBParryStatus()
        {
            Id = GBStatusId.ReactiveStatus;
        }

        public override void NotifyStatusOfGameplayConditions(BDCSystem system)
        {

        }
    }
}
