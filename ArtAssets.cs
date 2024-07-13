
using AscendedZ.entities;
using AscendedZ.skills;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class BackgroundAssets
    {
        private static List<string> _overworldBgs = new List<string>();

        public static List<string> OverworldBgs
        {
            get
            {
                AssetUtil.GetFilesFromDir(_overworldBgs, "res://cg_backgrounds/overworld/");
                return _overworldBgs;
            }
        }

        private static List<string> _combatBgs = new List<string>();

        public static List<string> CombatBgs
        {
            get
            {
                AssetUtil.GetFilesFromDir(_combatBgs, "res://cg_backgrounds/dungeon/");
                return _combatBgs;
            }
        }

        public static string GetBackground(int tier)
        {
            int index = Equations.GetTierIndexBy10(tier);
            return OverworldBgs[index];
        }

        public static string GetCombatBackground(int tier)
        {
            int index = Equations.GetTierIndexBy10(tier);
            return CombatBgs[index];
        }
    }

    /// <summary>
    /// Quick access to art assets using res paths
    /// </summary>
    public class CharacterImageAssets
    {
        private static List<string> _playerPics = new List<string>();

        public static List<string> PlayerPics
        {
            get
            {
                AssetUtil.GetFilesFromDir(_playerPics, "res://player_pics/");
                return _playerPics;
            }
        }

        public static string GetImagePath(string name)
        {
            string nameToLower = name.ToLower();
            return $"res://entity_pics/{nameToLower}.png";
        }

        public static Texture2D GetTextureForItemList(string imagePath)
        {
            Texture2D texture = ResourceLoader.Load<Texture2D>(imagePath);

            Image image = texture.GetImage();
            image.Resize(32, 32);

            return ImageTexture.CreateFromImage(image);
        }
    }

    public class SkillAssets
    {
        // Startups
        public static readonly string STARTUP1_MG = "startup_anim";

        // Effect Animations
        public static readonly string WIND_T1 = "wind1";
        public static readonly string FIRE_T1 = "fire1";
        public static readonly string ELEC_T1 = "elec1";
        public static readonly string ICE_T1 = "ice1";
        public static readonly string LIGHT_T1 = "light1";
        public static readonly string DARK_T1 = "dark1";

        public static readonly string WIND_T2 = "wind2";
        public static readonly string FIRE_T2 = "fire2";
        public static readonly string ELEC_T2 = "elec2";
        public static readonly string ICE_T2 = "ice2";
        public static readonly string LIGHT_T2 = "light2";
        public static readonly string DARK_T2 = "dark2";

        public static readonly string WIND_T3 = "wind3";
        public static readonly string FIRE_T3 = "fire3";
        public static readonly string ELEC_T3 = "elec3";
        public static readonly string ICE_T3 = "ice3";
        public static readonly string LIGHT_T3 = "light3";
        public static readonly string DARK_T3 = "dark3";

        public static readonly string HEAL_T1 = "heal1";
        public static readonly string STUN_T1 = "stun1";
        public static readonly string AGRO = "agro";
        public static readonly string VOID_SHIELD = "void_shield";

        // ICONS + ICON_STRINGS
        public static readonly string ICON_ATLAS = "res://misc_icons/IconSet.png";

        // Skill Icons - Elements
        public static readonly string FIRE_ICON = "Fire";
        public static readonly string ICE_ICON = "Ice";
        public static readonly string ELEC_ICON = "Elec";
        public static readonly string WIND_ICON = "Wind";
        public static readonly string LIGHT_ICON = "Light";
        public static readonly string DARK_ICON = "Dark";

        // Skill Icons - Voids
        public static readonly string VOID_FIRE_ICON = "VoidFire";
        public static readonly string VOID_ICE_ICON = "VoidIce";
        public static readonly string VOID_WIND_ICON = "VoidWind";

        // Skill Icons - Weaks
        public static readonly string WEAK_FIRE_ICON = "WeakFire";
        public static readonly string WEAK_ELEC_ICON = "WeakElec";

        // Skill Icons - Other
        public static readonly string HEAL_ICON = "Heal";
        public static readonly string STUN_ICON = "Stun";
        public static readonly string AGRO_ICON = "Agro";
        public static readonly string PASS_ICON = "Pass";
        public static readonly string RETREAT_ICON = "Retreat";

        public static readonly string MAGIC_ICON = "Magic";

        public static readonly string QUEST_ICON = "Quest";

        // Currency Icons
        public static readonly string VORPEX_ICON = "Vorpex";
        public static readonly string PARTY_COIN_ICON = "Party Coins";
        public static readonly string DELLENCOIN = "Dellencoin";
        public static readonly string MINION_SHARD = "Minion Shards";

        // Dungeon Crawling Icons
        public static readonly string DAGGER_ICON = "Dagger";
        public static readonly string SWORD_ICON = "Sword";
        public static readonly string FLAIL_ICON = "Flail";
        public static readonly string AXE_ICON = "Axe";
        public static readonly string WHIP_ICON = "Whip";
        public static readonly string STAFF_ICON = "Staff";
        public static readonly string BOW_ICON = "Bow";
        public static readonly string CROSSBOW_ICON = "Crossbow";
        public static readonly string FLINTLOCK_ICON = "Flintlock";
        public static readonly string CLAW_ICON = "Claw";
        public static readonly string SPEAR_ICON = "Spear";
        public static readonly string GREATSWORD_ICON = "Greatsword";
        public static readonly string HAMMER_ICON = "Hammer";

        // GB Statuses

        public static readonly System.Collections.Generic.Dictionary<string, KeyValuePair<int, int>> ICONS = new System.Collections.Generic.Dictionary<string, KeyValuePair<int, int>>()
        {
            [FIRE_ICON] = new KeyValuePair<int, int>(0, 128),
            [VOID_FIRE_ICON] = new KeyValuePair<int, int>(32, 2144),
            [VOID_ICE_ICON] = new KeyValuePair<int, int>(224, 2144),
            [VOID_WIND_ICON] = new KeyValuePair<int, int>(128, 2144),
            [ICE_ICON] = new KeyValuePair<int, int>(32, 128),
            [ELEC_ICON] = new KeyValuePair<int, int>(64, 128),
            [WIND_ICON] = new KeyValuePair<int, int>(160, 128),
            [LIGHT_ICON] = new KeyValuePair<int, int>(192, 128),
            [DARK_ICON] = new KeyValuePair<int, int>(224, 128),
            [HEAL_ICON] = new KeyValuePair<int, int>(256, 128),
            [STUN_ICON] = new KeyValuePair<int, int>(288, 0),
            [AGRO_ICON] = new KeyValuePair<int, int>(480, 0),
            [PASS_ICON] = new KeyValuePair<int, int>(352, 128),
            [RETREAT_ICON] = new KeyValuePair<int, int>(288, 576),
            [VORPEX_ICON] = new KeyValuePair<int, int>(448, 1376),
            [PARTY_COIN_ICON] = new KeyValuePair<int, int>(32, 288),
            [DELLENCOIN] = new KeyValuePair<int, int>(320, 608),
            [WEAK_FIRE_ICON] = new KeyValuePair<int, int>(32, 1600),
            [WEAK_ELEC_ICON] = new KeyValuePair<int, int>(96, 1600),
            [QUEST_ICON] = new KeyValuePair<int, int>(480, 352),
            [MAGIC_ICON] = new KeyValuePair<int, int>(384, 480),
            // DUNGEON CRAWLING WEAPONS
            [DAGGER_ICON] = new KeyValuePair<int, int>(0, 192),
            [SWORD_ICON] = new KeyValuePair<int, int>(32, 192),
            [FLAIL_ICON] = new KeyValuePair<int, int>(64, 192),
            [AXE_ICON] = new KeyValuePair<int, int>(96, 192),
            [WHIP_ICON] = new KeyValuePair<int, int>(128, 192),
            [STAFF_ICON] = new KeyValuePair<int, int>(160, 192),
            [BOW_ICON] = new KeyValuePair<int, int>(192, 192),
            [CROSSBOW_ICON] = new KeyValuePair<int, int>(224, 192),
            [FLINTLOCK_ICON] = new KeyValuePair<int, int>(256, 192),
            [CLAW_ICON] = new KeyValuePair<int, int>(288, 192),
            [SPEAR_ICON] = new KeyValuePair<int, int>(352, 192),
            [GREATSWORD_ICON] = new KeyValuePair<int, int>(32, 224),
            [HAMMER_ICON] = new KeyValuePair<int, int>(62, 224),
            [MINION_SHARD] = new KeyValuePair<int, int>(320, 1440)
        };

        public static KeyValuePair<int, int> GetIcon(string key)
        {
            if (string.IsNullOrEmpty(key) || !ICONS.ContainsKey(key))
                throw new Exception($"{key} not present in dictionary.");

            return ICONS[key];
        }

        public static string GetElementIconByElementEnum(Elements element)
        {
            switch (element)
            {
                case Elements.Fire:
                    return FIRE_ICON;
                case Elements.Ice:
                    return ICE_ICON;
                case Elements.Elec:
                    return ELEC_ICON;
                case Elements.Wind:
                    return WIND_ICON;
                case Elements.Light:
                    return LIGHT_ICON;
                case Elements.Dark:
                    return DARK_ICON;
                default:
                    throw new Exception($"Element, {element.ToString()}, does not have icon.");
            }
        }

        public static string GetAnimationByElementAndTier(int tier, Elements element)
        {
            var tier1Animations = new System.Collections.Generic.Dictionary<Elements, string> 
            { 
                { Elements.Fire, FIRE_T1 },
                { Elements.Ice, ICE_T1 },
                { Elements.Wind, WIND_T1 },
                { Elements.Elec, ELEC_T1 },
                { Elements.Dark, DARK_T1 },
                { Elements.Light, LIGHT_T1 } 
            };

            var tier2Animations = new System.Collections.Generic.Dictionary<Elements, string> 
            {
                { Elements.Fire, FIRE_T2 },
                { Elements.Ice, ICE_T2 },
                { Elements.Wind, WIND_T2 },
                { Elements.Elec, ELEC_T2 },
                { Elements.Dark, DARK_T2 },
                { Elements.Light, LIGHT_T2 }
            };

            var tier3Animations = new System.Collections.Generic.Dictionary<Elements, string> 
            {
                { Elements.Fire, FIRE_T3 },
                { Elements.Ice, ICE_T3 },
                { Elements.Wind, WIND_T3 },
                { Elements.Elec, ELEC_T3 },
                { Elements.Dark, DARK_T3 },
                { Elements.Light, LIGHT_T3 }
            };

            string animation = string.Empty;

            switch (tier)
            {
                case 1:
                    animation = tier1Animations[element];
                    break;
                case 2:
                    animation = tier2Animations[element];
                    break;
                default:
                    animation = tier3Animations[element];
                    break;
            }

            return animation;
        }

        public static AtlasTexture GenerateIcon(string iconKey)
        {
            KeyValuePair<int, int> coords = GetIcon(iconKey);

            AtlasTexture icon = new AtlasTexture();
            icon.Atlas = ResourceLoader.Load<Texture2D>("res://misc_icons/IconSet.png");
            icon.Region = new Rect2(coords.Key, coords.Value, 32, 32);
            return icon;
        }
    }
}
