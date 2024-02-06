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

        /// <summary>
        /// Initial list of random encounters to be expanded on during generation.
        /// </summary>
        private static readonly List<string> RANDOM_ENEMIES = new List<string>
        {
            EnemyNames.Liamlas, EnemyNames.Fastrobren, 
            EnemyNames.Thylaf, EnemyNames.Arwig, EnemyNames.Riccman,
            EnemyNames.Gardmuel, EnemyNames.Sachael, EnemyNames.Isenald
        };

        private static readonly List<string>[] TUTORIAL_ENCOUNTERS = new List<string>[] 
        {
            new List<string>(){ EnemyNames.Conlen },
            new List<string>(){ EnemyNames.Liamlas, EnemyNames.Orachar },
            new List<string>(){ EnemyNames.Fastrobren, EnemyNames.Conlen, EnemyNames.Liamlas },
            new List<string>(){ EnemyNames.CattuTDroni },
            new List<string>(){ EnemyNames.Harbinger }
        };

        /// <summary>
        /// Boss encounters for every 10 floors
        /// </summary>
        private static readonly List<string> BOSS_ENCOUNTERS = new List<string>
        {
            EnemyNames.Elliot_Onyx,
            EnemyNames.Sable_Vonner
        };


        /// <summary>
        /// Based on tier. Each one is cumulative based on the tier.
        /// Entry [1] would be included with entry [0] and so on.
        /// </summary>
        private static readonly List<string>[] VENDOR_WARES = new List<string>[]
        {
            new List<string>(){ PartyNames.Locphiedon },
            new List<string>(){ PartyNames.Gagar },
            new List<string>(){ PartyNames.Yuudam },
            new List<string>(){ PartyNames.Pecheal, PartyNames.Toke },
            new List<string>(){ PartyNames.Maxwald, PartyNames.Halvia }
        };

        private static readonly List<string> CUSTOM_WARES = new List<string>
        {
            EnemyNames.Conlen, EnemyNames.Orachar, PartyNames.Andmond, PartyNames.Joan, PartyNames.Tyhere, PartyNames.Paria
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
                        // use RANDOM_ENEMIES as a base
                        List<string> possibleEncounters = new List<string>(RANDOM_ENEMIES);

                        int minEncounters = 2;
                        int maxEncounters = 4;

                        // new encounters available past certain tiers
                        if(tier > 10)
                        {
                            possibleEncounters.AddRange(new string[] { EnemyNames.Ed, EnemyNames.Otem, EnemyNames.Hesret });
                            maxEncounters = 6;
                        }

                        if(tier > 15)
                        {
                            possibleEncounters.AddRange(new string[] { EnemyNames.Nanfrea, EnemyNames.Ferza, EnemyNames.Anrol, EnemyNames.David });
                            minEncounters = 3;
                        }

                        int numEnemies = RANDOM.Next(minEncounters, maxEncounters);
                        for (int i = 0; i < numEnemies; i++)
                        {
                            int randomEnemyIndex = RANDOM.Next(possibleEncounters.Count);
                            string enemyName = possibleEncounters[randomEnemyIndex];
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

        public static List<OverworldEntity> MakeShopVendorWares(int tier, bool isCustom = false)
        {
            List<OverworldEntity> partyMembers = new List<OverworldEntity>();

            // Get vendor wares based on the tier we're on
            if (!isCustom)
            {
                int vendorWaresIndex = 0;
                foreach (int index in SHOP_INDEXES)
                {
                    if (tier >= index)
                    {
                        foreach (string member in VENDOR_WARES[vendorWaresIndex])
                            partyMembers.Add(PartyMemberGenerator.MakePartyMember(member));

                        vendorWaresIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                foreach(string name in CUSTOM_WARES)
                    partyMembers.Add(PartyMemberGenerator.MakePartyMember(name));
            }


            // scale price based on tier
            if(tier > 5)
            {
                int remainder = tier % 5;
                int numPriceSpikes = (tier - remainder) / 5;
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
