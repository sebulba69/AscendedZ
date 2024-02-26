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
            _functionDictionary[EnemyNames.Fledan] = MakeFledan;
            _functionDictionary[EnemyNames.Walds] = MakeWalds;
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

            orachar.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

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

        public Enemy MakeFledan()
        {
            var fledan = MakeAlternatingEnemy(EnemyNames.Fledan, 10);

            fledan.Resistances.SetResistance(ResistanceType.Rs, Elements.Light);
            fledan.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);

            fledan.Skills.Add(SkillDatabase.Light1.Clone());
            fledan.Skills.Add(SkillDatabase.Ice1.Clone());

            return fledan;
        }

        public Enemy MakeWalds()
        {
            var walds = MakeAlternatingEnemy(EnemyNames.Walds, 10);

            walds.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);
            walds.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);

            walds.Skills.Add(SkillDatabase.Dark1.Clone());
            walds.Skills.Add(SkillDatabase.Wind1.Clone());

            return walds;
        }

        protected Enemy MakeAlternatingEnemy(string name, int hp)
        {
            return new AlternatingEnemy
            {
                Name = $"[AE] {name}",
                MaxHP = hp,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
