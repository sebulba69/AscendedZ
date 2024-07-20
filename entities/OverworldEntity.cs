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
        private int _maxLevelCap = 999;
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

        public int RefundCost 
        { 
            get 
            {
                int refund = (int)(VorpexValue * 0.1) + 1;
                if (FusionGrade > 0)
                    refund *= FusionGrade;

                return refund;
            } 
        }

        public int MaxHP { get; set; }
        public string GradeString { get; set; }
        public int SkillCap { get => _skillCap; set => _skillCap = value; }
        public string DisplayName 
        { 
            get
            {
                string retString;

                if (string.IsNullOrEmpty(GradeString))
                    retString = Name;
                else
                    retString = $"[{GradeString}] {Name}";

                return retString;
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
                Resistances = Resistances,
                BaseName = Name
            };

            foreach (var skill in Skills)
                player.Skills.Add(skill.Clone());

            player.Skills.Add(SkillDatabase.Pass);
            return player;
        }

        private const int TIER_CAP = 5;

        public void LevelUp()
        {
            Level++;
            GradeString = GetLevelString();

            VorpexValue = Equations.GetVorpexLevelValue(VorpexValue, Level);
            MaxHP = Equations.GetOWMaxHPUpgrade(MaxHP, Level);

            foreach (ISkill skill in Skills)
                skill.LevelUp();
        }

        public string GetHPLevelUpPreview()
        {
            return $"{Equations.GetOWMaxHPUpgrade(MaxHP, Level)} HP";
        }

        private string GetLevelString()
        {
            string[] grades = { "F", "E", "D", "C", "B", "A", "S", "SS", "SSS", "SX4", "SX5", "SX6", "SX7", "SX8", "SX9", "SX10", "XG", "XXG", "X3G", "X4G", "X5G", "ASC" };
            string gradeString = string.Empty;
            if (Level == _maxLevelCap)
            {
                Level = 0;
                Grade++;

                if (Grade == grades.Length - 1)
                {
                    _maxLevelCap *= 2;
                }

                gradeString = $"{grades[Grade]}";
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
                skills.Append(skill.GetUpgradeString() + "\n");

            int refundYield = VorpexValue;

            if (FusionGrade > 0)
                refundYield *= FusionGrade;

            return $"{MaxHP} HP → {GetHPLevelUpPreview()}\n{Resistances.GetResistanceString()}\n{skills.ToString()}\nRefund Yield: {refundYield} VC";
        }

        public string GetFusionString()
        {
            StringBuilder skills = new StringBuilder();

            skills.AppendLine(GetSkills(true));

            string maxHP = GetHPString();

            return $"{maxHP}\n{Resistances.GetResistanceString()}\n{skills.ToString()}";
        }

        public override string ToString()
        {
            StringBuilder skills = new StringBuilder();

            skills.AppendLine(GetSkills(false));

            string maxHP = GetHPString();

            return $"{maxHP}\n{Resistances.GetResistanceString()}\n{skills.ToString()}";
        }

        private string GetHPString()
        {
            string maxHP = $"{MaxHP} HP";
            if (FusionGrade > 0)
                maxHP = $"{maxHP} ● Fusion {FusionGrade}";

            return maxHP;
        }

        private string GetSkills(bool fusion)
        {
            StringBuilder skills = new StringBuilder();

            if (Skills.Count > 0)
            {
                foreach (ISkill skill in Skills)
                {
                    if (fusion)
                    {
                        var clone = skill.Clone();
                        for(int i = 0; i < FusionGrade; i++)
                            clone.LevelUp();

                        skills.AppendLine(clone.ToString());
                    }
                    else
                    {
                        skills.AppendLine(skill.ToString());
                    }
                    
                }
                    
            }
            else
            {
                skills.AppendLine("[NONE]");
            }

            return skills.ToString();
        }
    }
}
