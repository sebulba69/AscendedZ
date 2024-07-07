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
        private BPlayerDC _player;
        private BEnemyDC _enemy;

        public BPlayerDC Player { get => _player; }
        public BEnemyDC Enemy { get => _enemy; }

        public BDCSystem(BPlayerDC player, BEnemyDC enemy)
        {
            _player = player;
            _enemy = enemy;
        }
    }
}
