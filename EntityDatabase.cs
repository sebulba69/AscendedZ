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
using AscendedZ.screens.back_end_screen_scripts;
using System.Xml.Linq;

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
        private static readonly int RANDOM_TIER = 5;

        private static EnemyMaker _enemyMaker = new EnemyMaker();

        private static readonly List<string>[] TUTORIAL_ENCOUNTERS = new List<string>[]
        {
            new List<string>(){ EnemyNames.Conlen },
            new List<string>(){ EnemyNames.Liamlas, EnemyNames.Orachar },
            new List<string>(){ EnemyNames.Fastrobren, EnemyNames.Conlen, EnemyNames.Liamlas },
            new List<string>(){ EnemyNames.CattuTDroni, EnemyNames.Orachar, EnemyNames.Conlen  },
        };

        /// <summary>
        /// Boss encounters for every 10 floors
        /// </summary>
        private static readonly List<string> BOSS_ENCOUNTERS = new List<string>
        {
            EnemyNames.Harbinger,
            EnemyNames.Elliot_Onyx,
            EnemyNames.Sable_Vonner,
            EnemyNames.Cloven_Umbra,
            EnemyNames.Ashen_Ash,
            EnemyNames.Ethel_Aura,
            EnemyNames.Kellam_Von_Stein,
            EnemyNames.Drace_Skinner,
            EnemyNames.Jude_Stone,
            EnemyNames.Drace_Razor
        };

        private static readonly List<string> DUNGEON_BOSS_ENCOUNTERS = new List<string>() 
        {
            EnemyNames.Ocura,
            EnemyNames.Emush
        };

        /// <summary>
        /// Based on tier. Each one is cumulative based on the tier.
        /// Entry [1] would be included with entry [0] and so on.
        /// </summary>
        private static readonly List<string>[] VENDOR_WARES = new List<string>[]
        {
            new List<string>(){ PartyNames.Locphiedon },
            new List<string>(){ PartyNames.Gagar},
            new List<string>(){ PartyNames.Maxwald },
            new List<string>(){ PartyNames.Yuudam },
            new List<string>(){ PartyNames.Pecheal, PartyNames.Toke },
            new List<string>(){ PartyNames.Halvia }
        };

        private static readonly List<string> CUSTOM_WARES = new List<string>
        {
            EnemyNames.Conlen, PartyNames.Andmond, PartyNames.Joan, PartyNames.Tyhere, PartyNames.Paria, EnemyNames.Isenald
        };

        /// <summary>
        /// A list of indexes that the current tier must be equal to or greater than
        /// to become available in the shop.
        /// </summary>
        private static readonly int[] SHOP_INDEXES = new int[] { 1, 2, 3, 5, 11, 13 };

        /// <summary>
        /// List based off the element each character is strong to.
        /// </summary>
        private static readonly Dictionary<Elements, string> FUSION1_RESULTS = new Dictionary<Elements, string>() 
        {
            { Elements.Fire, PartyNames.Ancrow },
            { Elements.Ice, PartyNames.Candun},
            { Elements.Wind, PartyNames.Samlin},
            { Elements.Elec, PartyNames.Ciavid },
            { Elements.Light, PartyNames.Conson },
            { Elements.Dark, PartyNames.Cermas }
        };

        private static readonly Dictionary<Elements, string> FUSION2_RESULTS = new Dictionary<Elements, string>()
        {
            { Elements.Fire, PartyNames.Marchris },
            { Elements.Ice, PartyNames.Thryth },
            { Elements.Wind, PartyNames.Everever },
            { Elements.Elec, PartyNames.Eri },
            { Elements.Light, PartyNames.Winegeful },
            { Elements.Dark, PartyNames.Fledron }
        };

        private static readonly Dictionary<Elements, string> FUSION3_RESULTS = new Dictionary<Elements, string>()
        {
            { Elements.Fire, PartyNames.Ride},
            { Elements.Ice,  PartyNames.Shacy},
            { Elements.Wind, PartyNames.Lesdan},
            { Elements.Elec,  PartyNames.Tinedo},
            { Elements.Light,  PartyNames.Earic},
            { Elements.Dark,  PartyNames.Baring }
        };

        private static readonly Dictionary<Elements, string> FUSION4_RESULTS = new Dictionary<Elements, string>()
        {
            { Elements.Fire, PartyNames.Muelwise},
            { Elements.Ice, PartyNames.Swithwil},
            { Elements.Wind,  PartyNames.Ronboard},
            { Elements.Elec, PartyNames.Xtrasu },
            { Elements.Light, PartyNames.LatauVHurquij},
            { Elements.Dark, PartyNames.Tami }
        };

        private static readonly Dictionary<Elements, string> FUSION5_RESULTS = new Dictionary<Elements, string>()
        {
            { Elements.Fire, PartyNames.Pher },
            { Elements.Ice,  PartyNames.Isenann},
            { Elements.Wind, PartyNames.Dosam },
            { Elements.Elec, PartyNames.Laanard },
            { Elements.Light, PartyNames.Hallou },
            { Elements.Dark, PartyNames.Dinowaru }
        };

        public static List<Enemy> MakeBattleEncounter(int tier, bool dungeonCrawlEncounter)
        {
            List<Enemy> encounter = new List<Enemy>();
            if ((tier - RANDOM_TIER) < 0)
            {
                foreach (string enemyName in TUTORIAL_ENCOUNTERS[tier - 1])
                {
                    var enemy = _enemyMaker.MakeEnemy(enemyName, tier);
                    enemy.Boost(tier);
                    encounter.Add(enemy);
                }
            }
            else
            {
                GameObject gameObject = PersistentGameObjects.GameObjectInstance();
                int encounterIndex = tier - RANDOM_TIER;

                List<string> encounterNames = new List<string>();

                // If we have already stored an encounter in this list, we want to re-use it.
                if (gameObject.EncountersIndex.Contains(encounterIndex) && !dungeonCrawlEncounter)
                {
                    encounterNames = gameObject.Encounters[encounterIndex];
                }
                else
                {
                    if (tier % 10 == 0 && !dungeonCrawlEncounter)
                    {
                        int bossIndex = (tier / 10) - 1;
                        encounterNames.Add(BOSS_ENCOUNTERS[bossIndex]);
                    }
                    else if ((tier - 5) % 50 == 0 && dungeonCrawlEncounter)
                    {
                        int bossIndex = (tier/50) - 1;
                        encounterNames.Add(DUNGEON_BOSS_ENCOUNTERS[bossIndex]);
                    }
                    else
                    {
                        // use RANDOM_ENEMIES as a base
                        List<string> possibleEncounters = new List<string>();

                        string[] tier1RandomEncounters = new string[] { EnemyNames.Liamlas, EnemyNames.Fastrobren, EnemyNames.Thylaf, EnemyNames.Arwig, EnemyNames.Riccman, EnemyNames.Gardmuel, EnemyNames.Sachael, EnemyNames.Isenald, EnemyNames.CattuTDroni };
                        string[] tier2RandomEncounters = new string[] { EnemyNames.Ed, EnemyNames.Otem, EnemyNames.Hesret };
                        string[] tier3RandomEncounters = new string[] { EnemyNames.Nanfrea, EnemyNames.Ferza, EnemyNames.Anrol, EnemyNames.David };
                        string[] tier4RandomEncounters = new string[] { EnemyNames.Fledan, EnemyNames.Walds, EnemyNames.Naldbear, EnemyNames.Stroma_Hele,EnemyNames.Thony, EnemyNames.Conson };
                        string[] tier5RandomEncounters = new string[] { EnemyNames.Pebrand, EnemyNames.Leofuwil, EnemyNames.Gormacwen, EnemyNames.Vidwerd, EnemyNames.Sylla, EnemyNames.Venforth };
                        string[] tier6RandomEncounters = new string[] { EnemyNames.Aldmas, EnemyNames.Fridan, EnemyNames.Bue, EnemyNames.Bued, EnemyNames.Bureen, EnemyNames.Wennald, EnemyNames.Garcar, EnemyNames.LaChris,
                                                                        EnemyNames.Isumforth, EnemyNames.Ingesc, EnemyNames.Rahfortin, EnemyNames.Leswith, EnemyNames.Paca, EnemyNames.Wigfred, EnemyNames.Lyley, EnemyNames.Acardeb,
                                                                        EnemyNames.Darol, EnemyNames.Hesbet, EnemyNames.Olu, EnemyNames.Iaviol, EnemyNames.Zalth, EnemyNames.Bernasbeorth };
                        string[] randomEnemies = new string[] { EnemyNames.Ardeb,  EnemyNames.DrigaBoli, EnemyNames.FoameShorti,
                                                                EnemyNames.ReeshiDeeme, EnemyNames.Tily, EnemyNames.Hahere };
                        
                        possibleEncounters.AddRange(tier1RandomEncounters);

                        int minEnemies = 2;
                        int maxEnemies = 4;

                        // new encounters available past certain tiers
                        if(tier > TierRequirements.TIER2_STRONGER_ENEMIES)
                        {
                            possibleEncounters.AddRange(tier2RandomEncounters);
                        }

                        if(tier > TierRequirements.TIER3_STRONGER_ENEMIES)
                        {
                            possibleEncounters.AddRange(tier3RandomEncounters);
                            minEnemies = 3;
                        }

                        if (tier > TierRequirements.TIER4_STRONGER_ENEMIES)
                        {
                            minEnemies = 4;
                            maxEnemies = 5;
                            possibleEncounters.RemoveRange(0, tier1RandomEncounters.Length);
                            possibleEncounters.AddRange(tier4RandomEncounters);
                        }

                        if(tier > TierRequirements.TIER5_STRONGER_ENEMIES)
                        {
                            possibleEncounters.RemoveRange(0, tier2RandomEncounters.Length);
                            possibleEncounters.AddRange(tier5RandomEncounters);
                        }

                        if (tier > TierRequirements.TIER6_STRONGER_ENEMIES)
                        {
                            possibleEncounters.RemoveRange(0, tier3RandomEncounters.Length);
                            possibleEncounters.RemoveRange(0, tier4RandomEncounters.Length);
                            possibleEncounters.AddRange(tier6RandomEncounters);
                            possibleEncounters.AddRange(randomEnemies);
                        }

                        int numEnemies = RANDOM.Next(minEnemies, maxEnemies + 1);
                        for (int i = 0; i < numEnemies; i++)
                        {
                            int randomEnemyIndex = RANDOM.Next(possibleEncounters.Count);
                            string enemyName = possibleEncounters[randomEnemyIndex];
                            encounterNames.Add(enemyName);
                        }
                    }

                    if (!dungeonCrawlEncounter)
                    {
                        gameObject.EncountersIndex.Add(encounterIndex);
                        gameObject.Encounters.Add(encounterNames);
                    }
                    PersistentGameObjects.Save();
                }

                int boost = GetTierBoost(tier);
                tier += boost;

                foreach (string name in encounterNames)
                {
                    Enemy enemy = _enemyMaker.MakeEnemy(name, tier);
                    enemy.Boost(tier);
                    encounter.Add(enemy);
                }
            }

            return encounter;
        }

        public static List<Enemy> MakeRandomEnemyEncounter(int tier, bool isBoss)
        {
            List<Enemy> encounter = new List<Enemy>();

            int min = 3;
            int max = 6;

            int encounterNumber = RANDOM.Next(min, max);
            var random = new RandomEnemyFactory();

            int boost = GetTierBoost(tier);
            tier += boost;
            random.SetTier(tier);

            if (!isBoss)
            {
                for (int e = 0; e < encounterNumber; e++)
                {
                    var enemy = random.GenerateEnemy();
                    enemy.Boost(tier);
                    encounter.Add(enemy);
                }
            }
            else
            {
                var boss = random.GenerateBoss(tier);
                boss.Boost(tier);
                encounter.Add(boss);
            }


            return encounter;
        }

        public static int GetTierBoost(int tier)
        {
            double boostBase = 0.2;

            int numBoosts = tier / 5;

            for (int i = 0; i < numBoosts; i++)
                boostBase += 0.05;

            return (int)(tier * boostBase);
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

            return partyMembers;
        }
    
        public static List<FusionObject> MakeFusionEntities(OverworldEntity material1, OverworldEntity material2)
        {
            int gradeDifference = Math.Abs(material2.FusionGrade - material1.FusionGrade);
            if(gradeDifference > 1)
                return new List<FusionObject>();

            int fusionGrade = material1.FusionGrade + 1;

            List<FusionObject> possibleFusions = new List<FusionObject>();
            Dictionary<Elements, string> fusionResults = new Dictionary<Elements, string>();
            List<Dictionary<Elements, string>> results = new List<Dictionary<Elements, string>>() 
            {
                FUSION1_RESULTS, FUSION2_RESULTS, FUSION3_RESULTS, FUSION4_RESULTS, FUSION5_RESULTS
            };
            
            if(fusionGrade - 1 < results.Count)
            {
                fusionResults = results[fusionGrade - 1];
            }
            else
            {
                fusionResults = results[results.Count - 1];
            }

            List<Elements> primaryElements = GetPrimaryElements(material1, material2);

            if (primaryElements.Count == 0)
            {
                primaryElements = GetPrimaryWeaknessElements(material1, material2);
                if(primaryElements.Count == 0)
                    primaryElements = GetNoneResistances(material1, material2);
            }

            PopulatePossibleFusions(fusionResults, primaryElements, possibleFusions, material1, material2);

            return possibleFusions;
        }
        
        private static void PopulatePossibleFusions(Dictionary<Elements, string> fusionResults, List<Elements> elements, 
                                                    List<FusionObject> possibleFusions, OverworldEntity material1, 
                                                    OverworldEntity material2)
        {
            foreach (var element in elements)
            {
                if (fusionResults.ContainsKey(element))
                {
                    var fusionObject = new FusionObject
                    {
                        Material1 = material1,
                        Material2 = material2
                    };

                    var fusion = PartyMemberGenerator.MakePartyMember(fusionResults[element]);
                    fusion.MaxHP = (int)((material1.MaxHP + material2.MaxHP) / 1.5);
                    fusion.VorpexValue = (material1.VorpexValue + material2.VorpexValue)/ 2;

                    fusion.SetLevel((material1.Level + material2.Level)/ 2);
                    fusionObject.Fusion = fusion;

                    possibleFusions.Add(fusionObject);
                }

            }
        }

        private static List<Elements> GetPrimaryElements(OverworldEntity material1, OverworldEntity material2)
        {
            List<Elements> elements = new List<Elements>();

            var mat1Elements = material1.Resistances.GetPrimaryElements();
            var mat2Elements = material2.Resistances.GetPrimaryElements();

            elements.AddRange(MatchElements(mat1Elements, mat2Elements));

            return elements;
        }

        private static List<Elements> GetPrimaryWeaknessElements(OverworldEntity material1, OverworldEntity material2)
        {
            List<Elements> elements = new List<Elements>();

            var mat1Elements =  material1.Resistances.GetPrimaryWeaknessElements();
            var mat2Elements =  material2.Resistances.GetPrimaryWeaknessElements();

            elements.AddRange(MatchElements(mat1Elements, mat2Elements));

            return elements;
        }

        private static List<Elements> GetNoneResistances(OverworldEntity material1, OverworldEntity material2)
        {
            List<Elements> primaryElements = new List<Elements>();

            if(material1.Resistances.HasNoResistances() && material2.Resistances.HasNoResistances())
            {
                Elements[] elements = Enum.GetValues<Elements>();

                for (int i = 0; i < 2; i++)
                    primaryElements.Add(elements[RANDOM.Next(0, elements.Length)]);
            }

            return primaryElements;
        }

        private static List<Elements> MatchElements(List<Elements> mat1Elements, List<Elements> mat2Elements)
        {
            List<Elements> elements = new List<Elements>();

            foreach (Elements mat1Element in mat1Elements)
            {
                foreach (Elements mat2Element in mat2Elements)
                {
                    if (mat1Element == mat2Element)
                    {
                        elements.Add(mat1Element);
                    }
                }
            }

            return elements;
        }
    
        /// <summary>
        /// Make party members for a battle quest
        /// </summary>
        public static List<string> GetAllPartyNamesForBattleQuest(int tier)
        {
            List<string> names = new List<string>();

            PopulatePartyNameList(tier, names);

            return names;
        }

        private static void PopulatePartyNameList(int tier, List<string> names)
        {
            int vendorWaresLength = VENDOR_WARES.Length;

            foreach (var list in VENDOR_WARES)
                names.AddRange(list);

            if (tier >= TierRequirements.FUSE)
                names.AddRange(CUSTOM_WARES);

            if (tier > TierRequirements.QUESTS_FUSION_MEMBERS)
                names.AddRange(FUSION1_RESULTS.Values);

            if (tier > TierRequirements.QUESTS_ALL_FUSION_MEMBERS)
            {
                names.AddRange(FUSION2_RESULTS.Values);
            }
        }

        public static int GetBossHP(string bossName)
        {
            int index = BOSS_ENCOUNTERS.IndexOf(bossName);
            if (index == -1)
            {
                throw new Exception("Boss not defined in encounter list");
            }

            // get the boss number
            index++;

            int startingHP = 15 * (index * 5);
            return startingHP;
        }

        public static int GetBossHPDC(string bossName)
        {
            int index = DUNGEON_BOSS_ENCOUNTERS.IndexOf(bossName);
            if (index == -1)
            {
                throw new Exception("Boss not defined in encounter list");
            }

            // get the boss number
            index++;
            index *= 50; // get the actual tier
            index /= 10;

            int startingHP = 15 * (index * 5);
            return startingHP;
        }

        public static int GetBossHPRandom(int tier)
        {
            // get random image
            return 15 * ((tier/10) * 5);
        }
    }
}
