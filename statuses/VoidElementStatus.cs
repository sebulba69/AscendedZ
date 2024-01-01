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
    public class VoidElementStatus : Status
    {
        private ResistanceType _oldResType;

        public Elements VoidElement { private get; set; }

        /// <summary>
        /// Icon must be set from initializer.
        /// </summary>
        public VoidElementStatus() : base()
        {
            _id = StatusId.VoidElementStatus;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            _oldResType = owner.Resistances.GetResistance(VoidElement);
            owner.Resistances.SetResistance(ResistanceType.Nu, VoidElement);

            base.ActivateStatus(owner);
        }

        public override void UpdateStatus(BattleResult result) {}

        /// <summary>
        /// Update the status after it's been applied at the start of the player/enemy (depends on owner) turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _statusOwner.Resistances.SetResistance(_oldResType, VoidElement);
            this.RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 1;
            wrapper.CounterColor = Colors.Green;
            wrapper.Description = $"Void Status: Sets the target's resistance to a specific element to Null for 1 turn.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new AgroStatus();
        }
    }
}
