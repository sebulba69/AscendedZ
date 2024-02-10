using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.buff_elements
{
    public class BuffIceStatus : ElementBuffStatus
    {
        public BuffIceStatus() : base()
        {
            _id = StatusId.ElementBuffStatus_Ice;
            BuffElement = skills.Elements.Ice;
            Icon = SkillAssets.GetElementIconByElementEnum(BuffElement);
        }

        public override Status Clone()
        {
            return new BuffIceStatus
            {
                BuffElement = this.BuffElement,
                Amount = this.Amount,
                Stacks = _stacks,
                Icon = this.Icon
            };
        }
    }
}
