using AscendedZ.resistances;
using AscendedZ.skills;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.partymember_objects
{
    public class Gagar : OverworldEntity
    {
        public Gagar()
        {
            this.Name = "Gagar";
            this.Image = "res://party_members/newpicture56_Gagar.png";
            this.VorpexValue = 1;
            this.MaxHP = 10;

            this.Resistances.CreateResistance(ResistanceType.Rs, Elements.Fir);
            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Ice);

            this.Skills.Add(SkillDatabase.FIRE_1);
        }

        public override OverworldEntity Create()
        {
            return new Gagar();
        }
    }
}
