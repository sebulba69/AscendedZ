using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.dungeon_crawling.combat.battledc.gbskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.player_combat_elements
{
    public class GBQueueItem
    {
        /// <summary>
        /// Use a skill
        /// </summary>
        public GBSkill Skill { get; set; }

        /// <summary>
        /// Use a base weapon attack
        /// </summary>
        public Weapon Weapon { get; set; }

        public GBEntity User { get; set; }

        public GBEntity Target { get; set; }

    }
}
