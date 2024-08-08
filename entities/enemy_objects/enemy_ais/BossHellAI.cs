using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using static Godot.WebSocketPeer;
using System.Text.Json.Serialization;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// Generic AI for boss battles so I don't have to hard script every single fight.
    /// </summary>
    [JsonDerivedType(typeof(BossHellAI), typeDiscriminator:nameof(BossHellAI))]
    public class BossHellAI : Enemy
    {
        private int _move;
        private int _wexHitCount;

        public BossHellAI() : base()
        {
            _move = 0;
            _wexHitCount = 0;
            _isBoss = true;
        }


        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (result.ResultType == BattleResultType.Wk)
                _wexHitCount++;

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            ISkill skill = Skills[_move];

            EnemyAction action = new EnemyAction() { Skill = skill };

            if (skill.Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)skill, battleSceneObject);
            }
            else if (skill.Id == SkillId.Status) 
            {
                if (!skill.BaseName.Contains("Void"))
                {
                    action.Target = FindTargetForStatus((StatusSkill)skill, battleSceneObject);
                }
                else
                {
                    int voidChance = _rng.Next(1, 101);
                    if (voidChance < 45)
                    {
                        action.Target = FindVoidStatus((StatusSkill)skill, battleSceneObject);
                    }
                    else
                    {
                        action.Target = null;
                    }
                }
                    
            }
            else if (skill.Id == SkillId.Eye)
            {
                if(_wexHitCount == 0)
                {
                    action.Target = null;
                }
                else
                {
                    action.Target = this;
                }
            }

            if(action.Target == null)
            {
                // find the nearest elemental skill
                while (Skills[_move].Id != SkillId.Elemental)
                    IncrementMove();

                action = GetNextAction(battleSceneObject);
            }
            else
            {
                IncrementMove();
            }

            if(skill.Id == SkillId.Eye && _wexHitCount > 0)
            {
                _wexHitCount--;

                if (_move == 0)
                    _move = Skills.Count - 1;
                else
                    _move--;
            }

            return action;
        }

        private BattleEntity FindTargetForStatus(StatusSkill status, BattleSceneObject battleSceneObject)
        {
            if(status.TargetType == TargetTypes.OPP_ALL || status.TargetType == TargetTypes.SINGLE_OPP)
            {
                var players = FindPlayersUnaffectedByStatus(battleSceneObject, status.Status);

                // no reason not to apply buffs/debuffs
                if(status.Status.Id == StatusId.AtkChangeStatus || status.Status.Id == StatusId.DefChangeStatus)
                {
                    return battleSceneObject.AlivePlayers[_rng.Next(battleSceneObject.AlivePlayers.Count)];
                }
                else if (players.Count == 0)
                {
                    return null;
                }
                else
                {
                    if (status.Status.Id == StatusId.StunStatus && players.Count == 1)
                    {
                        return null;
                    }
                    else
                    {
                        return players[_rng.Next(_rng.Next(players.Count))];
                    }   
                }
            }
            else
            {
                return this;
            }
        }

        private BattleEntity FindVoidStatus(StatusSkill skill, BattleSceneObject battleSceneObject)
        {
            var status = skill.Status;

            if(StatusHandler.HasStatus(status.Id) || _wexHitCount < 3)
            {
                return null;
            }
            else
            {
                return this;
            }
        }

        private void IncrementMove()
        {
            _move++;
            if (_move >= Skills.Count)
                _move = 0;
        }

        public override void ResetEnemyState()
        {
            _wexHitCount = 0;
        }
    }
}
