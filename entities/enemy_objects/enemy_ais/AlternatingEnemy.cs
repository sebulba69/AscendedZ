using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
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
        private Random _rng;
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

        protected ISkill _selectedSkill;
        protected bool _isAgroOverride = false;

        public AlternatingEnemy() : base()
        {
            _rng = new Random();
            Turns = 1;
        }

        /// <summary>
        /// AlternatingEnemies are enemies who ignore the gameState and simply alternates moves
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            _selectedSkill = Skills[CurrentMove++];
            return _selectedSkill;
        }

        /// <summary>
        /// AlternatingEnemy always pick targets at random.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            List<BattlePlayer> partyMembers = battleSceneObject.AlivePlayers;
            BattlePlayer agroStatus = partyMembers.Find(player => { return player.StatusHandler.HasStatus(statuses.StatusId.AgroStatus); });

            // someone has the agro status
            if(agroStatus != null)
            {
                _isAgroOverride = true;
                return agroStatus;
            }
            else
            {
                int i = _rng.Next(partyMembers.Count);
                return partyMembers[i];
            }
        }

        public override void ResetEnemyState()
        {
            CurrentMove = 0;
            _selectedSkill = null;
            _isAgroOverride = false;
        }
    }
}
