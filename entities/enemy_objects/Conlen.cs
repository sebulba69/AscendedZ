using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects
{
    public class Conlen : AlternatingEnemy
    {
        public Conlen() : base()
        {
            this.Name = "Conlen";
            this.MaxHP = 6;
            this.Image = "res://enemy_pics/newpicture49.png";
            this.Resistances = new ResistanceArray();
            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Wind);
            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Ice);

            this.Skills.Add(SkillDatabase.ELEC_1);
            this.Skills.Add(SkillDatabase.FIRE_1);
        }

        public override Enemy Create()
        {
            return new Conlen();
        }
    }
}
