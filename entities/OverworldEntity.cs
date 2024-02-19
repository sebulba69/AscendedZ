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

namespace AscendedZ.entities.partymember_objects
{
    public class OverworldEntity : Entity
    {
        private int _level = 0;
        private int _grade = 0;
        private int _vorpexCost = 1;
        private int _shopCost = 1;
        private bool _isInParty = false;
        private int _fusionGrade = 0;
        private int _upgradeShardYield = 10;
        private int _skillCap = 2;

        public bool IsInParty { get => _isInParty; set => _isInParty = value; }
        public int Level { get => _level; set => _level = value; }
        public int Grade { get => _grade; set => _grade = value; }
        public int VorpexValue { get => _vorpexCost; set => _vorpexCost = value; }
        public int ShopCost { get => _shopCost; set => _shopCost = value; }
        public int UpgradeShardYield { get => _upgradeShardYield; set => _upgradeShardYield = value; }
        public int MaxHP { get; set; }
        public string GradeString { get; set; }
        public int SkillCap { get => _skillCap; set => _skillCap = value; }
        public string DisplayName { get
            {
                if (string.IsNullOrEmpty(GradeString))
                {
                    return Name;
                }
                else
                {
                    return $"[{GradeString}] {Name}";
                }
            } 
        }
        public List<ISkill> Skills { get; set; } = new();
        public ResistanceArray Resistances { get; set; } = new();
        public int FusionGrade { get => _fusionGrade; set => _fusionGrade = value; }

        public BattlePlayer MakeBattlePlayer()
        {
            var player = new BattlePlayer()
            {
                Name = DisplayName,
                Image = Image,
                HP = MaxHP,
                MaxHP = MaxHP,
                Resistances = Resistances
            };

            foreach (var skill in Skills)
                player.Skills.Add(skill.Clone());

            player.Skills.Add(SkillDatabase.Pass);
            return player;
        }

        public void LevelUp()
        {
            Level++;

            VorpexValue = VorpexValue + (Level - 1) * 2;

            GradeString = GetLevelString();

            MaxHP += GetMaxHPUpgradeValue();

            foreach (ISkill skill in Skills)
                skill.LevelUp();
        }

        public void BoostShopCost()
        {
            ShopCost = (ShopCost + Level) * 2;
        }

        public string GetHPLevelUpPreview()
        {
            return $"{MaxHP + GetMaxHPUpgradeValue()} HP";
        }

        private int GetMaxHPUpgradeValue()
        {
            return (7 + 2 * (Level + 1)) / 2;
        }

        private string GetLevelString()
        {
            string[] grades = { "F", "E", "D", "C", "B", "A", "S", "SS", "SSS" };
            string gradeString = string.Empty;
            if (Level == 10)
            {
                Level = 0;
                Grade++;

                if (Grade >= grades.Length)
                {
                    int last = grades.Length - 1;
                    gradeString = $"{grades[last]}";

                    int remainder = Grade - last;
                    gradeString = $"{gradeString}x{remainder}";
                }
                else
                {
                    gradeString = $"{grades[Grade]}";
                }
            }
            else
            {
                gradeString = $"{grades[Grade]}.{Level}";
            }

            return gradeString;
        }

        public string GetUpgradeString()
        {
            StringBuilder skills = new StringBuilder();
            foreach (ISkill skill in Skills)
                skills.AppendLine(skill.GetUpgradeString());

            return $"{MaxHP} HP → {GetHPLevelUpPreview()}\n{Resistances.GetResistanceString()}\n{skills.ToString()}";
        }

        public override string ToString()
        {
            StringBuilder skills = new StringBuilder();
            if(Skills.Count > 0)
            {
                foreach (ISkill skill in Skills)
                    skills.AppendLine(skill.ToString());
            }
            else
            {
                skills.AppendLine("[NONE]");
            }

            return $"{MaxHP} HP\n{Resistances.GetResistanceString()}\n{skills.ToString()}";
        }
    }
}
