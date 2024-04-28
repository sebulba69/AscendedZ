using AscendedZ.dungeon_crawling.combat;
using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.databases
{
    public class EnemyDCFactory
    {
        private int _tier;
        private Random _rng;

        public EnemyDCFactory(int tier, Random rng)
        {
            _tier = tier;
            _rng = rng;
        }

        public BEnemyDC MakeEnemy()
        {
            BEnemyDC enemy = new BEnemyDC(_tier, MakeEnemyStats());

            if(_tier < 10)
            {
                List<string> tier1_9Images = new List<string>() 
                {
                    EnemyNames.Anrol,
                    EnemyNames.Conlen,
                    EnemyNames.David,
                    EnemyNames.Nanfrea,
                    EnemyNames.CattuTDroni
                };

                enemy.Image = CharacterImageAssets.GetImagePath(tier1_9Images[_rng.Next(tier1_9Images.Count)]);
            }

            return enemy;
        }

        private StatsDC MakeEnemyStats()
        {
            StatsDC statsDC = new StatsDC() 
            {
                HP = 10,
                MP = 999
            };

            for (int l = 1; l < _tier; l++)
                statsDC.LevelUp();

            return statsDC;
        }
    }
}
