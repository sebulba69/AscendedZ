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
        private readonly Dictionary<GBStatusId, Action> _statusHandlers;

        public string Name { get; set; }
        public long Attack { get; set; }
        public Elements Element { get; set; }
        public Weapon Weapon { get; set; }

        public GBBattlePlayer()
        {
            _statusHandlers = new Dictionary<GBStatusId, Action>()
            {
                { GBStatusId.DaggerParry, ApplyParryStance }
            };
        }

        public void HandleSelfStatus(GBStatusId id)
        {
            _statusHandlers[id].Invoke();
        }

        private void ApplyParryStance()
        {
            var parry = new GBStatus() 
            {
                Id = GBStatusId.DaggerParry,
                Icon = SkillAssets.DAGGER_ICON,
                Owner = this,
                TurnCount = 0
            };

            Statuses.Add(parry);
        }
    }
}
