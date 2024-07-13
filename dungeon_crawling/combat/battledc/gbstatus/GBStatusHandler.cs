using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbstatus
{
    public class GBStatusHandler
    {
        private readonly Dictionary<GBStatusId, Action<GBEntity>> _statusHandlers;

        public List<GBStatus> Statuses { get; set; }

        public GBStatusHandler()
        {
            Statuses = new List<GBStatus>();
            _statusHandlers = new Dictionary<GBStatusId, Action<GBEntity>>()
            {
                { GBStatusId.DaggerParry, ApplyParryStance }
            };
        }

        public void HandleSelfStatus(GBStatusId id, GBEntity user)
        {
            _statusHandlers[id].Invoke(user);
        }

        private void ApplyParryStance(GBEntity user)
        {
            var parry = new GBStatus()
            {
                Id = GBStatusId.DaggerParry,
                Icon = SkillAssets.DAGGER_ICON,
                Owner = user,
                TurnCount = 0
            };

            Statuses.Add(parry);
        }
    }
}
