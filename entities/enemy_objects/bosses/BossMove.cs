using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class BossMove
    {
        private List<int> _possibleMoveIndexes;

        protected List<int> PossibleMoves { get => _possibleMoveIndexes; }

        public BossMove(List<int> possibleMoveIndexes)
        {
            _possibleMoveIndexes = possibleMoveIndexes;
        }

        public virtual ISkill GetSkillAndSetTarget(BattleSceneObject battleSceneObject, BossScript script, Func<BattleEntity, bool> condition = null)
        {
            // by default, return a random move and target
            int skillIndex = _possibleMoveIndexes[script.RNG.Next(_possibleMoveIndexes.Count)];
            ISkill skill = script.Skills[skillIndex];
            
            if(skill.TargetType == TargetTypes.SINGLE_OPP)
            {
                var alivePlayers = battleSceneObject.Players.FindAll(p => p.HP > 0);
                script.Target = alivePlayers[script.RNG.Next(alivePlayers.Count)];
            }
            else
            {
                var aliveEnemies = battleSceneObject.Enemies.FindAll(e => e.HP > 0);
                script.Target = aliveEnemies[script.RNG.Next(aliveEnemies.Count)];
            }

            return skill;
        }
    }
}
