using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object.quests
{
    /// <summary>
    /// Check this quest when you're at the end of a battle.
    /// </summary>
    public class BattleQuest : Quest
    {
        private List<string> _requiredPartyMembers;
        private int _minReqPartySize = 0;
        private int _reqTurnCount = 0;

        /// <summary>
        /// Tier to repeat.
        /// </summary>
        public int Tier { get; set; }

        /// <summary>
        /// Challenge 1 = List of PartyNames
        /// </summary>
        public List<string> ReqPartyBaseNames { get => _requiredPartyMembers; set => _requiredPartyMembers = value; }
        
        /// <summary>
        /// Challenge 2
        /// </summary>
        public int ReqPartySize { get => _minReqPartySize; set => _minReqPartySize = value; }
        
        /// <summary>
        /// Challenge 3
        /// </summary>
        public int ReqTurnCount { get => _reqTurnCount; set => _reqTurnCount = value; }

        public BattleQuest()
        {
            _maxChallenges = 3;
            _requiredPartyMembers = new List<string>();
        }

        public override string GetQuestNameString()
        {
            return $"[T.{Tier}] Battle Quest";
        }

        public string GetInBattleDisplayString()
        {
            List<string> displayText = new List<string>();
            if (ReqPartyBaseNames.Count > 0)
                displayText.Add(string.Join(", ", ReqPartyBaseNames));

            if (ReqPartySize > 0)
                displayText.Add($"Party <= {ReqPartySize}");

            if (ReqTurnCount > 0)
                displayText.Add($"Turns <= {ReqTurnCount}");

            string finalString;

            if (displayText.Count == 0)
                finalString = $"Complete Tier {Tier}";
            else
                finalString = string.Join(" ● ", displayText);

            return finalString;
        }

        public override void GenerateQuest(Random rng, int maxTier)
        {
            int tier = 0;
            int partySize = 0;
            int turnCount = 0;
            List<string> partyBaseNames = new List<string>();

            // get the number of challenges
            int numBattleChallenges = rng.Next(MaxChallenges + 1);
            tier = MakeRandomQuestTier(rng, maxTier);

            turnCount = rng.Next(3, 5);

            if (numBattleChallenges >= 1)
            {
                int basePartySize = 4;
                List<string> battleQuestBaseNames = EntityDatabase.GetAllPartyNamesForBattleQuest(tier);

                int numParty = rng.Next(1, basePartySize);

                for (int p = 0; p < numParty; p++)
                {
                    string baseName = battleQuestBaseNames[rng.Next(battleQuestBaseNames.Count)];
                    if(!partyBaseNames.Contains(baseName))
                        partyBaseNames.Add(baseName);
                    else
                    {
                        while(partyBaseNames.Contains(baseName))
                            baseName = battleQuestBaseNames[rng.Next(battleQuestBaseNames.Count)];

                        partyBaseNames.Add(baseName);
                    }
                }

                VorpexReward = VorpexReward + (int)Math.Ceiling(VorpexReward * 0.75);
            }

            if (numBattleChallenges >= 2)
            {
                int minPartyNames = partyBaseNames.Count;
                if (minPartyNames == 1)
                    minPartyNames++;

                partySize = rng.Next(minPartyNames, 5);

                VorpexReward = VorpexReward + (int)Math.Ceiling(VorpexReward * 0.75);
            }

            VorpexReward *= 2;
            Tier = tier;
            ReqPartyBaseNames.AddRange(partyBaseNames);
            ReqPartySize = partySize;
            ReqTurnCount = turnCount;
        }

        public override string ToString()
        {
            StringBuilder desc = new StringBuilder();

            desc.AppendLine($"Battle Quest ● Tier: {Tier} ● Reward: {VorpexReward} VC");

            if (ReqPartyBaseNames.Count > 0)
                desc.AppendLine($"Req. in Party: {string.Join(", ", ReqPartyBaseNames)}");

            if(ReqPartySize > 0)
                desc.AppendLine($"Req. Party Size: {ReqPartySize}");

            if (ReqTurnCount > 0)
                desc.AppendLine($"Max Turns: {ReqTurnCount}");

            return desc.ToString();
        }
    }
}
