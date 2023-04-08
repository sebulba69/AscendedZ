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
        public static readonly List<int> REWARD_TIERS = new List<int>()
        {
            1, 5
        };

        private readonly static List<Currency[]> REWARDS = new List<Currency[]>() 
        { 
            new Currency[] { new Vorpex(){ Amount = 1 } }
        };

        /// <summary>
        /// Generate a set of rewards based on the tier.
        /// Index = tier - 1 because tier starts @ 1
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
        public static Currency[] GenerateReward(int tier)
        {
            int index = tier - 1;
            
            if (index >= REWARDS.Count)
                return null;

            return REWARDS[tier - 1];
        }
    }
}
