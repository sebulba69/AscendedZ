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
        protected bool _isBoss = false;
        public bool IsBoss { get => _isBoss; }

        public Enemy() {}

        public void Boost(int boost)
        {
            HP += boost;
            foreach(ISkill skill in Skills)
            {
                if(skill.Id == SkillId.Elemental)
                {
                    ElementSkill elementalSkill = (ElementSkill)skill;
                    elementalSkill.Damage += ((boost / 4) + 1);
                }
            }
        }

        public virtual void ResetEnemyState()
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
