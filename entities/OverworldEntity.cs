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
        private bool _gradeCapHit = false;
        private int _ascendedLevel = 0;

        public int AscendedLevel { get => _ascendedLevel; set => _ascendedLevel = value; }
        public bool GradeCapHit { get => _gradeCapHit; set => _gradeCapHit = value; }
        public bool IsInParty { get => _isInParty; set => _isInParty = value; }
        public int Level { get => _level; set => _level = value; }
        public int Grade { get => _grade; set => _grade = value; }
        public int VorpexValue { get => _vorpexCost; set => _vorpexCost = value; }
        public int ShopCost { get => _shopCost; set => _shopCost = value; }
        public int UpgradeShardYield { get => _upgradeShardYield; set => _upgradeShardYield = value; }
        public int UpgradeShardValue { get => 100; }
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

                if (_ascendedLevel > 0)
                    retString += $" [{_ascendedLevel}]";

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
                Resistances = Resistances
            };

            foreach (var skill in Skills)
                player.Skills.Add(skill.Clone());

            player.Skills.Add(SkillDatabase.Pass);
            return player;
        }

        private const int TIER_CAP = 5;

        public bool CanAscend()
        {
            bool canAscend = false;

            if (_gradeCapHit)
            {
                canAscend = true;
            }
            else
            {
                switch (_ascendedLevel)
                {
                    case 0:
                        canAscend = Grade >= 1;
                        break;
                    case 1:
                        canAscend = Grade >= 3;
                        break;
                    case 2:
                        canAscend = Grade >= 5;
                        break;
                    case 3:
                        canAscend =  Grade >= 6;
                        break;
                    case 4:
                        canAscend = Grade >= 7;
                        break;
                    default:
                        canAscend = Grade >= 8;
                        break;
                }
            }

            return canAscend;
        }

        public void Ascend()
        {
            _ascendedLevel++;
            Grade = 0;
            Level = 0;

            for (int i = 0; i < Skills.Count; i++)
            {
                if (Skills[i].Id == SkillId.Elemental)
                {
                    ElementSkill elementSkill = (ElementSkill)Skills[i];
                    if (elementSkill.Tier < TIER_CAP)
                    {
                        Skills[i] = SkillDatabase.GetNextTierOfElementSkill(elementSkill.Tier, elementSkill).Clone();
                    }
                    else
                    {
                        ElementSkill newSkill = (ElementSkill)SkillDatabase.GetNextTierOfElementSkill(elementSkill.Tier, elementSkill).Clone();
                        newSkill.Damage += (_ascendedLevel * 2);
                        Skills[i] = newSkill;
                    }
                }
                else if (Skills[i].Id == SkillId.Healing)
                {
                    HealSkill healSkill = (HealSkill)Skills[i];
                    if (healSkill.Tier < TIER_CAP)
                    {
                        Skills[i] = SkillDatabase.GetNextTierOfHealSkill(healSkill.Tier);
                    }
                    else
                    {
                        HealSkill newSkill = (HealSkill)SkillDatabase.GetNextTierOfHealSkill(healSkill.Tier).Clone();
                        newSkill.HealAmount += (_ascendedLevel * 2);
                        Skills[i] = newSkill;
                    }
                }
            }
        }

        public void LevelUp()
        {
            Level++;

            try
            {
                VorpexValue = VorpexValue + (Level - 1) * 2;
            }
            catch (Exception)
            {
                VorpexValue = int.MaxValue - 1;
            }

            GradeString = GetLevelString();

            try
            {
                MaxHP += GetMaxHPUpgradeValue();
            }
            catch (Exception)
            {
                MaxHP = int.MaxValue - 1;
            }
            

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
            if (Level == 50)
            {
                Level = 0;
                Grade++;

                if (Grade == grades.Length - 1)
                    _gradeCapHit = true;

                gradeString = $"{grades[Grade]}";
            }
            else
            {
                gradeString = $"{grades[Grade]}.{Level}";
            }

            return gradeString;
        }

        public string GetUpgradeString(bool showUpgradeShards)
        {
            StringBuilder skills = new StringBuilder();
            foreach (ISkill skill in Skills)
                skills.AppendLine(skill.GetUpgradeString());

            if(!showUpgradeShards)
                return $"{MaxHP} HP → {GetHPLevelUpPreview()}\n{Resistances.GetResistanceString()}\n{skills.ToString()}";
            else
            {
                string reqAscendGrade;

                switch (_ascendedLevel)
                {
                    case 0:
                        reqAscendGrade = "E";
                        break;
                    case 1:
                        reqAscendGrade = "C";
                        break;
                    case 2:
                        reqAscendGrade = "A";
                        break;
                    case 3:
                        reqAscendGrade = "S";
                        break;
                    case 4:
                        reqAscendGrade = "SS";
                        break;
                    default:
                        reqAscendGrade = "SSS";
                        break;
                }
                 

                return $"{MaxHP} HP → {GetHPLevelUpPreview()}\n" +
                        $"{Resistances.GetResistanceString()}\n" +
                        $"{skills.ToString()}" +
                        $"Yield → {UpgradeShardYield} US\n" +
                        $"ASC Grade: {reqAscendGrade}";
            }

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
