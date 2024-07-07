using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class Equations
    {
        public static int GetVorpexLevelValue(int initialVCValue, int level)
        {
            int vorpex;
            try
            {
                vorpex = initialVCValue + level;
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
                int boost = level / 2;
                if (boost == 0)
                    boost = 1;
                hp += (2 + boost);
            }
            catch (Exception)
            {
                hp = int.MaxValue - 1;
            }

            return hp;
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

        public static int GetDungeonCrawlEncounters(int tier)
        {
            return (tier / 10) + 2;
        }
    }
}
