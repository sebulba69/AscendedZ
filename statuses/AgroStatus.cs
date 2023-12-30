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
    public class AgroStatus : Status
    {
        public AgroStatus() : base()
        {
            _id = StatusId.AgroStatus;
            this.Icon = SkillAssets.AGRO_ICON;
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            base.ActivateStatus(owner);
        }

        public override void UpdateStatus(BattleResult result)
        {
        }

        /// <summary>
        /// Update the status after it's been applied at the start of a turn.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            if (!this.Active)
                this.Active = true;

            this.RemoveStatus = true;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = 1;
            wrapper.CounterColor = Colors.Green;
            wrapper.Description = $"Agro Status: All enemies target the player with the agro status.";

            return wrapper;
        }

        public override Status Clone()
        {
            return new AgroStatus();
        }
    }
}
