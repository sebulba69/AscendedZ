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
        private const int MAX_QUESTS = 10;
        private Random _rng;

        /// <summary>
        /// Public for Serialization. Do not access these outside of this class.
        /// </summary>
        public List<BattleQuest> BattleQuests { get; set; }

        public List<DeliveryQuest> DeliveryQuests { get; set; }

        public QuestObject()
        {
            _rng = new Random();

            if (BattleQuests == null)
                BattleQuests = new List<BattleQuest>();

            if (DeliveryQuests == null)
                DeliveryQuests = new List<DeliveryQuest>();
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
                    DeliveryQuest partyQuest = new DeliveryQuest() { VorpexReward = vorpexReward };
                    partyQuest.GenerateQuest(_rng, maxTier);
                    
                    DeliveryQuests.Add(partyQuest);
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
                        int reqPartyMembers = quest.ReqPartyBaseNames.Count;
                        int partyCount = 0;
                        foreach (var member in battleSceneObject.Players)
                        {
                            if (quest.ReqPartyBaseNames.Contains(member.BaseName))
                                partyCount++;
                        }

                        isComplete = (partyCount == reqPartyMembers);
                    }

                    quest.Completed = isComplete;
                }
            }
        }

        public void CheckDeliveryQuestConditions(List<OverworldEntity> reserves)
        {
            foreach(DeliveryQuest deliveryQuest in DeliveryQuests)
            {
                if (string.IsNullOrEmpty(deliveryQuest.PartyMemberName))
                    throw new Exception("Must set a party member name as a bare minimum for the quest.");

                bool isComplete = false;

                foreach (OverworldEntity reserve in reserves)
                {
                    isComplete =
                        deliveryQuest.PartyMemberName.Equals(reserve.Name)
                        && deliveryQuest.Level <= reserve.Level
                        && deliveryQuest.AscendedLevel <= reserve.AscendedLevel
                        && deliveryQuest.Grade <= reserve.Grade;

                    if (isComplete && deliveryQuest.SkillBaseNames.Count > 0)
                    {
                        foreach (ISkill skill in reserve.Skills)
                        {
                            isComplete = (deliveryQuest.SkillBaseNames.Contains(skill.BaseName));
                            if (!isComplete)
                                break;
                        }
                    }

                    if (isComplete)
                        break;
                }

                deliveryQuest.Completed = isComplete;
            }
        }

        public void Complete(int index)
        {
            if(index < BattleQuests.Count)
            {
                BattleQuests.RemoveAt(index);
            }
            else
            {
                index -= BattleQuests.Count;
                if(index < DeliveryQuests.Count)
                {
                    var deliveryQuest = DeliveryQuests[index];
                    var mp = PersistentGameObjects.GameObjectInstance().MainPlayer;
                    var partyMember = mp.ReserveMembers.Find(p => p.Name.Equals(deliveryQuest.PartyMemberName));
                    
                    if(partyMember.IsInParty)
                        mp.Party.RemovePartyMember(partyMember);

                    mp.ReserveMembers.Remove(partyMember);

                    DeliveryQuests.RemoveAt(index);
                }
            }
        }

        public List<Quest> GetQuests()
        {
            List<Quest> quests = new List<Quest>();

            quests.AddRange(BattleQuests);

            quests.AddRange(DeliveryQuests);

            return quests;
        }

        public int GetTotalQuestCount()
        {
            return GetQuests().Count;
        }

        public void Clear()
        {
            ClearQuestList(BattleQuests);
            ClearQuestList(DeliveryQuests);
        }

        private void ClearQuestList<E>(List<E> quests) where E : Quest
        {
            quests.RemoveAll(q => !q.Save);
        }
    }
}
