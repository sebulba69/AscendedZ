using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    /// <summary>
    /// If you hit 3 stacks of this status, the player with the status cannot attack for 1 turn.
    /// Then, the status is removed.
    /// </summary>
    public class StunStatus : Status
    {
        /// <summary>
        /// The total amount of turns the Status is active for.
        /// </summary>
        private const int ACTIVE_TURNS = 1;
        private const int REQUIRED_STACKS_TO_BE_ACTIVE = 2;

        private int _stacks;
        private int _activeTurns;

        public StunStatus() : base()
        {
            _id = StatusId.StunStatus;
            this.Icon = SkillAssets.STUN_ICON;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            _stacks = 0;
            _activeTurns = 0;
            base.ActivateStatus(owner);
        }

        public override void UpdateStatus(BattleResult result)
        {
            if(result.ResultType == BattleResultType.Wk
                && _stacks < REQUIRED_STACKS_TO_BE_ACTIVE
                && result.Target.Equals(_statusOwner))
            {
                _stacks++;
                if (_stacks == REQUIRED_STACKS_TO_BE_ACTIVE)
                    _statusOwner.CanAttack = false;
                else
                    _statusOwner.CanAttack = true;
            }
        }

        /// <summary>
        /// Update the status after it's been applied.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            if(_stacks == REQUIRED_STACKS_TO_BE_ACTIVE)
            {
                if (!this.Active)
                    this.Active = true;

                _activeTurns++;
                if (_activeTurns == ACTIVE_TURNS)
                {
                    this.RemoveStatus = true;
                    entity.CanAttack = true;
                }
            }
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = _stacks;
            if (_stacks == REQUIRED_STACKS_TO_BE_ACTIVE)
                wrapper.CounterColor = Colors.Green;

            wrapper.Description = $"Stun Status: Prevents attacks at {REQUIRED_STACKS_TO_BE_ACTIVE} stacks for 1 turn.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new StunStatus();
        }
    }
}
