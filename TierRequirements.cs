using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class TierRequirements
    {
        // tiers 10, 20, 30, 40, 50 = the point at which the grade of fusion is allowed
        private static readonly int[] FUSION_TIERS = { 10, 20, 30, 40, 50 };

        /// <summary>
        /// 5
        /// </summary>
        public static int UPGRADE_SCREEN = 5;

        /// <summary>
        /// 10
        /// </summary>
        public static int TIER2_STRONGER_ENEMIES = 10;

        /// <summary>
        /// 15
        /// </summary>
        public static int TIER3_STRONGER_ENEMIES = 15;
        public static int FUSE = 15;

        /// <summary>
        /// 20
        /// </summary>
        public static int TIER4_STRONGER_ENEMIES = 20;
        public static int QUESTS_FUSION_MEMBERS = 20;

        /// <summary>
        /// 20
        /// </summary>
        public static int QUESTS_PARTY_MEMBERS_UPGRADE = 20;

        /// <summary>
        /// 30
        /// </summary>
        public static int QUESTS_ALL_FUSION_MEMBERS = 30;
        public static int ALL_HIT_SKILLS = 40;

        
        /// <summary>
        /// 40
        /// </summary>
        public static int TIER5_STRONGER_ENEMIES = 40;
        public static int TIER6_STRONGER_ENEMIES = 50;

        public static int TIER2_ELEMENTALSPELLS = 50;
        public static int TIER3_ELEMENTALSPELLS = 100;
        public static int TIER4_ELEMENTALSPELLS = 200;
        public static int TIER5_ELEMENTALSPELLS = 300;

        /// <summary>
        /// 100 - Tier I can reasonably expect players to have ascended characters
        /// </summary>
        public static int ASCENSION = 100;

        public static int GetFusionTierRequirement(int fusionGrade)
        {
            int index = fusionGrade - 1;
            if(index >= FUSION_TIERS.Length)
            {
                return -1;
            }
            else
            {
                return FUSION_TIERS[index];
            }
        }
    }
}
