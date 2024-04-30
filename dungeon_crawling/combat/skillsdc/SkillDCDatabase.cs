using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.skillsdc
{
    public class SkillDCDatabase
    {
        public static SkillDC GetElementSkillDC(Elements element)
        {
            return new SkillDC()
            {
                Id = SkillId.Elemental,
                Name = element.ToString(),
                Icon = SkillAssets.GetElementIconByElementEnum(element),
                Element = element,
                TargetType = TargetTypes.SINGLE_OPP,
                Level = 1,
                Value = 2,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                EndupAnimation = SkillAssets.GetAnimationByElementAndTier(1, element)
            };
        }
    }
}
