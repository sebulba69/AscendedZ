using AscendedZ.dungeon_crawling.combat.battledc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.player_combat_elements
{
    public class GBMinion : GBBattlePlayer
    {
        public bool Equipped { get; set; }

        public GBMinion() : base() {}

        public void LevelUp()
        {
            Weapon.AddXP(Weapon.XPRequired);
        }
    }
}
