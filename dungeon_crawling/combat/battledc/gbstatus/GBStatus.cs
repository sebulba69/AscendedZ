using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbstatus
{
    public class GBStatus
    {
        public GBStatusId Id { get; set; }
        public GBEntity Owner { get; set; }
        public string Icon { get; set; }

        public virtual void NotifyStatusOfGameplayConditions(BDCSystem system) 
        { 
        }
    }
}
