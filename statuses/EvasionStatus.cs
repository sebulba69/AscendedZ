using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    public class EvasionStatus : Status
    {
        private int _stacks;
        private const int STACK_CAP = 4;
        private int _activeTurns;

        public EvasionStatus() : base()
        {
            _id = StatusId.EvasionStatus;
            _stacks = 0;
            _activeTurns = 0;
            this.Icon = SkillAssets.EVADE_STATUS_ICON;
            Name = "Evasion";
            UpdateDuringOwnersTurn = true;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            _stacks++;
            base.ActivateStatus(owner);
        }

        public override void IncreaseStatusCounter()
        {
            _stacks++;
            if (_stacks >= STACK_CAP)
            {
                _stacks = STACK_CAP;
                if (!Active)
                    Active = true;
            }
        }

        public override void DecreaseStatusCounter()
        {
            _stacks--;
            if (_stacks < STACK_CAP)
                Active = false;

            if (_stacks == 0)
                RemoveStatus = true;
        }

        public override void UpdateStatus(BattleResult result)
        {
        }

        /// <summary>
        /// Update the status after it's been applied at the start of a turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            if(Active)
            {
                _activeTurns++;
                if(_activeTurns == 3)
                {
                    Active = false;
                    RemoveStatus = true;
                }
            }
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = _stacks;
            wrapper.CounterColor = Colors.White;
            if (_stacks == STACK_CAP)
                wrapper.CounterColor = Colors.Green;
            wrapper.Description = "At 3 stacks, the next attack will be evaded (-2 opp. turns)\nthen this status will be removed. This status lasts\nfor 2 turns if not proc'd.";

            return wrapper;
        }
    }
}
