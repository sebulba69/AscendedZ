using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class AlternatingEnemyFactory: EnemyFactory
    {
        public AlternatingEnemyFactory()
        {
            _functionDictionary[EnemyNames.Conlen] = MakeConlen;
            _functionDictionary[EnemyNames.Orachar] = MakeOrachar;
            _functionDictionary[EnemyNames.Fastrobren] = MakeFastrobren;
            _functionDictionary[EnemyNames.Liamlas] = MakeLiamlas;
        }

        public Enemy MakeConlen()
        {
            var conlen = MakeAlternatingEnemy(EnemyNames.Conlen, 6);

            conlen.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            conlen.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            conlen.Skills.Add(SkillDatabase.Elec1.Clone());
            conlen.Skills.Add(SkillDatabase.Fire1.Clone());

            return conlen;
        }

        public Enemy MakeOrachar()
        {
            var orachar = MakeAlternatingEnemy(EnemyNames.Orachar, 6);

            orachar.Resistances.SetResistance(ResistanceType.Wk, Elements.Fir);

            orachar.Skills.Add(SkillDatabase.Ice1.Clone());

            return orachar;
        }

        public Enemy MakeFastrobren()
        {
            var fastrobren = MakeAlternatingEnemy(EnemyNames.Fastrobren, 4);

            fastrobren.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            fastrobren.Skills.Add(SkillDatabase.Dark1.Clone());

            return fastrobren;
        }

        public Enemy MakeLiamlas()
        {
            var liamlas = MakeAlternatingEnemy(EnemyNames.Liamlas, 6);
            
            liamlas.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);

            liamlas.Skills.Add(SkillDatabase.Light1.Clone());

            return liamlas;
        }

        protected Enemy MakeAlternatingEnemy(string name, int hp)
        {
            return new AlternatingEnemy
            {
                Name = $"[AE] {name}",
                MaxHP = hp,
                Image = CharacterImageAssets.GetImage(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
