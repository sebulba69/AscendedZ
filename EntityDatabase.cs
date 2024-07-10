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
            EnemyNames.Ashen_Ash
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
            EnemyNames.Conlen, EnemyNames.Orachar, PartyNames.Andmond, PartyNames.Joan, PartyNames.Tyhere, PartyNames.Paria
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

        public static List<Enemy> MakeBattleEncounter(int tier)
        {
            List<Enemy> encounter = new List<Enemy>();
            if ((tier - RANDOM_TIER) < 0)
            {
                foreach (string enemyName in TUTORIAL_ENCOUNTERS[tier - 1])
                {
                    var enemy = _enemyMaker.MakeEnemy(enemyName);
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
                if (gameObject.Encounters.Count > encounterIndex)
                {
                    encounterNames = gameObject.Encounters[encounterIndex];
                }
                else
                {
                    if (tier % 10 == 0)
                    {
                        int bossIndex = (tier / 10) - 1;
                        encounterNames.Add(BOSS_ENCOUNTERS[bossIndex]);
                    }
                    else
                    {
                        // use RANDOM_ENEMIES as a base
                        List<string> possibleEncounters = new List<string>();

                        string[] randomEncounters = new string[] 
                        {
                            EnemyNames.Liamlas, EnemyNames.Fastrobren,
                            EnemyNames.Thylaf, EnemyNames.Arwig, EnemyNames.Riccman,
                            EnemyNames.Gardmuel, EnemyNames.Sachael, EnemyNames.Isenald, 
                            EnemyNames.CattuTDroni
                        };
                        
                        possibleEncounters.AddRange(randomEncounters);

                        int minEnemies = 2;
                        int maxEnemies = 4;

                        // new encounters available past certain tiers
                        if(tier > TierRequirements.TIER2_STRONGER_ENEMIES)
                        {
                            possibleEncounters.AddRange(new string[] { EnemyNames.Ed, EnemyNames.Otem, EnemyNames.Hesret });
                        }

                        if(tier > TierRequirements.TIER3_STRONGER_ENEMIES)
                        {
                            possibleEncounters.AddRange(new string[] { EnemyNames.Nanfrea, EnemyNames.Ferza, EnemyNames.Anrol, EnemyNames.David });
                            minEnemies = 3;
                        }

                        if (tier > TierRequirements.TIER4_STRONGER_ENEMIES)
                        {
                            possibleEncounters.RemoveRange(0, 2);
                            possibleEncounters.AddRange(new string[] 
                            {
                                EnemyNames.Fledan,
                                EnemyNames.Walds,
                                EnemyNames.Naldbear,
                                EnemyNames.Stroma_Hele,
                                EnemyNames.Thony,
                                EnemyNames.Conson
                            });
                        }

                        if(tier > TierRequirements.TIER5_STRONGER_ENEMIES)
                        {
                            possibleEncounters.AddRange(new string[]
                            {
                                EnemyNames.Pebrand,
                                EnemyNames.Leofuwil,
                                EnemyNames.Gormacwen,
                                EnemyNames.Vidwerd,
                                EnemyNames.Sylla,
                                EnemyNames.Venforth
                            });
                        }

                        int numEnemies = RANDOM.Next(minEnemies, maxEnemies);
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
                    enemy.Boost(tier);
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

            return partyMembers;
        }
    
        public static List<FusionObject> MakeFusionEntities(OverworldEntity material1, OverworldEntity material2)
        {
            int fusionGrade = GetFusionResultLevel(material1, material2);

            List<FusionObject> possibleFusions = new List<FusionObject>();
            Dictionary<Elements, string> fusionResults = new Dictionary<Elements, string>();

            switch (fusionGrade)
            {
                case 1:
                    fusionResults = FUSION1_RESULTS;
                    break;
                case 2:
                    fusionResults = FUSION2_RESULTS;
                    break;
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

        private static int GetFusionResultLevel(OverworldEntity material1, OverworldEntity material2)
        {
            // default to mat1 grade (if ==, it'll just be this + 1)
            int grade = material1.FusionGrade;
            
            // handle greater than/less than grades differently
            if(material1.FusionGrade != material2.FusionGrade)
            {
                int gradeDifference = Math.Abs(material1.FusionGrade - material2.FusionGrade);

                // with a difference of 1, default to the highest rank
                if(gradeDifference == 1)
                {
                    // go with the bigger grade
                    if (material1.FusionGrade > material2.FusionGrade)
                        grade = material1.FusionGrade;
                    else
                        grade = material2.FusionGrade;
                }
                else
                {
                    // go with the smaller grade
                    if (material1.FusionGrade < material2.FusionGrade)
                        grade = material1.FusionGrade;
                    else
                        grade = material2.FusionGrade;
                }
            }

            return grade + 1;
        }
        
        private static void PopulatePossibleFusions(Dictionary<Elements, string> fusionResults, List<Elements> elements, 
                                                    List<FusionObject> possibleFusions, OverworldEntity material1, 
                                                    OverworldEntity material2)
        {
            foreach (var element in elements)
            {
                if (fusionResults.ContainsKey(element))
                {
                    possibleFusions.Add(new FusionObject
                    {
                        Fusion = PartyMemberGenerator.MakePartyMember(fusionResults[element]),
                        Material1 = material1,
                        Material2 = material2
                    });
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
                throw new Exception("Boss not defined in encounter list");

            // get the boss number
            index++;

            int baseHP = 6;
            return baseHP * ((index*5)/2);
        }
    }
}
