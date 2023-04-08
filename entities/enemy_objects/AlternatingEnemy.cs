﻿using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects
{
    /// <summary>
    /// An enemy who targets the lowest HP party member and alternates its moves throughout a turn.
    /// </summary>
    public class AlternatingEnemy : Enemy
    {
        private Random _rng;
        private int _currentMove = 0;
        private int CurrentMove
        {
            get
            {
                return _currentMove;
            }
            set
            {
                _currentMove = value;
                if (_currentMove == this.Skills.Count)
                    _currentMove = 0;
            }
        }

        public AlternatingEnemy() : base() 
        {
            _rng = new Random();
            this.Turns = 1;
        }

        /// <summary>
        /// AlternatingEnemies are enemies who ignore the gameState and simply alternates moves
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            return this.Skills[CurrentMove++];
        }

        /// <summary>
        /// AlternatingEnemy always pick targets at random.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            var partyMembers = battleSceneObject.Players.FindAll(member => member.HP > 0);
            int i = _rng.Next(partyMembers.Count);
            return partyMembers[i];
        }

        public override void ResetEnemyState()
        {
            CurrentMove = 0;
        }

    }
}
