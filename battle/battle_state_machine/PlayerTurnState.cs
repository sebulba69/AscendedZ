using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;

namespace AscendedZ.battle.battle_state_machine
{
    /// <summary>
    /// This state handles all activities that take place during the Player's turn.
    /// </summary>
    public class PlayerTurnState : IBattleState
    {
        private int _activePlayer;

        public void StartState(BattleSceneObject battleSceneObject)
        {
            var players = battleSceneObject.Players;
            _activePlayer = players.FindIndex(p => p.HP > 0);

            var active = players[_activePlayer];
            active.IsActive = true;

            battleSceneObject.ActivePlayer = active;
            battleSceneObject.SkillSelected += _OnSkillSelected;
        }

        private void _OnSkillSelected(object sender, PlayerTargetSelectedEventArgs eventArgs)
        {
            BattleSceneObject battleSceneObject = sender as BattleSceneObject;
            
            var players = battleSceneObject.Players;
            var active = players[_activePlayer];

            BattleResult result = default(BattleResult);

            ISkill skill = active.Skills[eventArgs.SkillIndex];
            // result cannot be null at the end of this function
            switch (skill.TargetType)
            {
                case TargetTypes.SINGLE_OPP:
                    // our only available targets are alive enemies
                    var possibleTargets = battleSceneObject.Enemies.FindAll(e => e.HP > 0);
                    var enemy = possibleTargets[eventArgs.TargetIndex];
                    result = skill.ProcessSkill(enemy);
                    break;
                case TargetTypes.SINGLE_TEAM:
                    var player = players[eventArgs.TargetIndex];
                    result = skill.ProcessSkill(player);
                    break;
            }

            result.User = active;
            battleSceneObject.HandlePostTurnProcessing(result);

            do
            {
                _activePlayer++;
                if (_activePlayer == players.Count)
                    _activePlayer = 0;

                // skip if nextActive cannot attack
                var nextActive = players[_activePlayer];
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

            } while (players[_activePlayer].HP == 0 || !players[_activePlayer].CanAttack);
            
            battleSceneObject.ActivePlayer = players[_activePlayer];
        }

        public void EndState(BattleSceneObject battleSceneObject)
        {
            battleSceneObject.SkillSelected -= _OnSkillSelected;
        }
    }
}
