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

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class CattuTDroni : Enemy
    {
        // Our skill indexes
        private const int ELEC = 0;
        private const int ICE = 1;
        private const int STUN = 2;

        private BattlePlayer _target;
        private Random _rng;
        public CattuTDroni() : base()
        {
            Name = EnemyNames.CATTUTDRONI;
            MaxHP = 8;
            Image = EnemyImageAssets.GetEnemyImage(Name);
            Resistances = new ResistanceArray();

            Resistances.CreateResistance(ResistanceType.Wk, Elements.Ice);

            Skills.Add(SkillDatabase.ELEC_1.Clone());
            Skills.Add(SkillDatabase.ICE_1.Clone());
            Skills.Add(SkillDatabase.STUN_S1.Clone());

            Turns = 2;

            _rng = new Random();
        }

        /// <summary>
        /// Find someone with the status
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            var partyMembers = battleSceneObject.AlivePlayers;

            // first it checks if the status is present on a party member
            _target = partyMembers[_rng.Next(0, partyMembers.Count)];
            ISkill skill = this.Skills[_rng.Next(ELEC, ICE + 1)];

            var nonStunnedMembers = partyMembers.FindAll(p => !p.StatusHandler.HasStatus(StatusId.StunStatus));
            
            // no one has the stun status
            if (nonStunnedMembers.Count == partyMembers.Count)
            {
                var membersWithWeakness = nonStunnedMembers.FindAll(member =>
                       member.Resistances.IsWeakToElement(Elements.Elec)
                    || member.Resistances.IsWeakToElement(Elements.Ice));

                if(membersWithWeakness.Count > 0)
                {
                    _target = membersWithWeakness[_rng.Next(0, membersWithWeakness.Count)];
                    skill = this.Skills[STUN];
                }
            }
            else
            {
                _target = partyMembers.Find(p => p.StatusHandler.HasStatus(StatusId.StunStatus));
                
                if (_target.Resistances.IsWeakToElement(Elements.Elec))
                    skill = this.Skills[ELEC];

                if (_target.Resistances.IsWeakToElement(Elements.Ice))
                    skill = this.Skills[ICE];
            }

            return skill;
        }

        /// <summary>
        /// Droni determines his target when picking his skill
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            return _target;
        }

        public override void ResetEnemyState()
        {
            // the ai doesn't need to be reset
        }
    }
}
