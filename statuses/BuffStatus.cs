using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    /// <summary>
    /// Buff a specific element when at max for one move each time
    /// a weakness is hit. This base class should be inherited from
    /// for all lower classes.
    /// </summary>
    public class ElementBuffStatus : Status
    {
        private int _stacks;
        private int _timesPerUpdate;

        private int _dmgModifier = 0;
        private const int MAX_STACKS = 2; // it cannot go higher than this value

        protected Elements _buffElement;

        public ElementBuffStatus(Elements element, int damageModifier) : base()
        {
            _timesPerUpdate = 0;
            _stacks = 0;
            _id = StatusId.ElementBuffStatus;
            _buffElement = element;
            _dmgModifier = damageModifier;
            Icon = ArtAssets.GetElementIconByElementEnum(element);
        }

        public override void ActivateStatus(BattleEntity owner)
        {
            _stacks = 0;
            base.ActivateStatus(owner);
        }

        public override void UpdateStatus(BattleResult result)
        {
            if (result.ResultType == BattleResultType.StatusApplied)
            {
                if (result.Target.Equals(_statusOwner))
                {
                    if (_timesPerUpdate % 2 == 0)
                    {
                        _stacks = (_stacks+1 >= MAX_STACKS) ? MAX_STACKS : _stacks + 1;
                        if (_stacks == MAX_STACKS && !this.Active)
                        {
                            this.Active = true;
                            _statusOwner.Skills.ForEach(skill =>
                            {
                                if (skill.GetType().Equals(typeof(ElementSkill)))
                                {
                                    var eSkill = (ElementSkill)skill;
                                    if (eSkill.Element == _buffElement)
                                        eSkill.DamageModifier = _dmgModifier;
                                }
                            });
                        }   

                    }
                    _timesPerUpdate++;
                }
            }

            bool isStatusOwner = result.User.Equals(_statusOwner);
            if (this.Active && isStatusOwner && result.SkillUsed.GetType().Equals(typeof(ElementSkill)))
            {
                var elementSkill = (ElementSkill)result.SkillUsed;
                if (elementSkill.Element == _buffElement)
                {
                    this.RemoveStatus = true;
                    foreach(var skill in result.User.Skills)
                    {
                        if (skill.GetType().Equals(typeof(ElementSkill)))
                        {
                            var edit = (ElementSkill)skill;
                            edit.DamageModifier = 0;
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Update the status after it's been applied.
        /// </summary>
        public override void UpdateStatusTurns(BattleEntity entity)
        {
            _timesPerUpdate = 0;
        }

        public override StatusIconWrapper CreateIconWrapper()
        {
            StatusIconWrapper wrapper = new StatusIconWrapper();

            wrapper.Icon = this.Icon;
            wrapper.Counter = _stacks;
            if (_stacks == MAX_STACKS)
                wrapper.CounterColor = Colors.Green;

            return wrapper;
        }

        public override Status Clone()
        {
            return new ElementBuffStatus(_buffElement, _dmgModifier);
        }
    }
}
