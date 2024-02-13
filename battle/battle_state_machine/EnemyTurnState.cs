using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.skills;
using Godot;
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
        private bool _firstAction = true;

        public void StartState(BattleSceneObject battleSceneObject)
        {
            _firstAction = true;

            _enemies = battleSceneObject.Enemies;
            _activeEnemy = _enemies.FindIndex(enemy => enemy.HP > 0);

            battleSceneObject.MakeEnemyDoTurn += _OnDoTurnRequest;
            battleSceneObject.StartEnemyDoTurn?.Invoke(this, EventArgs.Empty);
        }

        public void _OnDoTurnRequest(object sender, EventArgs e)
        {
            BattleSceneObject battleSceneObject = sender as BattleSceneObject;

            if (_firstAction)
            {
                _firstAction = false;
                battleSceneObject.PostUIUpdate();
                return;
            }
            
            var active = _enemies[_activeEnemy];

            BattleResult result = default(BattleResult);

            EnemyAction action = active.GetNextAction(battleSceneObject);

            ISkill skill = action.Skill;
            BattleEntity target = action.Target;

            StringBuilder logEntry = new StringBuilder();

            logEntry.Append($"[color=coral]{active.Name}[/color] used [color=burlywood]{skill.Name}[/color] on [color=gold]{target.Name}[/color]. ");

            switch (skill.TargetType)
            {
                case TargetTypes.SINGLE_OPP:
                case TargetTypes.SINGLE_TEAM:
                    result = skill.ProcessSkill(target);
                    break;
            }

            result.User = active;

            logEntry.Append(result.Log.ToString());

            result.Log = logEntry;

            battleSceneObject.HandlePostTurnProcessing(result);
        }

        public void ChangeActiveEntity(BattleSceneObject battleSceneObject)
        {
            do
            {
                _activeEnemy++;
                if (_activeEnemy == _enemies.Count)
                    _activeEnemy = 0;

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
