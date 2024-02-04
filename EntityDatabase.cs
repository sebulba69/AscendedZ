using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.entities.enemy_objects.enemy_ais;
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
using AscendedZ.game_object;

namespace AscendedZ
{
    /// <summary>
    /// This class is to be used when we want to access Enemies, Party Members, Bosses, etc.
    /// </summary>
    public class EntityDatabase
    {
        private static readonly Random RANDOM = new Random();

        /// <summary>
        /// The max tier where we start generating enemies randomly.
        /// </summary>
        private static readonly int RANDOM_TIER = 6;

        private static EnemyMaker _enemyMaker = new EnemyMaker();

        private static readonly List<string> RANDOM_ENEMIES = new List<string>
        {
            EnemyNames.LIAMLAS, EnemyNames.FASTROBREN, 
            EnemyNames.THYLAF, EnemyNames.ARWIG, EnemyNames.RICCMAN,
            EnemyNames.GARDMUEL, EnemyNames.SACHAEL, EnemyNames.ISENALD
        };

        private static readonly List<string>[] TUTORIAL_ENCOUNTERS = new List<string>[] 
        {
            new List<string>(){ EnemyNames.CONLEN },
            new List<string>(){ EnemyNames.LIAMLAS, EnemyNames.ORAHCAR },
            new List<string>(){ EnemyNames.FASTROBREN, EnemyNames.CONLEN, EnemyNames.LIAMLAS },
            new List<string>(){ EnemyNames.CATTUTDRONI },
            new List<string>(){ EnemyNames.HARBINGER }
        };

        /// <summary>
        /// Boss encounters for every 10 floors
        /// </summary>
        private static readonly List<string> BOSS_ENCOUNTERS = new List<string>
        {
            EnemyNames.ELLIOT_ONYX
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
            if ((tier - RANDOM_TIER) < 0)
            {
                foreach (string enemyName in TUTORIAL_ENCOUNTERS[tier - 1])
                {
                    encounter.Add(_enemyMaker.MakeEnemy(enemyName));
                }
            }
            else
            {
                GameObject gameObject = PersistentGameObjects.GameObjectInstance();

                int encounterIndex = tier - RANDOM_TIER;

                List<string> encounterNames = new List<string>();

                double m = 0.01 * Math.Pow((encounterIndex - 10), 2) + 1;
                int boost = (int)(Math.Pow(encounterIndex, m)) + 2;

                // If we have already stored an encounter in this list, we want to re-use it.
                if (gameObject.Encounters.Count > encounterIndex)
                {
                    encounterNames = gameObject.Encounters[encounterIndex];
                }
                else
                {
                    if (tier % 10 == 0)
                    {
                        encounterNames.Add(BOSS_ENCOUNTERS[(tier/10) - 1]);
                    }
                    else
                    {
                        int numEnemies = RANDOM.Next(2, 4);
                        for (int i = 0; i < numEnemies; i++)
                        {
                            int randomEnemyIndex = RANDOM.Next(RANDOM_ENEMIES.Count);
                            string enemyName = RANDOM_ENEMIES[randomEnemyIndex];
                            encounterNames.Add(enemyName);
                        }
                    }

                    gameObject.Encounters.Add(encounterNames);
                    PersistentGameObjects.Save();
                }

                foreach (string name in encounterNames)
                {
                    Enemy enemy = _enemyMaker.MakeEnemy(name);
                    enemy.Boost(tier, boost);
                    encounter.Add(enemy);
                }
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
                        partyMembers.Add(PartyMemberGenerator.MakePartyMember(member));

                    vendorWaresIndex++;
                }
                else
                {
                    break;
                }
            }

            // scale price based on tier
            if(tier > 5 && tier % 5 == 0)
            {
                int numPriceSpikes = tier / 5;
                for(int times = 0; times < numPriceSpikes; times++)
                {
                    foreach (var member in partyMembers)
                    {
                        member.BoostShopCost();
                    }  
                }
            }

            return partyMembers;
        }
    }
}
