using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbstatus
{
    public class ParryStatus : GBStatusBase, IGBStatus
    {
        public ParryStatus()
        {
            _name = "Parry";
            _icon = SkillAssets.DAGGER_ICON;
        }

        public IGBStatus Clone()
        {
            return new ParryStatus();
        }
    }
}
