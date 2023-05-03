using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.partymember_objects
{
    public class Yuudam : OverworldEntity
    {
        public Yuudam() : base()
        {
            this.Name = "Yuudam";
            this.Image = "res://party_members/newpicture99.png";
            this.VorpexValue = 1;
            this.MaxHP = 10;
            this.Resistances.CreateResistance(ResistanceType.Wk, Elements.Dark);
            this.Skills.Add(SkillDatabase.HEAL_1.Clone());
        }

        public override OverworldEntity Create()
        {
            return new Yuudam();
        }
    }
}
