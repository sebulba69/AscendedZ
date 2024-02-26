using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class CopyCatEnemyFactory : EnemyFactory
    {
        public CopyCatEnemyFactory()
        {
            _functionDictionary[EnemyNames.Naldbear] = MakeNaldbear;
            _functionDictionary[EnemyNames.Stroma_Hele] = MakeStromaHele;
        }
        public Enemy MakeNaldbear()
        {
            string name = EnemyNames.Naldbear;
            int hp = 12;
            var naldbear = MakeCopyCatEnemy(name, hp);

            naldbear.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);

            naldbear.Skills.Add(SkillDatabase.Elec1.Clone());

            return naldbear;
        }

        public Enemy MakeStromaHele()
        {
            string name = EnemyNames.Stroma_Hele;
            int hp = 12;
            var stroma = MakeCopyCatEnemy(name, hp);

            stroma.Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);

            stroma.Skills.Add(SkillDatabase.Fire1.Clone());

            return stroma;
        }

        protected Enemy MakeCopyCatEnemy(string name, int hp)
        {
            return new CopyCatEnemy
            {
                Name = $"[CC] {name}",
                MaxHP = hp,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
