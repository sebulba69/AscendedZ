﻿using AscendedZ.currency;
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
        private static int A = 6;
        private static int B = 5;
        private static int C = 2;

        private static readonly HashSet<int> REWARD_TIERS = new HashSet<int>()
        {
            1, 4, 5
        };

        public static bool CanGenerateRewardsAtTier(int tier)
        {
            return (REWARD_TIERS.Contains(tier) || tier % 5 == 0);
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

            if (REWARD_TIERS.Contains(tier))
            {
                rewards.Add(new Vorpex() { Amount = 1 });
            }
            else if (tier >= 6)
            {
                int amount = (int)(A * Math.Log(tier - B) + C);
                rewards.Add(new Vorpex() { Amount = amount });
            }

            return rewards;
        }
    }
}
