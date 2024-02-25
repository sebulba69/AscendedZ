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
        private const int MAX_QUESTS = 5;
        private Random _rng;

        /// <summary>
        /// Public for Serialization. Do not access these outside of this class.
        /// </summary>
        public List<BattleQuest> BattleQuests { get; set; }

        public List<PartyQuest> PartyQuests { get; set; }

        public QuestObject()
        {
            _rng = new Random();

            if (BattleQuests == null)
                BattleQuests = new List<BattleQuest>();

            if (PartyQuests == null)
                PartyQuests = new List<PartyQuest>();
        }

        public void GenerateQuests(int maxTier)
        {
            int questsToGenerate = MAX_QUESTS - GetTotalQuestCount();

            for(int i = 0; i < questsToGenerate; i++)
            {
                int vorpexReward = RewardGenerator.GetVorpexAmount(maxTier) * 2;
                int questType = _rng.Next(0, 2);

                if(questType == 0)
                {
                    BattleQuest battleQuest = new BattleQuest() { VorpexReward = vorpexReward };
                    battleQuest.GenerateQuest(_rng, maxTier);

                    BattleQuests.Add(battleQuest);
                }
                else if(questType == 1)
                {
                    PartyQuest partyQuest = new PartyQuest() { VorpexReward = vorpexReward };
                    partyQuest.GenerateQuest(_rng, maxTier);
                    
                    PartyQuests.Add(partyQuest);
                }
                else
                {
                    throw new Exception("Type of quest not implemented");
                }
            }

            if (questsToGenerate > 0)
                PersistentGameObjects.Save();
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
                if (!quest.Completed)
                {
                    bool isComplete = false;

                    if (quest.Tier == 0)
                        throw new Exception("Tier must be set by a quest.");

                    isComplete = (quest.Tier == battleSceneObject.Tier);

                    if(quest.ReqTurnCount > 0)
                        isComplete = (quest.ReqTurnCount == battleSceneObject.TurnCount);

                    if (quest.ReqPartySize > 0)
                        isComplete = (quest.ReqPartySize == battleSceneObject.Players.Count);

                    if(quest.ReqPartyBaseNames.Count > 0)
                    {
                        foreach(var member in battleSceneObject.Players)
                        {
                            isComplete = (quest.ReqPartyBaseNames.Contains(member.BaseName));
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
                if(!partyQuest.Completed)
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

        public void Remove(int index)
        {
            if(index < BattleQuests.Count)
            {
                BattleQuests.RemoveAt(index);
            }
            else
            {
                index -= BattleQuests.Count;
                if(index < PartyQuests.Count)
                {
                    PartyQuests.RemoveAt(index);
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
            return GetQuests().Count;
        }
    }
}
