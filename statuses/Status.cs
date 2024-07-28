using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses.buff_elements;
using AscendedZ.statuses.void_elements;
using AscendedZ.statuses.weak_element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    public enum StatusId 
    { 
        Default,
        StunStatus, 
        ElementBuffStatus_Wind,
        ElementBuffStatus_Fire,
        ElementBuffStatus_Elec,
        ElementBuffStatus_Ice, 
        ElementBuffStatus_Dark, 
        ElementBuffStatus_Light, 
        AgroStatus,
        VoidFireStatus,
        VoidIceStatus,
        VoidWindStatus,
        VoidElecStatus,
        VoidDarkStatus,
        VoidLightStatus,
        WexElecStatus,
        WexIceStatus,
        WexFireStatus,
        WexDarkStatus,
        PoisonStatus,
        GuardStatus
    }

    public class Status
    {
        private bool _active = false;
        private bool _removeStatus = false;
        private bool _updateEveryOtherTurn = false;
        protected BattleEntity _statusOwner; // the person with this status

        protected StatusId _id;
        public StatusId Id { get => _id; }
        public string Icon { get; set; }
        public bool Active { get => _active; protected set => _active = value; }
        public bool RemoveStatus { get => _removeStatus; set => _removeStatus = value; }
        /// <summary>
        /// Update during owner's turn or opponent's turn.
        /// </summary>
        public bool UpdateDuringOwnersTurn { get => _updateEveryOtherTurn; protected set => _updateEveryOtherTurn = value; }
        public string Name { get; set; }

        /// <summary>
        /// This function is called when you activate this status for the first time.
        /// </summary>
        public virtual void ActivateStatus(BattleEntity owner)
        {
            _statusOwner = owner;
            this.RemoveStatus = false;
        }

        /// <summary>
        /// This function should be called when a status is applied to an entity after being cast.
        /// By default, applying the same status twice will do nothing.
        /// </summary>
        public virtual void ApplyStatus() {}

        public virtual void ClearStatus() { }

        /// <summary>
        /// Update ongoing statuses with the latest battle result
        /// </summary>
        /// <param name="result"></param>
        public virtual void UpdateStatus(BattleResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update turn count on the status.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void UpdateStatusTurns(BattleEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual StatusIconWrapper CreateIconWrapper()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This Clone method will almost always end up getting called post
        /// Serialization. During battle, if a status needs to preserve state,
        /// then there should be an underlying Clone method that overwrites this one.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Status Clone()
        {
            switch (Id)
            {
                case StatusId.GuardStatus:
                    return new GuardStatus();
                case StatusId.StunStatus:
                    return new StunStatus();
                case StatusId.AgroStatus:
                    return new AgroStatus();
                case StatusId.ElementBuffStatus_Elec:
                    return new BuffElecStatus();
                case StatusId.ElementBuffStatus_Fire:
                    return new BuffFireStatus();
                case StatusId.ElementBuffStatus_Wind:
                    return new BuffWindStatus();
                case StatusId.ElementBuffStatus_Ice:
                    return new BuffIceStatus();
                case StatusId.ElementBuffStatus_Dark:
                    return new BuffDarkStatus();
                case StatusId.ElementBuffStatus_Light:
                    return new BuffLightStatus();
                case StatusId.VoidFireStatus:
                    return new VoidFireStatus();
                case StatusId.VoidIceStatus:
                    return new VoidIceStatus();
                case StatusId.VoidWindStatus:
                    return new VoidWindStatus();
                case StatusId.VoidElecStatus:
                    return new VoidElecStatus();
                case StatusId.VoidDarkStatus:
                    return new VoidDarkStatus();
                case StatusId.VoidLightStatus:
                    return new VoidLightStatus();
                case StatusId.PoisonStatus:
                    return new PoisonStatus();
                case StatusId.WexDarkStatus:
                    return new WeakDarkStatus();
                case StatusId.WexIceStatus:
                    return new WeakIceStatus();
                case StatusId.WexFireStatus:
                    return new WeakFireStatus();
                case StatusId.WexElecStatus:
                    return new WeakElecStatus();
                case StatusId.Default:
                    // hack for backwards compatibility with v0.02 Pre-Alpha
                    if (Icon.Contains(SkillAssets.VOID_FIRE_ICON))
                    {
                        _id = StatusId.VoidFireStatus;
                        return Clone();
                    }
                    else if (Icon.Contains(SkillAssets.VOID_ELEC_ICON))
                    {
                        _id = StatusId.VoidElecStatus;
                        return Clone();
                    }
                    else if (Icon.Contains(SkillAssets.VOID_ICE_ICON))
                    {
                        _id = StatusId.VoidIceStatus;
                        return Clone();
                    }
                    else if (Icon.Contains(SkillAssets.VOID_WIND_ICON))
                    {
                        _id = StatusId.VoidWindStatus;
                        return Clone();
                    }
                    else if (Icon.Contains(SkillAssets.VOID_DARK_ICON))
                    {
                        _id = StatusId.VoidDarkStatus;
                        return Clone();
                    }
                    else if (Icon.Contains(SkillAssets.VOID_LIGHT_ICON))
                    {
                        _id = StatusId.VoidLightStatus;
                        return Clone();
                    }
                    else if (Icon.Contains(SkillAssets.POISON_ICON))
                    {
                        _id = StatusId.PoisonStatus;
                        return Clone();
                    }
                    else if (Icon.Contains(SkillAssets.STUN_ICON))
                    {
                        _id = StatusId.StunStatus;
                        return Clone();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
