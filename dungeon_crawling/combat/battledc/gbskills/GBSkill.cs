using AscendedZ.dungeon_crawling.combat.battledc.gbstatus;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbskills
{
    public class GBSkill
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public GBStatusId Status { get; set; }
        public Elements Element { get; set; }
        public GBTargetType TargetType { get; set; }

        public void ApplySinglePlayer(GBBattlePlayer user, BEnemyDC target)
        {
            if(Status == GBStatusId.None)
            {
                if (TargetType == GBTargetType.Self)
                {
                    user.HandleSelfStatus(Status);
                }
            }
        }
    }
}
