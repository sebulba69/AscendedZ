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
            new List<string>(){ PartyNames.LOCPHIEDON },
            new List<string>(){ PartyNames.GAGAR },
            new List<string>(){ PartyNames.YUUDAM },
            new List<string>(){ PartyNames.PECHEAL, PartyNames.TOKE },
            new List<string>(){ PartyNames.MAXWALD, PartyNames.HALVIA }
        };

        /// <summary>
        /// A list of indexes that the current tier must be equal to or greater than
        /// to become available in the shop.
        /// </summary>
        private static readonly int[] SHOP_INDEXES = new int[] { 1, 2, 5, 6, 8 };

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

            // Get vendor wares based on the tier we're on
            int vendorWaresIndex = 0;
            foreach (int index in SHOP_INDEXES)
            {
                if(tier >= index)
                {
                    foreach(string member in VENDOR_WARES[vendorWaresIndex])
                        partyMembers.Add(PartyMemberGenerator.MakePartyMember(member, true));

                    vendorWaresIndex++;
                }
                else
                {
                    break;
                }
            }

            return partyMembers;
        }
    }
}
