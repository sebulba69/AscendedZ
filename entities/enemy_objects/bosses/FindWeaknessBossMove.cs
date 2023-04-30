using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class FindWeaknessBossMove : BossMove
    {
        public FindWeaknessBossMove(List<int> possibleMoveIndexes) : base(possibleMoveIndexes)
        {
        }

        public override ISkill GetSkillAndSetTarget(BattleSceneObject battleSceneObject, BossScript script, Func<BattleEntity, bool> condition = null)
        {
            

            ISkill skill = null;
            int index = -1;

            script.PrepTarget(battleSceneObject);

            // if the target was decided on at this point
            if (script.Target != null)
            {
                index = FindPossibleWeakness(script.Target, script);
                if(index < 0)
                {
                    skill = SetSkillManual(battleSceneObject, script);
                }
                else
                {
                    skill = script.Skills[index];
                }
            }
            else
            {
                skill = SetSkillManual(battleSceneObject, script);
            }

            return skill;
        }

        private ISkill SetSkillManual(BattleSceneObject battleSceneObject, BossScript script)
        {
            ISkill skill = null;
            List<int> weakSkills = new List<int>();
            var alivePlayers = battleSceneObject.Players.FindAll(p => p.HP > 0);
            var weakPlayers = alivePlayers.FindAll((p) =>
            {
                bool indexGreaterThanZero = false;
                int index = FindPossibleWeakness(p, script);
                if (index >= 0)
                {
                    weakSkills.Add(index);
                    indexGreaterThanZero = true;
                }
                return indexGreaterThanZero;
            });

            if (weakPlayers.Count > 0)
            {
                int weakSkillIndex = script.RNG.Next(weakPlayers.Count);
                int target = weakSkills[weakSkillIndex];
                skill = script.Skills[target];
                script.Target = weakPlayers[weakSkillIndex];
            }
            return skill;
        }

        private int FindPossibleWeakness(BattleEntity entity, BossScript script)
        {
            int weaknessIndex = -1;
            foreach (int possibleMove in PossibleMoves)
            {
                ElementSkill elementSkill = (ElementSkill)script.Skills[possibleMove];
                if (entity.Resistances.IsWeakToElement(elementSkill.Element))
                {
                    weaknessIndex = possibleMove;
                    break;
                }
            }
            return weaknessIndex;
        }
    }
}
