using AscendedZ.battle;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object.quests
{
    /// <summary>
    /// Do not make an instance of this class. This is just to hold common
    /// properties shared among quests.
    /// </summary>
    public class Quest
    {
        private bool _save = false;
        private bool _completed = false;
        protected int _maxChallenges = 0;
        private int _vorpexReward = 10;

        public bool Completed { get => _completed; set => _completed = value; }
        public int VorpexReward { get => _vorpexReward; set => _vorpexReward = value; }
        public int MaxChallenges { get => _maxChallenges; set => _maxChallenges = value;}
        public bool Save { get => _save; set => _save = value; }

        public virtual Texture2D GetIcon()
        {
            return SkillAssets.GenerateIcon(SkillAssets.QUEST_ICON);
        }

        public virtual void GenerateQuest(Random rng, int maxTier)
        {
            throw new NotImplementedException();
        }

        public virtual string GetQuestNameString()
        {
            throw new NotImplementedException();
        }

        protected int MakeRandomQuestTier(Random rng, int maxTier)
        {
            int tier = TierRequirements.QUESTS;

            // exclude boss rounds
            if (maxTier + 1 % 10 != 0)
                maxTier++;

            tier = rng.Next(maxTier - 5, maxTier);

            if (tier % 10 == 0)
                tier++;

            if (tier <= 5)
                tier = 6;

            return tier;
        }
    }
}
