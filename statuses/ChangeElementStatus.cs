using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    public class ChangeElementStatus : Status
    {
        private ResistanceType _oldResType;
        protected ResistanceType _newResType;

        protected Elements _elementToChange;

        /// <summary>
        /// Icon must be set from initializer.
        /// </summary>
        public ChangeElementStatus() : base()
        {
            this.UpdateDuringOwnersTurn = true;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            _oldResType = owner.Resistances.GetResistance(_elementToChange);
            owner.Resistances.SetResistance(_newResType, _elementToChange);
            base.ActivateStatus(owner);
        }

        public override void UpdateStatus(BattleResult result) {}

        /// <summary>
        /// Update the status after it's been applied at the start of the player/enemy (depends on owner) turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _statusOwner.Resistances.SetResistance(_oldResType, _elementToChange);
            this.RemoveStatus = true;
        }

        public override void ClearStatus()
        {
            _statusOwner.Resistances.SetResistance(_oldResType, _elementToChange);
            this.RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 1;
            wrapper.CounterColor = Colors.Green;
            wrapper.Description = $"Sets the target's resistance to a specific element to {_newResType} for 1 turn.";

            return wrapper;
        }
    }
}
