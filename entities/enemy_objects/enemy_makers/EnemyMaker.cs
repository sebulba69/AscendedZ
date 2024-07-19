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
                new BuffEnemyFactory(),
                new ResistanceChangerEnemyFactory(),
                new CopyCatEnemyFactory()
            };
        }

        public Enemy MakeEnemy(string name, int tier)
        {
            Enemy enemy = null;

            int i = 0;
            while(enemy == null)
            {
                if (i == _enemyFactories.Count)
                    throw new Exception("Enemy not able to be generated by our factories");

                var factory = _enemyFactories[i++];
                factory.SetTier(tier);
                enemy = factory.GetEnemyByName(name);
            }

            return enemy;
        }
    }
}
