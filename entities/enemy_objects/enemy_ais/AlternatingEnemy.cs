﻿using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// An enemy who targets the lowest HP party member and alternates its moves throughout a turn.
    /// </summary>
    public class AlternatingEnemy : Enemy
    {
        protected int _currentMove = 0;
        protected int CurrentMove
        {
            get
            {
                return _currentMove;
            }
            set
            {
                _currentMove = value;
                if (_currentMove == Skills.Count)
                    _currentMove = 0;
            }
        }

        protected bool _isAgroOverride = false;

        public AlternatingEnemy() : base()
        {
            Turns = 1;
            Description = $"Class: Alternating Enemy\nDescription: Randomly picks targets for an attack.\nIt will alternate through each of its skills at least once.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            ISkill skill = Skills[CurrentMove++];
            BattleEntity target;

            List<BattlePlayer> partyMembers = battleSceneObject.AlivePlayers;
            BattlePlayer agroStatus = partyMembers.Find(player => { return player.StatusHandler.HasStatus(statuses.StatusId.AgroStatus); });

            // someone has the agro status
            if (agroStatus != null)
            {
                _isAgroOverride = true;
                target = agroStatus;
            }
            else
            {
                int i = _rng.Next(partyMembers.Count);
                target = partyMembers[i];
            }

            return new EnemyAction 
            { 
                Skill = skill,
                Target = target
            };
        }

        public override void ResetEnemyState()
        {
            CurrentMove = 0;
            _isAgroOverride = false;
        }
    }
}
