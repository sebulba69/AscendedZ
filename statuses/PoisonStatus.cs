﻿using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    public class PoisonStatus : Status
    {
        private int _activeTurns;

        public PoisonStatus() : base() 
        {
            _id = StatusId.PoisonStatus;
            _activeTurns = 0;
            this.Icon = SkillAssets.POISON_ICON;
            Name = "Poison";
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            _activeTurns++;
            base.ActivateStatus(owner);
        }

        public override void UpdateStatus(BattleResult result) { }

        /// <summary>
        /// Update the status after it's been applied at the start of the player/enemy (depends on owner) turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _statusOwner.HP -= (int)(_statusOwner.HP * 0.15);
            if (_statusOwner.HP < 0) 
                _statusOwner.HP = 0;
            
            if (_activeTurns == 2)
                RemoveStatus = true;
        }


        public override void ClearStatus()
        {
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = _activeTurns;
            wrapper.CounterColor = Colors.White;
            wrapper.Description = $"Reduce HP by 15% for 2 turns.";

            return wrapper;
        }
    }
}
