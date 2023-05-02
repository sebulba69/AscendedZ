using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects.misc_one_offs;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.status_hunter_enemies
{
    public class StatusHunterEnemy : Enemy
    {
        protected const int STATUS_SKILL_INDEX = 0;
        protected const int NONSTATUS_SKILL_INDEX = 1;

        private bool _noStatusDetected;
        private Random _rng;
        protected StatusId StatusToApply { set; get; }

        public StatusHunterEnemy() : base()
        {
            _rng= new Random();
            Turns = 1;
            _noStatusDetected = false;
        }

        /// <summary>
        /// AlternatingEnemies are enemies who ignore the gameState and simply alternates moves
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            if (this.Skills.Count < 2)
                throw new Exception("These enemies must have a minimum of 2 skills");

            ISkill skill = (_noStatusDetected) ? this.Skills[NONSTATUS_SKILL_INDEX] : this.Skills[STATUS_SKILL_INDEX];
            return skill;
        }

        /// <summary>
        /// StunHunterEnemies look for anyone who doesn't have their chosen status and applies it.
        /// If no one has the chosen status, it'll just attack randomly.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            _noStatusDetected = false;

            var partyMembers = battleSceneObject.AlivePlayers;
            var partyMembersWithStatus = partyMembers.FindAll(member => member.StatusHandler.HasStatus(StatusToApply));

            int i;
            if (partyMembersWithStatus.Count == 0)
            {
                _noStatusDetected = true;
                i = _rng.Next(partyMembersWithStatus.Count);
                return partyMembersWithStatus[i];
            }
            else
            {
                i = _rng.Next(partyMembers.Count);
                return partyMembers[i];
            }
        }

        public override void ResetEnemyState() {}
    }
}
