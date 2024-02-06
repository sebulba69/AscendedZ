using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses.void_elements
{
    public class VoidFireStatus : ChangeElementStatus
    {
        public VoidFireStatus() : base()
        {
            _id = StatusId.VoidElementStatus;
            _elementToChange = skills.Elements.Fir;
            _newResType = resistances.ResistanceType.Nu;
            this.Icon = SkillAssets.VOID_FIRE_ICON;
        }

        public override Status Clone()
        {
            return new VoidFireStatus();
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            return base.CreateIconWrapper();
        }
    }
}
