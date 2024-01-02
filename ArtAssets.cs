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
        private static readonly string TIER1_10 = "res://cg_backgrounds/bg00501.jpg";
        private static readonly string TIER11_20 = "res://cg_backgrounds/bg03401.jpg";
        private static readonly string TIER21_30 = "res://cg_backgrounds/bg00607.jpg";

        private static readonly string CBT_TIER1_10 = "res://cg_backgrounds/bg03101.jpg";
        private static readonly string CBT_TIER11_20 = "res://cg_backgrounds/bg02501.jpg";
        private static readonly string CBT_TIER21_30 = "res://cg_backgrounds/bg02402.jpg";

        public static string GetBackground(int maxTier)
        {
            if(maxTier > 20)
            {
                return TIER21_30;
            }
            else if(maxTier > 10)
            {
                return TIER11_20;
            }
            else
            {
                return TIER1_10;
            }
        }

        public static string GetCombatBackground(int maxTier)
        {
            if (maxTier > 20)
            {
                return CBT_TIER21_30;
            }
            else if (maxTier > 10)
            {
                return CBT_TIER11_20;
            }
            else
            {
                return CBT_TIER1_10;
            }
        }
    }

    /// <summary>
    /// Quick access to art assets using res paths
    /// </summary>
    public class CharacterImageAssets
    {
        private static readonly string IMAGE_DIR = "res://entity_pics/";
        private static List<string> _playerPics = new List<string>();

        public static List<string> PlayerPics
        {
            get
            {
                if (_playerPics.Count == 0)
                {
                    using (DirAccess dir = DirAccess.Open("res://player_pics/"))
                    {
                        dir.ListDirBegin();
                        string filename;
                        while (!string.IsNullOrEmpty(filename = dir.GetNext()))
                        {
                            filename = filename.Replace("png.import", "png");
                            string path = System.IO.Path.Combine(dir.GetCurrentDir(), filename);
                            _playerPics.Add(path);
                            filename = dir.GetNext();
                        }

                        dir.ListDirEnd();
                    }
                }
                return _playerPics;
            }
        }

        public static string GetImage(string name)
        {
            string nameToLower = name.ToLower();
            return $"{IMAGE_DIR}{nameToLower}.png";
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

        // Effects
        public static readonly string WIND_T1 = "wind1";
        public static readonly string FIRE_T1 = "fire1";
        public static readonly string ELEC_T1 = "elec1";
        public static readonly string ICE_T1 = "ice1";
        public static readonly string LIGHT_T1 = "light1";
        public static readonly string DARK_T1 = "dark1";
        public static readonly string HEAL_T1 = "heal1";
        public static readonly string STUN_T1 = "stun1";
        public static readonly string AGRO = "agro";
        public static readonly string VOID_SHIELD = "void_shield";

        // ICONS + ICON_STRINGS
        public static readonly string ICON_ATLAS = "res://misc_icons/IconSet.png";

        // Skill Icons
        public static readonly string FIRE_ICON = "Fire";
        public static readonly string VOID_FIRE_ICON = "VoidFire";
        public static readonly string ICE_ICON = "Ice";
        public static readonly string ELEC_ICON = "Elec";
        public static readonly string WIND_ICON = "Wind";
        public static readonly string LIGHT_ICON = "Light";
        public static readonly string DARK_ICON = "Dark";
        public static readonly string HEAL_ICON = "Heal";
        public static readonly string STUN_ICON = "Stun";
        public static readonly string AGRO_ICON = "Agro";
        public static readonly string PASS_ICON = "Pass";
        public static readonly string RETREAT_ICON = "Retreat";

        // Currency Icons
        public static readonly string VORPEX_ICON = "Vorpex";

        public static readonly System.Collections.Generic.Dictionary<string, KeyValuePair<int, int>> ICONS = new System.Collections.Generic.Dictionary<string, KeyValuePair<int, int>>()
        {
            [FIRE_ICON] = new KeyValuePair<int, int>(0, 128),
            [VOID_FIRE_ICON] = new KeyValuePair<int, int>(32, 2144),
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
            [VORPEX_ICON] = new KeyValuePair<int, int>(448, 1376)
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
                case Elements.Fir:
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
            var tier1Animations = new System.Collections.Generic.Dictionary<Elements, string> { 
                { Elements.Fir, FIRE_T1 },
                { Elements.Ice, ICE_T1 },
                { Elements.Wind, WIND_T1 },
                { Elements.Elec, ELEC_T1 },
                { Elements.Dark, DARK_T1 },
                { Elements.Light, LIGHT_T1 } 
            };

            string animation = string.Empty;

            if(tier == 1)
                animation = tier1Animations[element];

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
