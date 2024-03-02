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

        public DeliveryQuest()
        {
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
            
            List<string> partyQuestBaseNames = EntityDatabase.GetAllPartyNamesForBattleQuest(maxTier);
            string partyQuestBaseName = partyQuestBaseNames[rng.Next(partyQuestBaseNames.Count)];
            OverworldEntity partyMember = PartyMemberGenerator.MakePartyMember(partyQuestBaseName);

            int numLevelUps = (int)Math.Ceiling(Math.Round(maxTier / Math.Sqrt(maxTier), 2));
            for (int level = 0; level < numLevelUps; level++)
                partyMember.LevelUp();

            PartyMemberName = partyMember.Name;
            Level = partyMember.Level;
            Grade = partyMember.Grade;
            DeliveryDisplayString = partyMember.DisplayName;

            int multiplier = (partyMember.FusionGrade + 1);
            VorpexReward = (partyMember.Level + partyMember.VorpexValue * multiplier)/2;
        }

        public override string ToString()
        {
            StringBuilder desc = new StringBuilder();

            desc.Append($"Delivery Quest ● Reward: {VorpexReward} VC\n");
            desc.Append($"Deliver (min. level): {DeliveryDisplayString}\n");
            desc.Append($"Party member is lost on completion.\n");

            return desc.ToString();
        }
    }
}
