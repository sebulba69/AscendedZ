using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects
{
    public class Liamlas : AlternatingEnemy
    {
        public Liamlas() : base()
        {
            this.Name = "Liamlas";
            this.MaxHP = 6;
            this.Image = "res://enemy_pics/newpicture47.png";
            this.Resistances = new ResistanceArray();
            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Dark);

            this.Skills.Add(SkillDatabase.LIGHT_1);
        }

        public override Enemy Create()
        {
            return new Liamlas();
        }
    }
}
