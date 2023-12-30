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
            _functionDictionary[EnemyNames.CONLEN] = MakeConlen;
            _functionDictionary[EnemyNames.ORAHCAR] = MakeOrachar;
            _functionDictionary[EnemyNames.FASTROBREN] = MakeFastrobren;
            _functionDictionary[EnemyNames.LIAMLAS] = MakeLiamlas;
        }

        public Enemy MakeConlen()
        {
            var conlen = MakeAlternatingEnemy(EnemyNames.CONLEN, 6);

            conlen.Resistances.CreateResistance(ResistanceType.Wk, Elements.Wind);
            conlen.Resistances.CreateResistance(ResistanceType.Wk, Elements.Ice);

            conlen.Skills.Add(SkillDatabase.ELEC_1.Clone());
            conlen.Skills.Add(SkillDatabase.FIRE_1.Clone());

            return conlen;
        }

        public Enemy MakeOrachar()
        {
            var orachar = MakeAlternatingEnemy(EnemyNames.ORAHCAR, 6);

            orachar.Resistances.CreateResistance(ResistanceType.Wk, Elements.Fir);

            orachar.Skills.Add(SkillDatabase.ICE_1.Clone());

            return orachar;
        }

        public Enemy MakeFastrobren()
        {
            var fastrobren = MakeAlternatingEnemy(EnemyNames.FASTROBREN, 4);

            fastrobren.Resistances = new ResistanceArray();
            fastrobren.Resistances.CreateResistance(ResistanceType.Wk, Elements.Light);

            fastrobren.Skills.Add(SkillDatabase.DARK_1.Clone());

            return fastrobren;
        }

        public Enemy MakeLiamlas()
        {
            var liamlas = MakeAlternatingEnemy(EnemyNames.LIAMLAS, 6);
            
            liamlas.Resistances = new ResistanceArray();
            liamlas.Resistances.CreateResistance(ResistanceType.Wk, Elements.Dark);

            liamlas.Skills.Add(SkillDatabase.LIGHT_1.Clone());

            return liamlas;
        }

        protected Enemy MakeAlternatingEnemy(string name, int hp)
        {
            return new AlternatingEnemy
            {
                Name = name,
                MaxHP = hp,
                Image = EnemyImageAssets.GetEnemyImage(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
