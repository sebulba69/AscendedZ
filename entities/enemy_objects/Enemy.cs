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
        public bool IsBoss { get => _isBoss; }
        
        protected Random _rng;

        public string Description { get; set; }

        public Enemy() 
        {
            _rng = new Random();
        }

        public void Boost(int tier, int boost)
        {
            MaxHP += boost;
            int numLevelUps = tier / 2;

            for (int i = 0; i < numLevelUps; i++)
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
    }
}
