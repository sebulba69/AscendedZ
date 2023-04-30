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
    /// <summary>
    /// Assumes the skill being passed in is a status skill.
    /// Checks for anyone who doesn't have the status before applying it.
    /// </summary>
    public class FindMissingStatusBossMove : BossMove
    {
        public FindMissingStatusBossMove(List<int> possibleMoveIndexes) : base(possibleMoveIndexes)
        {
            if (possibleMoveIndexes.Count > 1)
                throw new Exception("This move only applies 1 status at a time where possible");
        }

        public override ISkill GetSkillAndSetTarget(BattleSceneObject battleSceneObject, BossScript script, 
            Func<BattleEntity, bool> condition = null)
        {
            var skillsToChooseFrom = script.Skills;
            int statusIndex = PossibleMoves[0];
            StatusSkill statusSkill = (StatusSkill)skillsToChooseFrom[statusIndex];

            var aliveMembers = battleSceneObject.Players.FindAll(p => p.HP > 0);
            var nonStatusAfflictedUsers = aliveMembers.FindAll(p => !p.StatusHandler.HasStatus(statusSkill.Status.Id));

            if(condition != null)
            {
                var availableWexUsers = nonStatusAfflictedUsers.FindAll(nsau => condition(nsau));
                if (availableWexUsers.Count > 0)
                {
                    script.Target = availableWexUsers[script.RNG.Next(availableWexUsers.Count)];
                }
                else
                {
                    // return null, we don't want to use the skill nor do we have a target
                    statusSkill = null;
                }
            }

            return statusSkill;
        }

    }
}
