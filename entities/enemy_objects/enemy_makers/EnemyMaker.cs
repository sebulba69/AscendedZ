﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class EnemyMaker
    {
        private List<EnemyFactory> _enemyFactories;

        public EnemyMaker()
        {
            _enemyFactories = new List<EnemyFactory> 
            {
                new AlternatingEnemyFactory(),
                new UniqueEnemyFactory(),
                new StatusAttackEnemyFactory(),
                new WeaknessHunterEnemyFactory(),
                new ProtectorEnemyFactory(),
                new BuffEnemyFactory()
            };
        }

        public Enemy MakeEnemy(string name)
        {
            Enemy enemy = null;

            int i = 0;
            while(enemy == null)
            {
                if (i == _enemyFactories.Count)
                    throw new Exception("Enemy not able to be generated by our factories");

                enemy = _enemyFactories[i++].GetEnemyByName(name);
            }

            return enemy;
        }
    }
}
