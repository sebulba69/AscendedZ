using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.buff_elements
{
    public class BuffWindStatus : ElementBuffStatus
    {
        public BuffWindStatus() : base()
        {
            _id = StatusId.ElementBuffStatus_Wind;
            BuffElement = Elements.Wind;
            Icon = SkillAssets.GetElementIconByElementEnum(BuffElement);
        }
    }
}
