using AscendedZ.entities.partymember_objects;
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

        /// <summary>
        /// Party Member Name <-- includes Grade
        /// </summary>
        public string PartyMemberName { get; set; }

        /// <summary>
        /// Challenge 1
        /// </summary>
        public List<string> SkillBaseNames { get => _skillBaseNames; set => _skillBaseNames = value; }

        public DeliveryQuest()
        {
            _maxChallenges = 1;
            _skillBaseNames = new List<string>();
        }

        public override string GetQuestNameString()
        {
            return $"[{PartyMemberName}] Delivery Quest";
        }

        public override void GenerateQuest(Random rng, int maxTier)
        {
            string partyMemberFullName = string.Empty;
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
                for(int s = 0; s < numSkills; s++)
                {
                    var skill = generatedSkills[rng.Next(generatedSkills.Count)];
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

            partyMemberFullName = partyMember.DisplayName;

            if (numPartyQuestChallenges > 0)
            {
                foreach (var skill in partyMember.Skills)
                {
                    skillBaseNames.Add(skill.BaseName);
                }   
            }



            PartyMemberName = partyMemberFullName;
            SkillBaseNames = skillBaseNames;
        }

        public override string ToString()
        {
            StringBuilder desc = new StringBuilder();

            desc.AppendLine($"Delivery Quest ● {VorpexReward}");
            desc.AppendLine($"Deliver: {PartyMemberName}");
            if(SkillBaseNames.Count > 0)
            {
                desc.AppendLine($"Req. Skills: {string.Join(", ", SkillBaseNames)}");
            }

            return desc.ToString();
        }
    }
}
