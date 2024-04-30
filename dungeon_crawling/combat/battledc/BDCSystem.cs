using AscendedZ.dungeon_crawling.combat.skillsdc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public class BDCSystem
    {
        private Queue<SkillDC> _moveQ;
        private BPlayerDC _player;
        private BEnemyDC _enemy;

        public BPlayerDC Player { get => _player; }
        public BEnemyDC Enemy { get => _enemy; }

        public BDCSystem(BPlayerDC player, BEnemyDC enemy)
        {
            _player = player;
            _enemy = enemy;
            _moveQ = new Queue<SkillDC>();
        }

        public void QueueMove(SkillDC skill)
        {
            _moveQ.Enqueue(skill);
        }
    }
}
