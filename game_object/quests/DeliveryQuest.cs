using AscendedZ.entities.partymember_objects;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object.quests
{
    public class DeliveryQuest : Quest
    {
        private List<string> _skillBaseNames;

        public string DeliveryDisplayString { get; set; }
        public string PartyMemberName { get; set; }
        public int Grade { get; set; }
        public int Level { get; set; }
        public int AscendedLevel { get; set; }

        /// <summary>
        /// Challenge 1
        /// </summary>
        public List<string> SkillBaseNames { get => _skillBaseNames; set => _skillBaseNames = value; }

        public DeliveryQuest()
        {
            _maxChallenges = 1;
            _skillBaseNames = new List<string>();
        }

        public override Texture2D GetIcon()
        {
            if (!string.IsNullOrEmpty(PartyMemberName))
            {
                string imagePath = CharacterImageAssets.GetImagePath(PartyMemberName);
                return CharacterImageAssets.GetTextureForItemList(imagePath);
            } 
            else
                return base.GetIcon();
        }

        public override string GetQuestNameString()
        {
            return $"[{DeliveryDisplayString}] Delivery Quest";
        }

        public override void GenerateQuest(Random rng, int maxTier)
        {
            string partyMemberBaseName = string.Empty;
            int grade = 0;
            int partyLevel = 0;
            int ascendedLevel = 0;
            List<string> skillBaseNames = new List<string>();
            int numPartyQuestChallenges = rng.Next(MaxChallenges + 1);

            List<string> partyQuestBaseNames = EntityDatabase.GetAllPartyNamesForBattleQuest(maxTier);
            string partyQuestBaseName = partyQuestBaseNames[rng.Next(partyQuestBaseNames.Count)];
            OverworldEntity partyMember = PartyMemberGenerator.MakePartyMember(partyQuestBaseName);

            int numLevelUps = (int)Math.Ceiling(Math.Round(maxTier / Math.Sqrt(maxTier), 2));

            if(partyMember.Skills.Count == 0)
            {
                var generatedSkills = SkillDatabase.GetAllGeneratableSkills(maxTier);

                int numSkills = rng.Next(1, 3);
                HashSet<ISkill> noDupeSkills = new HashSet<ISkill>();
                for(int s = 0; s < numSkills; s++)
                {
                    var skill = generatedSkills[rng.Next(generatedSkills.Count)];
                    if (noDupeSkills.Contains(skill))
                    {
                        while(noDupeSkills.Contains(skill))
                            skill = generatedSkills[rng.Next(generatedSkills.Count)];
                    }

                    noDupeSkills.Add(skill);
                    partyMember.Skills.Add(skill.Clone());
                }
            }

            for (int level = 0; level < numLevelUps; level++)
            {
                partyMember.LevelUp();
               
                if (maxTier >= TierRequirements.ASCENSION && partyMember.CanAscend())
                {
                    // 40% chance of an ascended party member being required for this quest
                    if ((rng.Next(1, 101) > 60))
                    {
                        int remainder = maxTier % 100;
                        int tier = maxTier - remainder;
                        int ascensionTimes = tier / 100;
                        for (int i = 0; i < ascensionTimes; i++)
                        {
                            partyMember.Ascend();
                        }
                    }
                }
            }

            partyMemberBaseName = partyMember.Name;
            grade = partyMember.Grade;
            partyLevel = partyMember.Level;
            ascendedLevel = partyMember.AscendedLevel;

            if (numPartyQuestChallenges > 0 && partyMember.FusionGrade > 0)
            {
                foreach (var skill in partyMember.Skills)
                    skillBaseNames.Add(skill.BaseName);

                VorpexReward = (VorpexReward + (int)Math.Ceiling(VorpexReward * 0.75)) * partyMember.Skills.Count + partyMember.FusionGrade;
            }

            PartyMemberName = partyMemberBaseName;
            Level = partyLevel;
            AscendedLevel = ascendedLevel;
            Grade = grade;
            SkillBaseNames = skillBaseNames;
            DeliveryDisplayString = partyMember.DisplayName;
        }

        public override string ToString()
        {
            StringBuilder desc = new StringBuilder();

            desc.AppendLine($"Delivery Quest ● Reward: {VorpexReward} VC");
            desc.AppendLine($"Deliver (min. level): {DeliveryDisplayString}");
            if(SkillBaseNames.Count > 0)
            {
                desc.AppendLine($"Req. Skills: {string.Join(", ", SkillBaseNames)}");
            }

            return desc.ToString();
        }
    }
}
