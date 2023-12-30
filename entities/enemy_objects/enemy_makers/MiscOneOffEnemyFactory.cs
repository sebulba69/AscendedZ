﻿using AscendedZ.entities.enemy_objects.bosses;
using AscendedZ.entities.enemy_objects.enemy_ais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    /// <summary>
    /// A factory for unique enemies with 1-off appearances.
    /// </summary>
    public class UniqueEnemyFactory : EnemyFactory
    {
        public UniqueEnemyFactory()
        {
            _functionDictionary[EnemyNames.CATTUTDRONI] = MakeCattuTDroni;
            _functionDictionary[EnemyNames.HARBINGER] = MakeHarbinger;
        }

        /// <summary>
        /// Mini-bosses.
        /// </summary>
        /// <returns></returns>
        public Enemy MakeCattuTDroni()
        {
            return new CattuTDroni();
        }

        public Enemy MakeHarbinger()
        {
            return new Harbinger();
        }
    }
}
