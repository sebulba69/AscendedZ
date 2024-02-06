using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.buff_elements
{
    public class BuffElecStatus : ElementBuffStatus
    {
        public BuffElecStatus() : base()
        {
            _id = StatusId.ElementBuffStatus_Elec;
            BuffElement = skills.Elements.Elec;
            Icon = SkillAssets.GetElementIconByElementEnum(BuffElement);
        }
    }
}
