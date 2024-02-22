using AscendedZ.battle;
using AscendedZ.game_object.quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class QuestObject
    {
        /// <summary>
        /// Public for Serialization. Do not access these outside of this class.
        /// </summary>
        public List<BattleQuest> BattleQuests { get; set; }

        public List<PartyQuest> PartyQuests { get; set; }

        public QuestObject()
        {
            if (BattleQuests == null)
                BattleQuests = new List<BattleQuest>();

            if (PartyQuests == null)
                PartyQuests = new List<PartyQuest>();
        }

        /// <summary>
        /// Check Battle Quest Conditions at the end of a battle.
        /// </summary>
        /// <param name="battleSceneObject"></param>
        /// <exception cref="Exception"></exception>
        public void CheckBattleQuestConditions(BattleSceneObject battleSceneObject)
        {
            foreach(BattleQuest quest in BattleQuests)
            {
                if (!quest.Completed && quest.Registered)
                {
                    bool isComplete = false;

                    if (quest.Tier == 0)
                        throw new Exception("Tier must be set by a quest.");

                    isComplete = (quest.Tier == battleSceneObject.Tier);

                    if(quest.ReqTurnCount > 0)
                        isComplete = (quest.ReqTurnCount == battleSceneObject.TurnCount);

                    if (quest.MinReqPartySize > 0)
                        isComplete = (quest.MinReqPartySize == battleSceneObject.Players.Count);

                    if(quest.RequiredPartyMembers.Count > 0)
                    {
                        foreach(var member in battleSceneObject.Players)
                        {
                            isComplete = (quest.RequiredPartyMembers.Contains(member.BaseName));
                            if (!isComplete)
                                break;
                        }
                    }

                    quest.Completed = isComplete;
                }
            }
        }
    }
}
