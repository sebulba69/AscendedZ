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
