using AscendedZ.battle;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object.quests;
using AscendedZ.skills;
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

        public void CheckPartyQuestConditions(List<OverworldEntity> reserves)
        {
            foreach(PartyQuest partyQuest in PartyQuests)
            {
                bool isComplete = false;
                if(!partyQuest.Completed && partyQuest.Registered)
                {
                    if (string.IsNullOrEmpty(partyQuest.PartyMemberName))
                        throw new Exception("Must set a party member name as a bare minimum for the quest.");

                    foreach(OverworldEntity reserve in reserves)
                    {
                        isComplete = (partyQuest.PartyMemberName.Equals(reserve.Name));

                        if (!isComplete)
                            break;

                        if(partyQuest.SkillBaseNames.Count > 0)
                        {
                            foreach(ISkill skill in reserve.Skills)
                            {
                                isComplete = (partyQuest.SkillBaseNames.Contains(skill.BaseName));
                                if (!isComplete)
                                    break;
                            }
                        }
                    }

                    partyQuest.Completed = isComplete;
                }
            }
        }

        public List<Quest> GetQuests()
        {
            List<Quest> quests = new List<Quest>();

            quests.AddRange(BattleQuests);

            quests.AddRange(PartyQuests);

            return quests;
        }

        public int GetTotalQuestCount()
        {
            return BattleQuests.Count + PartyQuests.Count;
        }

        public int GetCompletedQuestCount()
        {
            int count = 0;

            foreach(var quest in GetQuests())
            {
                if (quest.Completed)
                    count++;
            }

            return count;
        }
    }
}
