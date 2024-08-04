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
   

    public class AtkChangeStatus : Status
    {
        private const int TURN_CAP = 3;
        private const int STACK_CAP = 2;

        private double _baseMultiplier;
        private int _turnCount, _stacks;

        public AtkChangeStatus() : base()
        {
            _id = StatusId.AtkChangeStatus;
            _baseMultiplier = 0.15;
            _turnCount = 0;
            _stacks = 0;
            Icon = SkillAssets.ATK_STATUS_ICON;
            UpdateDuringOwnersTurn = true;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            base.ActivateStatus(owner);
            _stacks = 0;
            ApplyBuff();
        }

        public override void IncreaseStatusCounter()
        {
            _stacks++;
            _turnCount = 0;

            if (_stacks == 0)
                _stacks = 1;

            if (_stacks >= STACK_CAP)
                _stacks = STACK_CAP;

            ApplyBuff();
        }

        public override void DecreaseStatusCounter()
        {
            _stacks--;
            _turnCount = 0;

            if (_stacks == 0)
                _stacks = -1;

            if (_stacks <= (STACK_CAP * -1))
                _stacks = (STACK_CAP * -1);

            ApplyBuff();
        }

        private void ApplyBuff()
        {
            if (_statusOwner == null)
                return;

            for(int e = 0; e < _statusOwner.ElementDamageModifiers.Length; e++)
                _statusOwner.ElementDamageModifiers[e] = (_baseMultiplier * _stacks);
        }


        public override void UpdateStatus(BattleResult result)
        {
        }

        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _turnCount++;

            if (_turnCount == TURN_CAP)
                RemoveStatus = true;
        }

        public override void ClearStatus()
        {
            for (int e = 0; e < _statusOwner.ElementDamageModifiers.Length; e++)
                _statusOwner.ElementDamageModifiers[e] = 0;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = _stacks;
            wrapper.CounterColor = Colors.White;

            if(_turnCount == TURN_CAP - 1)
            {
                wrapper.CounterColor = Colors.Red;
            }

            wrapper.Description = $"Change damage for all elements by {Math.Round((_baseMultiplier * _stacks) * 100, 1)}%\nFor {TURN_CAP} turns. Cap = +-2";

            return wrapper;
        }
    }
}
