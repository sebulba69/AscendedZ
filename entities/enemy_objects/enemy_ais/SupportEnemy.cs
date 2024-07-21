using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class SupportEnemy : AlternatingEnemy
    {
        private ISkill _heal;

        public SupportEnemy() : base()
        {
            Turns = 2;
            _heal = SkillDatabase.Heal1.Clone();
            Description = $"[SPRT]: Will always heal any enemy whose health is 50% or below. Will do normal attacks otherwise.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            var action = base.GetNextAction(battleSceneObject);

            var enemies = battleSceneObject.AliveEnemies;
            foreach (var enemy in enemies) 
            {
                int hpPercentage = (int)((enemy.HP / (double)enemy.MaxHP) * 100);
                if(hpPercentage <= 50)
                {
                    action = new EnemyAction()
                    {
                        Target = enemy,
                        Skill = _heal
                    };
                    break;
                }
            }

            return action;
        }
    }
}
