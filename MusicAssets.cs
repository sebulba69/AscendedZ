using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class MusicAssets
    {
        public static readonly string OVERWORLD_1 = "[DDS1] River of Samsara";
        public static readonly string OVERWORLD_2 = "[KH2] Hollow Bastion";
        public static readonly string OVERWORLD_3 = "[KH2] Underworld";
        public static readonly string OVERWORLD_4 = "[SMT3] Sound Test";

        private static readonly Dictionary<string, string> OVERWORLD_MUSIC = new Dictionary<string, string>
        {
            { OVERWORLD_1, "res://music/overworld/overworld1.ogg" },
            { OVERWORLD_2, "res://music/overworld/hollow_bastion.ogg" },
            { OVERWORLD_3, "res://music/overworld/underworld.ogg" },
            { OVERWORLD_4, "res://music/overworld/sound_test_nctrn.ogg" }
        };

        public static readonly string DUNGEON1_4 = "res://music/dungeons/dungeon1-4.ogg";
        public static readonly string DUNGEON5 = "res://music/dungeons/dungeon5.ogg";
        public static readonly string DUNGEON6_9 = "res://music/dungeons/dungeon6-9.ogg";

        public static string GetOverworldTrack(string key)
        {
            return OVERWORLD_MUSIC[key];
        }

        public static List<string> GetOverworldTrackKeys()
        {
            return OVERWORLD_MUSIC.Keys.ToList<string>();
        }

        public static string GetDungeonTrack(int tier)
        {
            if(tier < 5)
            {
                return DUNGEON1_4;
            }
            else if(tier == 5)
            {
                return DUNGEON5;
            }
            else if(tier > 5 && tier <= 9)
            {
                return DUNGEON6_9;
            }
            else
            {
                return "temp";
            }
        }
    }
}
