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

        public string Description { get; protected set; }

        public Enemy() {}

        public void Boost(int level, int boost)
        {
            HP += boost;
            foreach(ISkill skill in Skills)
            {
                skill.LevelUpEnemy(level, boost);
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
    }
}
