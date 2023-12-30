using AscendedZ.entities;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    /// <summary>
    /// Quick access to art assets using res paths
    /// </summary>
    public class PlayerPartyAssets
    {
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

        public static Dictionary<string, string> PartyMemberPics = new Dictionary<string, string> 
        {
            { PartyNames.LOCPHIEDON, "res://party_members/newpicture86.png" },
            { PartyNames.GAGAR, "res://party_members/newpicture56_Gagar.png" },
            { PartyNames.YUUDAM, "res://party_members/newpicture99.png" },
            { PartyNames.PECHEAL, "res://party_members/newpicture17_Pecheal_ice.png" },
            { PartyNames.TOKE, "res://party_members/newpicture60_toke_dark.png" },
            { PartyNames.MAXWALD, "res://party_members/newpicture57_maxwald_light.png" },
            { PartyNames.HALVIA, "res://party_members/halvia.png" }
        };
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

        // ICONS + ICON_STRINGS
        public static readonly string ICON_ATLAS = "res://misc_icons/IconSet.png";

        // Skill Icons
        public static readonly string FIRE_ICON = "Fire";
        public static readonly string ICE_ICON = "Ice";
        public static readonly string ELEC_ICON = "Elec";
        public static readonly string WIND_ICON = "Wind";
        public static readonly string LIGHT_ICON = "Light";
        public static readonly string DARK_ICON = "Dark";
        public static readonly string HEAL_ICON = "Heal";
        public static readonly string STUN_ICON = "Stun";
        public static readonly string PASS_ICON = "Pass";
        public static readonly string RETREAT_ICON = "Retreat";

        // Currency Icons
        public static readonly string VORPEX_ICON = "Vorpex";

        public static readonly Dictionary<string, KeyValuePair<int, int>> ICONS = new Dictionary<string, KeyValuePair<int, int>>()
        {
            [FIRE_ICON] = new KeyValuePair<int, int>(0, 128),
            [ICE_ICON] = new KeyValuePair<int, int>(32, 128),
            [ELEC_ICON] = new KeyValuePair<int, int>(64, 128),
            [WIND_ICON] = new KeyValuePair<int, int>(160, 128),
            [LIGHT_ICON] = new KeyValuePair<int, int>(192, 128),
            [DARK_ICON] = new KeyValuePair<int, int>(224, 128),
            [HEAL_ICON] = new KeyValuePair<int, int>(256, 128),
            [STUN_ICON] = new KeyValuePair<int, int>(288, 0),
            [PASS_ICON] = new KeyValuePair<int, int>(352, 128),
            [RETREAT_ICON] = new KeyValuePair<int, int>(288, 576),
            [VORPEX_ICON] = new KeyValuePair<int, int>(448, 1376)
        };

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

        public static AtlasTexture GenerateIcon(string iconKey)
        {
            KeyValuePair<int, int> coords = ICONS[iconKey];

            AtlasTexture icon = new AtlasTexture();
            icon.Atlas = ResourceLoader.Load<Texture2D>("res://misc_icons/IconSet.png");
            icon.Region = new Rect2(coords.Key, coords.Value, 32, 32);
            return icon;
        }
    }
}
