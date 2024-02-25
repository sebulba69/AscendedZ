using AscendedZ.entities.partymember_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object.quests
{
    public class PartyQuest : Quest
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

        public PartyQuest()
        {
            _maxChallenges = 1;
            _skillBaseNames = new List<string>();
        }

        public override string GetQuestNameString()
        {
            return $"[{PartyMemberName}] Party Quest";
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

            }

            for (int level = 0; level < numLevelUps; level++)
                partyMember.LevelUp();

            partyMemberFullName = partyMember.DisplayName;

            if (numPartyQuestChallenges > 0)
                foreach (var skill in partyMember.Skills)
                    skillBaseNames.Add(skill.BaseName);

            PartyMemberName = partyMemberFullName;
            SkillBaseNames = skillBaseNames;
        }

        public override string ToString()
        {
            StringBuilder desc = new StringBuilder();

            desc.AppendLine($"Party Quest ● {VorpexReward}");
            desc.AppendLine($"Member: {PartyMemberName}");
            if(SkillBaseNames.Count > 0)
            {
                desc.AppendLine($"Skills: {string.Join(", ", SkillBaseNames)}");
            }

            return desc.ToString();
        }
    }
}
