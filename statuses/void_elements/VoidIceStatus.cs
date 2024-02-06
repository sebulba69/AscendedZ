using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    public class VoidIceStatus : ChangeElementStatus
    {
        public VoidIceStatus() : base()
        {
            _id = StatusId.VoidElementStatus;

            _elementToChange = skills.Elements.Ice;
            _newResType = resistances.ResistanceType.Nu;
            this.Icon = SkillAssets.VOID_ICE_ICON;
        }

        public override Status Clone()
        {
            return new VoidIceStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
