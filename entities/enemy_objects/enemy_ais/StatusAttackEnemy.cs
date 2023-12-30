using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.Projection;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// An enemy that only attacks players with a specific status.
    /// </summary>
    public class StatusAttackEnemy : Enemy
    {
        private Random _rng;
        protected Status _status;

        private const int STATUS_SKILL = 0;
        private const int ATTACK_SKILL = 1;

        public StatusAttackEnemy()
        {
            _rng = new Random();
            Turns = 1;
        }

        /// <summary>
        /// If the status wasn't applied to a character, apply it.
        /// </summary>
        /// <param name="battleSceneObject"></param>
        /// <returns></returns>
        public override ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            List<BattlePlayer> players = battleSceneObject.AlivePlayers;

            // find a player who is affected by the status
            BattlePlayer playerAffectedByStatus = players.Find((player) => { return player.StatusHandler.HasStatus(_status.Id); });

            ISkill skill;
            if (playerAffectedByStatus == null)
                skill = Skills[STATUS_SKILL];
            else
                skill = Skills[ATTACK_SKILL];

            return skill;
        }

        /// <summary>
        /// Get the next target who is not affected by the status.
        /// </summary>
        /// <param name="battleSceneObject"></param>
        /// <returns></returns>
        public override BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            // find a player who is affected by the status
            List<BattlePlayer> players = battleSceneObject.AlivePlayers;
            BattlePlayer playerAffectedByStatus = players.Find((player) => { return player.StatusHandler.HasStatus(_status.Id); });

            if(playerAffectedByStatus == null)
                return players[_rng.Next(players.Count)];
            else
                return playerAffectedByStatus;
        }
    }
}
