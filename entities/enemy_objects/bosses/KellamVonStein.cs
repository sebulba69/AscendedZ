﻿using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class KellamVonStein : Enemy
    {
        private int _phase;
        private bool _poisonDistributed;
        private int _phase3;

        public KellamVonStein() : base()
        {
            _isBoss = true;

            Name = EnemyNames.Kellam_Von_Stein;
            MaxHP = EntityDatabase.GetBossHP(Name);
            Image = CharacterImageAssets.GetImagePath(Name);

            Resistances = new ResistanceArray();

            Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            Skills.Add(SkillDatabase.Poison.Clone());
            Skills.Add(SkillDatabase.Wind1.Clone());
            Skills.Add(SkillDatabase.DracoTherium.Clone());
            Skills.Add(SkillDatabase.DarkAll.Clone());

            _phase = 0;
            _phase3 = 0;
            Turns = 2;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            switch (_phase)
            {
                case 0:
                    action.Skill = Skills[0];
                    if (_poisonDistributed)
                    {
                        var unpoisoned = battleSceneObject.AlivePlayers.FindAll(p => !p.StatusHandler.HasStatus(statuses.StatusId.PoisonStatus));
                        action.Target = unpoisoned[_rng.Next(unpoisoned.Count)];
                    }
                    else
                    {
                        action.Target = GetRandomAlivePlayer(battleSceneObject);
                    }
                    break;
                case 1:
                    action.Skill = Skills[1];
                    var unpoisonedP2 = battleSceneObject.AlivePlayers.FindAll(p => !p.StatusHandler.HasStatus(statuses.StatusId.PoisonStatus));
                    action.Target = unpoisonedP2[_rng.Next(unpoisonedP2.Count)];
                    break;
                case 2:
                    if(_phase3 == 0)
                    {
                        action.Skill = Skills[2];
                        _phase3++;
                    }
                    else
                    {
                        action.Skill = Skills[3];
                    }
                    break;
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _phase++;
            if(_phase >= 3)
            {
                _poisonDistributed = false;
                _phase = 0;
                _phase3 = 0;
            }
                
        }
    }
}
