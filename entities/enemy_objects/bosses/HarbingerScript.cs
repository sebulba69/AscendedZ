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
    /// PHASE 1: STUN, HIT_WEAK, BUFF
    /// PHASE 2: HIT_WEAK, BUFF, BUFF
    /// PHASE 3: FIRE, DEFAULT, DEFAULT
    /// </summary>
    public class HarbingerScript : BossScript
    {
        private const int ELEC = 0;
        private const int ICE = 1;
        private const int STUN = 2;
        private const int DARK = 3;
        private const int DARK_BUFF = 4;

        public HarbingerScript(List<ISkill> skills) : base(skills)
        {
            _skillScriptIndexes = new List<List<BossMove>>();
            var potentialWeaknesses = new FindWeaknessBossMove(new List<int>() { ELEC, ICE });
            var randomElementalMove = new BossMove(new List<int>() { ELEC, ICE, DARK });
            var darkOnly = new FindWeaknessBossMove(new List<int>() { DARK });
            var darkBuff = new BossMove(new List<int>() { DARK_BUFF });
            var stun = new FindMissingStatusBossMove(new List<int>() { STUN });

            var phase1 = new List<BossMove>() { stun, potentialWeaknesses, potentialWeaknesses };
            var phase2 = new List<BossMove>() { potentialWeaknesses, darkBuff, darkBuff };
            var phase3 = new List<BossMove>() { darkOnly, stun, potentialWeaknesses, potentialWeaknesses };
            var defaultPhase = new List<BossMove>() { randomElementalMove, randomElementalMove, randomElementalMove };

            _skillScriptIndexes.Add(phase1);
            _skillScriptIndexes.Add(phase2);
            _skillScriptIndexes.Add(phase3);
            _defaultScript = defaultPhase;

            _maxPhase = _skillScriptIndexes.Count;
            _maxMoves = 4;
        }

        public override void PrepTarget(BattleSceneObject battleSceneObject)
        {
            var stunnedPlayer = 
                battleSceneObject.Players.Find(p =>
                {
                    bool hasStatusAndIsActive = false;
                    foreach(var status in p.StatusHandler.Statuses)
                    {
                        hasStatusAndIsActive = (status.Id == statuses.StatusId.StunStatus) && (!status.Active);
                        if (hasStatusAndIsActive)
                            break;
                    }
                    return hasStatusAndIsActive; 
                });
            this.Target = stunnedPlayer;
        }

        public override ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            var currentPhase = _skillScriptIndexes[_currentPhase];

            ISkill skill = null;

            if (_currentMove >= currentPhase.Count)
            {
                if (_currentMove == _defaultScript.Count)
                    _currentMove = _defaultScript.Count - 1;

                skill = _defaultScript[_currentMove].GetSkillAndSetTarget(battleSceneObject, this);
            }
            else
            {
                var currentMove = currentPhase[_currentMove];

                skill = currentMove.GetSkillAndSetTarget(battleSceneObject, this, IsEntityWeakToElements);
                if (skill == null)
                {
                    // default script for Harbinger does NOT contain any statuses, so no need to check
                    skill = _defaultScript[_currentMove].GetSkillAndSetTarget(battleSceneObject, this);
                }
            }

            base.IncrementCurrentMove();
            return skill;
        }

        private bool IsEntityWeakToElements(BattleEntity target)
        {
            List<ElementSkill> elementSkills = new List<ElementSkill>()
                {
                    (ElementSkill)this.Skills[ELEC],
                    (ElementSkill)this.Skills[ICE]
                };

            bool hasWeakness = false;
            foreach (var elementSkill in elementSkills)
            {
                if (target.Resistances.IsWeakToElement(elementSkill.Element))
                {
                    hasWeakness = true;
                    break;
                }
            }
            return hasWeakness;
        }
    }
}
