using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.skills;
using AscendedZ.entities.battle_entities;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class EyeEnemy : AlternatingEnemy
    {
        private bool _useEye;
        public ISkill EyeSkill { get; set; }

        public EyeEnemy()
        {
            Turns = 1;
            Description = "[EYE] Will increase enemy turns if its weakness is hit. Otherwise, it attacks randomly.";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            _useEye = (result.ResultType == BattleResultType.Wk);

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            var action = base.GetNextAction(battleSceneObject);

            if (_useEye) 
            {
                action.Target = null;
                action.Skill = EyeSkill;
                _useEye = false;
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            base.ResetEnemyState();
            _useEye = false;
        }
    }
}
