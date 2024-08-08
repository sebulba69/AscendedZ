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
            _functionDictionary[EnemyNames.Isenald] = MakeIsenald;
            _functionDictionary[EnemyNames.Gardmuel] = MakeGardmuel;
            _functionDictionary[EnemyNames.Sachael] = MakeSachael;
            _functionDictionary[EnemyNames.Pebrand] = MakePebrand;
            _functionDictionary[EnemyNames.Leofuwil] = MakeLeofuwil;
            _functionDictionary[EnemyNames.Gupmoth] = MakeGupmoth;
            _functionDictionary[EnemyNames.Maltamos] = MakeMaltamos;
            _functionDictionary[EnemyNames.Rusnopi] = MakeRusnopi;
            _functionDictionary[EnemyNames.Sufnod] = MakeSufnod;
            _functionDictionary[EnemyNames.Uptali] = MakeUptali;
            _functionDictionary[EnemyNames.Vaphos] = MakeVaphos;
            _functionDictionary[EnemyNames.Ket] = MakeKet;
        }

        public Enemy MakeIsenald()
        {
            var isenald = MakeWeaknessHunterEnemy(EnemyNames.Isenald, 5);

            isenald.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            isenald.Resistances.SetResistance(ResistanceType.Rs, Elements.Light);

            isenald.Skills.Add(SkillDatabase.Light1);

            return isenald;
        }

        public Enemy MakeGardmuel()
        {
            var gardmuel = MakeWeaknessHunterEnemy(EnemyNames.Gardmuel, 7);

            gardmuel.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);
            gardmuel.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            gardmuel.Skills.Add(SkillDatabase.Dark1);

            return gardmuel;
        }

        public Enemy MakeSachael()
        {
            var sachael = MakeWeaknessHunterEnemy(EnemyNames.Sachael, 6);

            sachael.Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);
            sachael.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            sachael.Skills.Add(SkillDatabase.Fire1);

            return sachael;
        }

        public Enemy MakePebrand()
        {
            var pebrand = MakeWeaknessHunterEnemy(EnemyNames.Pebrand, 10);

            pebrand.Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);
            pebrand.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            pebrand.Skills.Add(SkillDatabase.IceAll);

            return pebrand;
        }

        public Enemy MakeLeofuwil()
        {
            var leofuwil = MakeWeaknessHunterEnemy(EnemyNames.Leofuwil, 10);

            leofuwil.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            leofuwil.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            leofuwil.Skills.Add(SkillDatabase.ElecAll);

            return leofuwil;
        }

        public Enemy MakeGupmoth()
        {
            var gupmoth = MakeWeaknessHunterEnemy(EnemyNames.Gupmoth, 20);

            gupmoth.Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            gupmoth.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);

            gupmoth.Skills.Add(SkillDatabase.Fire1.Clone());
            gupmoth.Skills.Add(SkillDatabase.Ice1.Clone());

            return gupmoth;
        }

        public Enemy MakeMaltamos()
        {
            var maltamos = MakeWeaknessHunterEnemy(EnemyNames.Maltamos, 20);

            maltamos.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            maltamos.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            maltamos.Skills.Add(SkillDatabase.WindAll.Clone());

            return maltamos;
        }

        public Enemy MakeRusnopi()
        {
            var rusnopi = MakeWeaknessHunterEnemy(EnemyNames.Rusnopi, 20);

            rusnopi.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            rusnopi.Resistances.SetResistance(ResistanceType.Dr, Elements.Light);

            rusnopi.Skills.Add(SkillDatabase.LightAll.Clone());

            return rusnopi;
        }

        public Enemy MakeUptali()
        {
            var uptali = MakeEvilEyeEnemy(EnemyNames.Uptali, 20, SkillDatabase.BeastEye);

            uptali.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            uptali.Skills.Add(SkillDatabase.Dark1.Clone());

            return uptali;
        }

        public Enemy MakeVaphos()
        {
            var uptali = MakeEvilEyeEnemy(EnemyNames.Vaphos, 20, SkillDatabase.BeastEye);

            uptali.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);

            uptali.Skills.Add(SkillDatabase.Light1.Clone());
            uptali.Skills.Add(SkillDatabase.Wind1.Clone());

            return uptali;
        }

        public Enemy MakeSufnod()
        {
            var sufnod = MakeEvilEyeEnemy(EnemyNames.Sufnod, 20, SkillDatabase.BeastEye);

            sufnod.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            sufnod.Skills.Add(SkillDatabase.Elec1.Clone());

            return sufnod;
        }

        public Enemy MakeKet()
        {
            var ket = MakeEvilEyeEnemy(EnemyNames.Ket, 20, SkillDatabase.BeastEye);

            ket.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);

            ket.Skills.Add(SkillDatabase.Wind1.Clone());

            return ket;
        }

        protected Enemy MakeEvilEyeEnemy(string name, int hp, EyeSkill eyeSkill)
        {
            return new EvilEyeEnemy
            {
                Name = $"[EEYE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                EyeSkill = eyeSkill
            };
        }


        private Enemy MakeWeaknessHunterEnemy(string name, int hp)
        {
            return new WeaknessHunterEnemy
            {
                Name = $"[WEX] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
