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
    public class EvilEyeEnemy : WeaknessHunterEnemy
    {
        private bool _usedEye;
        public ISkill EyeSkill { get; set; }

        public EvilEyeEnemy()
        {
            Turns = 1;
            Description = "[EEYE] Will always cast an eye skill as its first\nmove, then will focus on weaknesses.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            var action = base.GetNextAction(battleSceneObject);

            if (!_usedEye)
            {
                action.Target = null;
                action.Skill = EyeSkill;
                _usedEye = true;
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            base.ResetEnemyState();
            _usedEye = false;
        }
    }
}
