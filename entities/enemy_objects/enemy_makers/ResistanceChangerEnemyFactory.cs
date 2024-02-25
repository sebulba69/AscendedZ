using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class ResistanceChangerEnemyFactory : EnemyFactory
    {
        public ResistanceChangerEnemyFactory()
        {
            _functionDictionary[EnemyNames.Thony] = MakeThony;
            _functionDictionary[EnemyNames.Conson] = MakeConson;
        }

        public Enemy MakeThony()
        {
            string name = EnemyNames.Thony;
            int hp = 12;
            Elements resist1 = Elements.Ice;
            Elements resist2 = Elements.Fir;

            var thony = MakeResistanceChangerEnemy(name, hp, resist1, resist2);

            thony.Skills.Add(SkillDatabase.Ice1.Clone());
            thony.Skills.Add(SkillDatabase.Fire1.Clone());

            return thony;
        }

        public Enemy MakeConson()
        {
            string name = EnemyNames.Conson;
            int hp = 12;
            Elements resist1 = Elements.Light;
            Elements resist2 = Elements.Dark;

            var conson = MakeResistanceChangerEnemy(name, hp, resist1, resist2);

            conson.Skills.Add(SkillDatabase.Dark1.Clone());
            conson.Skills.Add(SkillDatabase.Light1.Clone());

            return conson;
        }

        protected Enemy MakeResistanceChangerEnemy(string name, int hp, Elements resist1, Elements resist2)
        {
            var resistChangerEnemy = new ResistanceChangerEnemy
            {
                Name = $"[RCE] {name}",
                MaxHP = hp,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                Resist1 = resist1,
                Resist2 = resist2
            };

            resistChangerEnemy.Resistances.SetResistance(ResistanceType.Rs, resist1);
            resistChangerEnemy.Resistances.SetResistance(ResistanceType.Wk, resist2);

            return resistChangerEnemy;
        }
    }
}
