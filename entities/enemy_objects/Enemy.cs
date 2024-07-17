using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects
{
    public class Enemy : BattleEntity
    {
        protected bool _isBoss = false;
        protected bool _isAgroOverride = false;
        public bool IsBoss { get => _isBoss; }
        
        protected Random _rng;

        public string Description { get; set; }

        public Enemy() 
        {
            Type = EntityType.Enemy;
            _rng = new Random();
        }

        public void Boost(int tier)
        {
            int boost = (tier+1);
            if(boost == 0)
                boost = 1;

            MaxHP *= (int)(boost * 0.75);

            for (int i = 0; i < (boost/2)+1; i++)
            {
                foreach (ISkill skill in Skills)
                {
                    skill.LevelUp();
                }
            }
        }

        public virtual void ResetEnemyState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Target + a Skill to be used during the next battle.
        /// </summary>
        /// <param name="battleSceneObject"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            throw new NotImplementedException();
        }

        protected BattleEntity GetRandomAlivePlayer(BattleSceneObject battleSceneObject)
        {
            var player = battleSceneObject.AlivePlayers[_rng.Next(battleSceneObject.AlivePlayers.Count)];
            return player;
        }

        protected BattleEntity GetTargetAffectedByAgro(BattleSceneObject battleSceneObject)
        {
            var agro = battleSceneObject.AlivePlayers.Find(p => p.StatusHandler.HasStatus(statuses.StatusId.AgroStatus));

            _isAgroOverride = (agro != null);

            return agro;
        }

        /// <summary>
        /// Utility function for a feature common to most enemy AI.
        /// </summary>
        /// <param name="battleSceneObject"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected List<BattlePlayer> FindPlayersWithWeaknessToElement(BattleSceneObject battleSceneObject, Elements element)
        {
            List<BattlePlayer> players = battleSceneObject.AlivePlayers;

            return players.FindAll(player => player.Resistances.IsWeakToElement(element));
        }

        public override string GetLogName()
        {
            return $"[color=coral]{this.Name}[/color]";
        }
    }
}
