using AscendedZ.entities.enemy_objects.enemy_ais;
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

            pebrand.Skills.Add(SkillDatabase.IceAll);

            return pebrand;
        }

        public Enemy MakeLeofuwil()
        {
            var leofuwil = MakeWeaknessHunterEnemy(EnemyNames.Leofuwil, 10);

            leofuwil.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);

            leofuwil.Skills.Add(SkillDatabase.ElecAll);

            return leofuwil;
        }

        private Enemy MakeWeaknessHunterEnemy(string name, int hp)
        {
            return new WeaknessHunterEnemy
            {
                Name = $"[WEX] {name}",
                MaxHP = hp,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
