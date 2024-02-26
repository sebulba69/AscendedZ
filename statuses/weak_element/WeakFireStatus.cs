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
            _id = StatusId.WexFireStatus;

            _elementToChange = skills.Elements.Fire;
            _newResType = resistances.ResistanceType.Wk;
            this.Icon = SkillAssets.WEAK_FIRE_ICON;

            Name = "Weak Fire";
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
