using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.weak_element
{
    public class WeakElecStatus : ChangeElementStatus
    {
        public WeakElecStatus() : base()
        {
            _id = StatusId.WexElecStatus;

            _elementToChange = skills.Elements.Elec;
            _newResType = resistances.ResistanceType.Wk;
            this.Icon = SkillAssets.WEAK_ELEC_ICON;
        }

        public override Status Clone()
        {
            return new WeakElecStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
