using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
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

        public Status Status { set => _status = value; }

        public StatusAttackEnemy()
        {
            _rng = new Random();
            Turns = 1;
            Description = $"Class: Status Enemy\nDescription: Randomly applies a status to all players who don't have one,\nthen it focuses on random attacks.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            List<BattlePlayer> players = battleSceneObject.AlivePlayers;
            ISkill skill;
            BattleEntity target;

            BattlePlayer playerAffectedByAgro = players.Find((player) => { return player.StatusHandler.HasStatus(StatusId.AgroStatus); });

            // agro overrides this enemy's programming
            if(playerAffectedByAgro == null)
            {
                // find a player who does not have the status
                List<BattlePlayer> playersUnaffectedByStatus = players.FindAll((player) => { return !player.StatusHandler.HasStatus(_status.Id); });

                if (playersUnaffectedByStatus.Count > 0)
                {
                    skill = Skills[STATUS_SKILL];
                    target = playersUnaffectedByStatus[_rng.Next(playersUnaffectedByStatus.Count)];
                }
                else
                {
                    skill = Skills[ATTACK_SKILL];
                    target = players[_rng.Next(players.Count)];
                }
            }
            else
            {
                skill = Skills[ATTACK_SKILL];
                target = playerAffectedByAgro;
            }


            return new EnemyAction
            {
                Skill = skill,
                Target = target
            };
        }

        public override void ResetEnemyState()
        {
            // Do nothing.
        }
    }
}
