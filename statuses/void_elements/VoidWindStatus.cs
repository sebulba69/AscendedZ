using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    public class VoidWindStatus : VoidElementStatus
    {
        public VoidWindStatus() : base()
        {
            _voidElement = skills.Elements.Wind;
            this.Icon = SkillAssets.VOID_WIND_ICON;
        }

        public override Status Clone()
        {
            return new VoidWindStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
