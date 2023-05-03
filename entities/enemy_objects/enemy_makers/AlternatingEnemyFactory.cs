using AscendedZ.entities.enemy_objects.alt_enemies;
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
            var conlen = MakeAlternatingEnemy("Conlen", 6, "res://enemy_pics/newpicture49.png");

            conlen.Resistances.CreateResistance(ResistanceType.Wk, Elements.Wind);
            conlen.Resistances.CreateResistance(ResistanceType.Wk, Elements.Ice);

            conlen.Skills.Add(SkillDatabase.ELEC_1.Clone());
            conlen.Skills.Add(SkillDatabase.FIRE_1.Clone());

            return conlen;
        }

        public Enemy MakeOrachar()
        {
            var orachar = MakeAlternatingEnemy("Orahcar", 6, "res://enemy_pics/newpicture52.png");

            orachar.Resistances.CreateResistance(ResistanceType.Wk, Elements.Fir);

            orachar.Skills.Add(SkillDatabase.ICE_1.Clone());

            return orachar;
        }

        public Enemy MakeFastrobren()
        {
            var fastrobren = MakeAlternatingEnemy("Fastrobren", 4, "res://enemy_pics/newpicture40.png");

            fastrobren.Resistances = new ResistanceArray();
            fastrobren.Resistances.CreateResistance(ResistanceType.Wk, Elements.Light);

            fastrobren.Skills.Add(SkillDatabase.DARK_1.Clone());

            return fastrobren;
        }

        public Enemy MakeLiamlas()
        {
            var liamlas = MakeAlternatingEnemy("Liamlas", 6, "res://enemy_pics/newpicture47.png");
            
            liamlas.Resistances = new ResistanceArray();
            liamlas.Resistances.CreateResistance(ResistanceType.Wk, Elements.Dark);

            liamlas.Skills.Add(SkillDatabase.LIGHT_1.Clone());

            return liamlas;
        }

        private Enemy MakeAlternatingEnemy(string name, int hp, string image)
        {
            var alternatingEnemy = new AlternatingEnemy();
            alternatingEnemy.Name = name;
            alternatingEnemy.MaxHP = hp;
            alternatingEnemy.Image = image;
            alternatingEnemy.Resistances = new ResistanceArray();
            return alternatingEnemy;
        }
    }
}
