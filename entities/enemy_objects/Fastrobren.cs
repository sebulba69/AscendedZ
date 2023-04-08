using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects
{
    public class Fastrobren : AlternatingEnemy
    {
        public Fastrobren() : base()
        {
            this.Name = "Fastrobren";
            this.MaxHP = 4;
            this.Image = "res://enemy_pics/newpicture40.png";
            this.Resistances = new ResistanceArray();
            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Light);

            this.Skills.Add(SkillDatabase.DARK_1);
        }

        public override Enemy Create()
        {
            return new Fastrobren();
        }
    }
}
