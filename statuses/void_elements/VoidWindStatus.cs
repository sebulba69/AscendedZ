using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    public class VoidWindStatus : ChangeElementStatus
    {
        public VoidWindStatus() : base()
        {
            _id = StatusId.VoidElementStatus;

            _elementToChange = skills.Elements.Wind;
            _newResType = resistances.ResistanceType.Nu;
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
