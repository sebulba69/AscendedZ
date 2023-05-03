using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.entities.enemy_objects.alt_enemies;
using AscendedZ.entities.enemy_objects.bosses;
using AscendedZ.entities.enemy_objects.enemy_makers;
using AscendedZ.entities.partymember_objects;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
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

        private static EnemyMaker _enemyMaker = new EnemyMaker();

        private static List<string>[] ENCOUNTERS = new List<string>[] 
        {
            new List<string>(){ EnemyNames.CONLEN },
            new List<string>(){ EnemyNames.LIAMLAS, EnemyNames.ORAHCAR},
            new List<string>(){ EnemyNames.FASTROBREN, EnemyNames.CONLEN, EnemyNames.LIAMLAS },
            new List<string>(){ EnemyNames.CATTUTDRONI },
            new List<string>(){ EnemyNames.HARBINGER }
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
                encounter.Add(_enemyMaker.MakeEnemy(enemyName));
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
