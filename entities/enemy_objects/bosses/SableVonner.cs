﻿using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.WebSocketPeer;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class SableVonner : Enemy
    {
        private bool _weaknessHit;
        private bool _isElec;

        private const int W_FIRE = 0;
        private const int W_ELEC = 1;
        private const int V_WIND = 2;

        private const int ELEC_BUFF = 3;
        private const int FIRE_BUFF = 4;

        private const int ELEC = 5;
        private const int FIRE = 6;

        private int _phase;

        public SableVonner() : base()
        {
            _isBoss = true;
            _weaknessHit = false;
            _isElec = true;
            _phase = 0;

            Name = EnemyNames.Sable_Vonner;
            Image = CharacterImageAssets.GetImage(Name);
            MaxHP = 150;
            Turns = 4;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            Skills.Add(SkillDatabase.WeakFire.Clone());
            Skills.Add(SkillDatabase.WeakElec.Clone());
            Skills.Add(SkillDatabase.VoidWind.Clone());

            Skills.Add(SkillDatabase.ElecBuff1.Clone());
            Skills.Add(SkillDatabase.FireBuff1.Clone());

            Skills.Add(SkillDatabase.Elec1.Clone());
            Skills.Add(SkillDatabase.Fire1.Clone());
        }

        public override BattleResult ApplyElementSkill(ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(skill);

            if (result.ResultType == BattleResultType.Wk)
                _weaknessHit = true;

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            switch (_phase)
            {
                case 0:
                    if (_weaknessHit)
                    {
                        action.Skill = Skills[V_WIND];
                        action.Target = this;
                    }
                    else
                    {
                        action.Skill = GetWex();
                        action.Target = GetRandomPlayer(battleSceneObject);
                    }
                    break;
                case 1:
                    action.Skill = GetBuff();
                    action.Target = this;
                    break;
                case 2:
                case 3:
                    Elements wex = (_isElec) ? Elements.Elec : Elements.Fir;
                    action.Skill = GetDamage();
                    action.Target = GetPlayerWithWexTo(battleSceneObject, wex);
                    break;
                default:
                    _phase = 1;
                    _isElec = !_isElec;
                    action = GetNextAction(battleSceneObject);
                    break;
            }

            _phase++;
            return action;
        }

        private ISkill GetWex()
        {
            if (_isElec)
                return Skills[W_ELEC];
            else
                return Skills[W_FIRE];
        }

        private ISkill GetBuff()
        {
            if (_isElec)
                return Skills[ELEC_BUFF];
            else
                return Skills[FIRE_BUFF];
        }

        private ISkill GetDamage()
        {
            if (_isElec)
                return Skills[ELEC];
            else
                return Skills[FIRE];
        }

        private BattleEntity GetRandomPlayer(BattleSceneObject battleSceneObject)
        {
            var players = battleSceneObject.AlivePlayers;
            return players[_rng.Next(0, players.Count)];
        }

        private BattleEntity GetPlayerWithWexTo(BattleSceneObject battleSceneObject, Elements wexElement)
        {
            var players = battleSceneObject.AlivePlayers;
            var wexToElement = players.FindAll(player => player.Resistances.IsWeakToElement(wexElement));
            if (wexToElement.Count == 0)
            {
                return GetRandomPlayer(battleSceneObject);
            }
            else
            {
                var status = wexToElement.Find(wexPlayer => wexPlayer.StatusHandler.HasStatus(statuses.StatusId.WexElementStatus));
                if (status == null)
                    return wexToElement[_rng.Next(0, wexToElement.Count)];
                else
                    return status;
            }
        }

        public override void ResetEnemyState()
        {
            _weaknessHit = false;
            _phase = 0;
            _isElec = !_isElec;
        }
    }
}
