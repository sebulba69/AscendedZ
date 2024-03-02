using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class Equations
    {
        private static int A = 6;
        private static int B = 5;
        private static int C = 2;

        public static int GetVorpexLevelValue(int initialVCValue, int level)
        {
            int vorpex;
            try
            {
                vorpex = initialVCValue + (level - 1) * 2;
            }
            catch (Exception)
            {
                vorpex = int.MaxValue - 1;
            }
            return vorpex;
        }

        public static int GetOWMaxHPUpgrade(int hp, int level)
        {
            try
            {
                int upgrade = (7 + 2 * (level + 1)) / 2;
                hp += upgrade/2;
            }
            catch (Exception)
            {
                hp = int.MaxValue - 1;
            }

            return hp;
        }

        public static int GetVorpexAmount(int a = 6, int b = 5, int c = 2, int tier = 10)
        {
            int amount = (int)(A * Math.Log(tier - B) + C);
            return amount;
        }

        public static int GetBoostAmount(int tier)
        {
            int boost = 0;

            int a = A + tier / 2;
            int b = B + tier / 2;
            boost = GetVorpexAmount(a, b, tier: tier);

            if (boost < 0)
                return 1;
            else
                return (boost/4) + 1;
        }

        public static int GetTierIndexBy10(int tier)
        {
            int index = 0;
            if (tier >= 11)
            {
                tier--;
                index = (tier - (tier % 10)) / 10;
            }
            return index;
        }

        public static int BoostShopCost(int shopCost, int numCalculations)
        {
            for(int i = 0; i < numCalculations; i++)
                shopCost *= 2;

            shopCost /= 3;

            return shopCost;
        }
    }
}
