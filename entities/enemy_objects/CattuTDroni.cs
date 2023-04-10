using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects
{
    public class CattuTDroni : Enemy
    {
        // Our skill indexes
        private const int ELEC = 0;
        private const int ICE = 1;
        private const int STUN = 2;

        private int _targetIndex;
        private Random _rng;
        public CattuTDroni() : base()
        {
            this.Name = "Cattu T'Droni";
            this.MaxHP = 6;
            this.Image = "res://enemy_pics/newpicture97.png";
            this.Resistances = new ResistanceArray();

            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Ice);

            this.Skills.Add(SkillDatabase.ELEC_1);
            this.Skills.Add(SkillDatabase.ICE_1);
            this.Skills.Add(SkillDatabase.STUN_1);
            
            this.Turns = 2;
            
            _rng = new Random();
        }

        public override Enemy Create()
        {
            return new CattuTDroni();
        }

        /// <summary>
        /// Find someone with the status
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            var partyMembers = battleSceneObject.Players.FindAll(member => member.HP > 0);

            // first it checks if the status is present on a party member
            BattlePlayer partyMember = null;
            
            for(int i = 0; i < partyMembers.Count; i++)
            {
                var member = partyMembers[i];
                if (member.StatusHandler.HasStatus(StatusId.StunStatus))
                {
                    partyMember = member;
                    _targetIndex = i;
                    break;
                }
            }

            if(partyMember != null)
            {
                ISkill skill;
                if (partyMember.Resistances.IsWeakToElement(Elements.Ice))
                    skill = this.Skills[ICE];
                else if (partyMember.Resistances.IsWeakToElement(Elements.Elec))
                    skill = this.Skills[ELEC];
                else
                {
                    int skillIndex = _rng.Next(ELEC, ICE + 1);
                    skill = this.Skills[skillIndex];
                }

                return skill;
            }
            else
            {
                // if no one has the status, put it on someone random
                _targetIndex = _rng.Next(0, partyMembers.Count);
                return this.Skills[STUN];
            }
        }

        /// <summary>
        /// Droni determines his target when picking his skill
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            var partyMembers = battleSceneObject.Players.FindAll(member => member.HP > 0);
            return partyMembers[_targetIndex];
        }

        public override void ResetEnemyState()
        {
            // the ai doesn't need to be reset
        }
    }
}
