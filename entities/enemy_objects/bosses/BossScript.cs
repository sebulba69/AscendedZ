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
    /// <summary>
    /// The code for scripting certain bosses is becoming so complicated that this class
    /// exists solely to help me write out future boss scripts. This doesn't have to be used
    /// all the time, but it'll be extremely helpful.
    /// </summary>
    public class BossScript
    {
        protected int _maxPhase;
        protected int _maxMoves;
        protected int _currentPhase;
        protected int _currentMove; // the current move we're using in our script
        
        /// <summary>
        /// Skills to choose from.
        /// </summary>
        protected List<ISkill> _skills;

        /// <summary>
        /// Indexes of each skill we're supposed to use sequentially
        /// </summary>
        protected List<List<BossMove>> _skillScriptIndexes;

        /// <summary>
        /// Our default set of skills we'll use if we can't use script indexes
        /// </summary>
        protected List<BossMove> _defaultScript;

        protected Random _rng;

        public Random RNG { get => _rng; }
        public List<ISkill> Skills { get => _skills; }
        public BattleEntity Target { set; get; }

        public BossScript(List<ISkill> skills)
        {
            _skills = new List<ISkill>();
            foreach (var skill in skills)
                _skills.Add(skill);
            
            _currentPhase = 0;
            _currentMove = 0;
            _rng = new Random();
        }

        protected virtual void IncrementCurrentMove()
        {
            _currentMove++;
            if (_currentMove == _maxMoves)
            {
                _currentMove = 0;
            }
        }

        public virtual void PrepTarget(BattleSceneObject battleSceneObject)
        {
            // do nothing
        }

        public virtual void ResetEnemyMove()
        {
            _currentMove = 0;
            _currentPhase++;
            if (_currentPhase == _maxPhase)
                _currentPhase = 0;
        }

        public virtual ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            throw new NotImplementedException();
        }
    }
}
