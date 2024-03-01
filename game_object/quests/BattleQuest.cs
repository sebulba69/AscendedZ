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

        public int Tier { get; set; }
        public List<string> ReqPartyBaseNames { get => _requiredPartyMembers; set => _requiredPartyMembers = value; }
        public int ReqTurnCount { get => _reqTurnCount; set => _reqTurnCount = value; }

        public BattleQuest()
        {
            _requiredPartyMembers = new List<string>();
        }

        public override string GetQuestNameString()
        {
            return $"[T.{Tier}] Battle Quest";
        }

        public string GetInBattleDisplayString()
        {
            List<string> displayText = new List<string>();

            displayText.Add(string.Join(", ", ReqPartyBaseNames));
            
            displayText.Add($"Turns <= {ReqTurnCount}");

            return string.Join(" ● ", displayText);
        }

        public override void GenerateQuest(Random rng, int maxTier)
        {
            int tier = 0;
            int partySize = 0;
            int turnCount = 0;
            List<string> partyBaseNames = new List<string>();

            // get the number of challenges
            tier = MakeRandomQuestTier(rng, maxTier);

            turnCount = rng.Next(3, 5);

            int basePartySize = 4;
            List<string> battleQuestBaseNames = EntityDatabase.GetAllPartyNamesForBattleQuest(tier);

            int numParty = rng.Next(1, basePartySize);

            for (int p = 0; p < numParty; p++)
            {
                string baseName = battleQuestBaseNames[rng.Next(battleQuestBaseNames.Count)];
                if (!partyBaseNames.Contains(baseName))
                    partyBaseNames.Add(baseName);
                else
                {
                    while (partyBaseNames.Contains(baseName))
                        baseName = battleQuestBaseNames[rng.Next(battleQuestBaseNames.Count)];

                    partyBaseNames.Add(baseName);
                }
            }

            int minPartyNames = partyBaseNames.Count;
            if (minPartyNames == 1)
                minPartyNames++;

            Tier = tier;
            ReqPartyBaseNames.AddRange(partyBaseNames);
            ReqTurnCount = turnCount;
            VorpexReward = (Math.Abs(tier - ReqTurnCount) * ReqPartyBaseNames.Count);
        }

        public override string ToString()
        {
            StringBuilder desc = new StringBuilder();

            desc.Append($"Battle Quest ● Tier: {Tier} ● Reward: {VorpexReward} VC\n");
            desc.Append($"Req. in Party: {string.Join(", ", ReqPartyBaseNames)}\n");
            desc.Append($"Max Turns: {ReqTurnCount}");

            return desc.ToString();
        }
    }
}
