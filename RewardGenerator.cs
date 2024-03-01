using AscendedZ.currency;
using AscendedZ.currency.rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class RewardGenerator
    {
        private static readonly HashSet<int> REWARD_TIERS = new HashSet<int>()
        {
            1, 4, 5
        };

        public static bool CanGenerateRewardsAtTier(int tier)
        {
            return (REWARD_TIERS.Contains(tier) || tier >= 6);
        }

        /// <summary>
        /// Generate a set of rewards based on the tier.
        /// Index = tier - 1 because tier starts @ 1
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
        public static List<Currency> GenerateReward(int tier)
        {
            List<Currency> rewards = new List<Currency>();

            if (tier > TierRequirements.UPGRADE_SCREEN)
            {
                rewards.Add(new Vorpex() { Amount = Equations.GetVorpexAmount(tier: tier) });
            }
            else
            {
                rewards.Add(new Vorpex() { Amount = 1 });
            }

            return rewards;
        }


    }
}
