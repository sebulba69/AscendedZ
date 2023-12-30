﻿using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class WeaknessHunterEnemyFactory : EnemyFactory
    {
        public WeaknessHunterEnemyFactory()
        {
            _functionDictionary[EnemyNames.ISENALD] = MakeIsenald;
            _functionDictionary[EnemyNames.GARDMUEL] = MakeGardmuel;
            _functionDictionary[EnemyNames.SACHAEL] = MakeSachael;
        }

        public Enemy MakeIsenald()
        {
            var isenald = MakeWeaknessHunterEnemy(EnemyNames.ISENALD, 5);

            isenald.Resistances.CreateResistance(ResistanceType.Wk, Elements.Dark);
            isenald.Resistances.CreateResistance(ResistanceType.Rs, Elements.Light);

            isenald.Skills.Add(SkillDatabase.LIGHT_1);

            return isenald;
        }

        public Enemy MakeGardmuel()
        {
            var gardmuel = MakeWeaknessHunterEnemy(EnemyNames.GARDMUEL, 7);

            gardmuel.Resistances.CreateResistance(ResistanceType.Rs, Elements.Dark);
            gardmuel.Resistances.CreateResistance(ResistanceType.Wk, Elements.Light);

            gardmuel.Skills.Add(SkillDatabase.DARK_1);

            return gardmuel;
        }

        public Enemy MakeSachael()
        {
            var sachael = MakeWeaknessHunterEnemy(EnemyNames.SACHAEL, 6);

            sachael.Resistances.CreateResistance(ResistanceType.Rs, Elements.Fir);
            sachael.Resistances.CreateResistance(ResistanceType.Wk, Elements.Ice);

            sachael.Skills.Add(SkillDatabase.FIRE_1);

            return sachael;
        }

        public Enemy MakeWeaknessHunterEnemy(string name, int hp)
        {
            return new WeaknessHunterEnemy
            {
                Name = name,
                MaxHP = hp,
                Image = EnemyImageAssets.GetEnemyImage(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
