using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    public class VoidFireStatus : VoidElementStatus
    {
        public VoidFireStatus() : base()
        {
            _voidElement = skills.Elements.Fir;
            this.Icon = SkillAssets.VOID_FIRE_ICON;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
