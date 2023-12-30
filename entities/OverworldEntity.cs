using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.json_interface_converters;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.entities
{
    public class OverworldEntity : Entity
    {
        public int VorpexValue { get; set; }

        public int MaxHP { get; set; }

        public List<ISkill> Skills { get; set; } = new();

        public ResistanceArray Resistances { get; set; } = new();

        public BattlePlayer MakeBattlePlayer()
        {
            var player = new BattlePlayer()
            {
                Name = this.Name,
                Image = this.Image,
                HP = this.MaxHP,
                MaxHP = this.MaxHP,
                Resistances = this.Resistances
            };

            foreach(var skill in this.Skills)
                player.Skills.Add(skill);

            player.Skills.Add(SkillDatabase.PASS);
            player.Skills.Add(SkillDatabase.RETREAT);
            return player;
        }

        public override string ToString()
        {
            StringBuilder skills = new StringBuilder();
            foreach(ISkill skill in this.Skills)
                skills.AppendLine(skill.ToString());
            return $"{this.MaxHP} HP\n{this.Resistances.GetResistanceString()}\n{skills.ToString()}";
        }
    }
}
