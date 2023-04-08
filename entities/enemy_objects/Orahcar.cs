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
    public class Orahcar : AlternatingEnemy
    {
        public Orahcar() : base()
        {
            this.Name = "Orahcar";
            this.MaxHP = 6;
            this.Image = "res://enemy_pics/newpicture52.png";
            this.Resistances = new ResistanceArray();
            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Fir);

            this.Skills.Add(SkillDatabase.ICE_1);
        }

        public override Enemy Create()
        {
            return new Orahcar();
        }
    }
}
