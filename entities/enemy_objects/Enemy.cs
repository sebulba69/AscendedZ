using AscendedZ.battle;
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
        public Enemy() {}

        public virtual void ResetEnemyState()
        {
            throw new NotImplementedException();
        }

        public virtual Enemy Create()
        {
            throw new NotImplementedException();
        }

        public virtual ISkill GetNextMove(BattleSceneObject gameState)
        {
            throw new NotImplementedException();
        }

        public virtual BattleEntity GetNextTarget(BattleSceneObject gameState)
        {
            throw new NotImplementedException();
        }
    }
}
