using AscendedZ.dungeon_crawling.combat.battledc.gbstatus;
using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using AscendedZ.skills;
using AscendedZ.statuses;
using AscendedZ.statuses.void_elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public class GBBattlePlayer : GBEntity
    {
        public Weapon Weapon { get; set; }

        public List<GBMinion> Minions { get; set; }

        public GBBattlePlayer() : base()
        {
            Minions = new List<GBMinion>();
        }
    }
}
