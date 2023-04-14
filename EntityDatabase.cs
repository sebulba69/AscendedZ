using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.entities.partymember_objects;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    /// <summary>
    /// This class is to be used when we want to access Enemies, Party Members, Bosses, etc.
    /// </summary>
    public class EntityDatabase
    {
        private static readonly Dictionary<string, OverworldEntity> PARTYMEMBERS = new Dictionary<string, OverworldEntity>()
        {
            ["Locphiedon"] = new Locphiedon(),
            ["Gagar"] = new Gagar(),
            ["Yuudam"] = new Yuudam()
        };

        /// <summary>
        /// A map of all enemies meant to be accessed using BASIC_ENEMIES or manually for
        /// pre-set encounters on floors.
        /// </summary>
        private static readonly Dictionary<string, Enemy> ENEMIES = new Dictionary<string, Enemy>()
        {
            ["Conlen"] = new Conlen(),
            ["Orahcar"] = new Orahcar(),
            ["Liamlas"] = new Liamlas(),
            ["CattuTDroni"] = new CattuTDroni(),
            ["Fastrobren"] = new Fastrobren()
        };

        private static List<string>[] ENCOUNTERS = new List<string>[] 
        {
            new List<string>(){ "Conlen" },
            new List<string>(){ "Liamlas", "Orahcar"},
            new List<string>(){ "Fastrobren", "Conlen", "Liamlas" },
            new List<string>(){ "CattuTDroni" }
        };

        /// <summary>
        /// Based on tier. Each one is cumulative based on the tier.
        /// Entry [1] would be included with entry [0] and so on.
        /// </summary>
        private static readonly List<string>[] VENDOR_WARES = new List<string>[]
        {
            new List<string>(){ "Locphiedon" },
            new List<string>(){ "Gagar" },
            new List<string>(){ "Yuudam" }
        };

        /// <summary>
        /// A list of indexes that the current tier must be equal to or greater than
        /// to become available in the shop.
        /// </summary>
        private static readonly int[] SHOP_INDEXES = new int[] { 2, 5, 10 };

        public static List<Enemy> MakeBattleEncounter(int tier)
        {
            List<Enemy> encounter = new List<Enemy>();
            foreach(string enemyName in ENCOUNTERS[tier - 1])
            {
                encounter.Add(ENEMIES[enemyName].Create());
            }
            return encounter;
        }

        public static List<OverworldEntity> MakeShopVendorWares(int tier)
        {
            List<OverworldEntity> partyMembers = new List<OverworldEntity>();
            int indexes = 1; // one by default

            // if the tier is bigger or equal to specific numbers, index++
            foreach(int index in SHOP_INDEXES)
            {
                if(tier >= index)
                {
                    indexes++;
                }
                else
                {
                    break;
                }
            }

            // we want the best units at the top, worst at the bottom
            for(int i = indexes - 1; i >= 0; i--)
            {
                foreach (string member in VENDOR_WARES[i])
                {
                    partyMembers.Add(PARTYMEMBERS[member].Create());
                }
            }
   
            return partyMembers;
        }
    }
}
