using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.weak_element
{
    public class WeakFireStatus : ChangeElementStatus
    {
        public WeakFireStatus() : base()
        {
            _id = StatusId.WexElementStatus;
            _elementToChange = skills.Elements.Fir;
            _newResType = resistances.ResistanceType.Wk;
            this.Icon = SkillAssets.WEAK_FIRE_ICON;
        }

        public override Status Clone()
        {
            return new WeakFireStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }

    }
}
