using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class CopyCatEnemy : AlternatingEnemy
    {
        private BattleResult _lastUsedSkill;

        public CopyCatEnemy() : base()
        {
            Description = "[CC]: Copy Cat Enemy. Will always copy the last action the player who attacked it took.";
        }

        public override BattleResult ApplyElementSkill(ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(skill);

            _lastUsedSkill = result;

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = base.GetNextAction(battleSceneObject);

            if (!_isAgroOverride)
                action.Target = _lastUsedSkill.User;

            if(_lastUsedSkill != null)
                action.Skill = _lastUsedSkill.SkillUsed.Clone();

            return action;
        }

        public override void ResetEnemyState()
        {
            _lastUsedSkill = null;

            base.ResetEnemyState();
        }
    }
}
