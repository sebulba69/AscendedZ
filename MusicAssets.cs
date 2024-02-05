using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        /// <summary>
        /// List of tracks in order the game plays them.
        /// </summary>
        private static readonly List<string> OVERWORLD_TRACKS = new List<string> 
        {
            OVERWORLD_1, OVERWORLD_2, OVERWORLD_3, OVERWORLD_4
        };
        
        private static readonly Dictionary<string, string> OVERWORLD_DICT = new Dictionary<string, string>
        {
            { OVERWORLD_1, "res://music/overworld/overworld1.ogg" },
            { OVERWORLD_2, "res://music/overworld/hollow_bastion.ogg" },
            { OVERWORLD_3, "res://music/overworld/underworld.ogg" },
            { OVERWORLD_4, "res://music/overworld/sound_test_nctrn.ogg" }
        };

        public static readonly string DUNGEON1_4 = "res://music/dungeons/dungeon1-4.ogg";
        public static readonly string DUNGEON5 = "res://music/dungeons/dungeon5.ogg";
        public static readonly string DUNGEON6_9 = "res://music/dungeons/dungeon6-9.ogg";
        public static readonly string DUNGEON10 = "res://music/dungeons/dungeon10.ogg";
        public static readonly string[] DUNGEON_TRACKS_REAL = { "res://music/dungeons/dungeon11-19.ogg" };

        public static readonly string BOSS_VICTORY = "res://music/boss_victory.ogg";
        public static readonly string FIRST_CUTSCENE = "res://music/cutscene.ogg";

        public static string GetOverworldTrackNormal()
        {
            int tier = PersistentGameObjects.GameObjectInstance().MaxTier;
            int index = 0;
            if(tier > 10)
                index = (tier - (tier % 10)) / 10;

            string trackKey = OVERWORLD_TRACKS[index];
            return trackKey;
        }

        /// <summary>
        /// Returns the track path for the overworld key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetOverworldTrackPath(string key)
        {
            return OVERWORLD_DICT[key];
        }

        public static List<string> GetOverworldTrackKeys()
        {
            return OVERWORLD_DICT.Keys.ToList<string>();
        }

        public static string GetDungeonTrack(int tier)
        {
            // tiers 5 - 10 have special tracks
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
            else if(tier == 10)
            {
                return DUNGEON10;
            }
            else if(tier > 10)
            {
                int index = ((tier - (tier % 10))/10) - 1;
                
                if (index > DUNGEON_TRACKS_REAL.Length)
                    index = 0;
                return DUNGEON_TRACKS_REAL[index];
            }
            else
            {
                return "temp";
            }
        }
    }
}
