using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;

namespace AscendedZ.battle.battle_state_machine
{
    public class EnemyTurnState : IBattleState
    {
        private List<Enemy> _enemies;
        private int _activeEnemy;

        public void StartState(BattleSceneObject battleSceneObject)
        {
            _enemies = battleSceneObject.Enemies;
            _activeEnemy = _enemies.FindIndex(enemy => enemy.HP > 0);

            battleSceneObject.MakeEnemyDoTurn += _OnDoTurnRequest;
        }

        public void _OnDoTurnRequest(object sender, EventArgs e)
        {
            BattleSceneObject battleSceneObject = sender as BattleSceneObject;
            var active = _enemies[_activeEnemy];

            BattleResult result = default(BattleResult);
            ISkill skill = active.GetNextMove(battleSceneObject);
            BattleEntity target = active.GetNextTarget(battleSceneObject);

            switch (skill.TargetType)
            {
                case TargetTypes.SINGLE_OPP:
                case TargetTypes.SINGLE_TEAM:
                    result = skill.ProcessSkill(target);
                    break;
            }

            result.User = active;
            battleSceneObject.HandlePostTurnProcessing(result);
        }

        public void ChangeActiveEntity(BattleSceneObject battleSceneObject)
        {
            do
            {
                _activeEnemy++;
                if (_activeEnemy == _enemies.Count)
                    _activeEnemy = 0;

                var nextActive = _enemies[_activeEnemy];
                if (!nextActive.CanAttack)
                {
                    BattleResult skipResult = new BattleResult()
                    {
                        Target = null,
                        User = nextActive,
                        ResultType = BattleResultType.StatusApplied
                    };

                    battleSceneObject.HandlePostTurnProcessing(skipResult);
                }
            } while (_enemies[_activeEnemy].HP == 0 || !_enemies[_activeEnemy].CanAttack);
        }

        public void EndState(BattleSceneObject battleSceneObject) 
        {
            _activeEnemy = 0;
            battleSceneObject.MakeEnemyDoTurn -= _OnDoTurnRequest;
            foreach (var enemy in _enemies)
                enemy.ResetEnemyState();
        }
    }
}
