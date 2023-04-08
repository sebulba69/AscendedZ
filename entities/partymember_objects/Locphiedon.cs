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
    public class Locphiedon : OverworldEntity
    {
        public Locphiedon() : base()
        {
            this.Name = "Locphiedon";
            this.Image = "res://party_members/newpicture86.png";
            this.VorpexValue = 1;
            this.MaxHP = 10;

            this.Resistances.CreateResistance(ResistanceType.Rs, Elements.Wind);
            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Elec);

            this.Skills.Add(SkillDatabase.WIND_1);
        }

        public override OverworldEntity Create()
        {
            return new Locphiedon();
        }
    }
}
