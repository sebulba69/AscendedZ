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
            _functionDictionary[EnemyNames.CattuTDroni] = MakeCattuTDroni;
            _functionDictionary[EnemyNames.Harbinger] = MakeHarbinger;
            _functionDictionary[EnemyNames.Elliot_Onyx] = MakeElliot;
            _functionDictionary[EnemyNames.Sable_Vonner] = MakeVonner;
            _functionDictionary[EnemyNames.Cloven_Umbra] = MakeUmbra;
            _functionDictionary[EnemyNames.Ashen_Ash] = MakeAshen;
        }

        /// <summary>
        /// Mini-bosses.
        /// </summary>
        /// <returns></returns>
        public Enemy MakeCattuTDroni()
        {
            Enemy droni = new CattuTDroni();
            droni.Name = $"[STN] {droni.Name}";
            return droni;
        }

        public Enemy MakeHarbinger()
        {
            return new Harbinger();
        }

        public Enemy MakeElliot()
        {
            return new ElliotOnyx();
        }

        public Enemy MakeVonner()
        {
            return new SableVonner();
        }

        public Enemy MakeUmbra()
        {
            return new ClovenUmbra();
        }

        public Enemy MakeAshen()
        {
            return new AshenAsh();
        }
    }
}
