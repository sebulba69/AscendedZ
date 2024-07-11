using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbskills
{
    public enum GBTargets
    {
        Self, Opponent, Allies, Enemies
    }

    public interface IGBSkill
    {
        WeaponType Type { get; } // for ID-ing deserialized objects
        string Icon { get; }
        string Animation { get; }
        GBTargets TargetType { get; set; }
        bool Enabled { get; }

        void Process(GBEntity user, GBEntity target);

        IGBSkill Clone();
    }
}
