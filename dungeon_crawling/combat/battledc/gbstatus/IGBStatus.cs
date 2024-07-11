using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbstatus
{
    public interface IGBStatus
    {
        /// <summary>
        /// The person afflicted with the status
        /// </summary>
        GBEntity Owner { get; }
        string Name { get; }
        string Icon { get; }

        void Apply(GBEntity owner);

        IGBStatus Clone();
    }
}
