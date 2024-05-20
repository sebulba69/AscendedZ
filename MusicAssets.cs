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
        public static readonly string OW_MUSIC_FOLDER = "res://music/overworld/";
        public static readonly string OW_DC = "res://music/overworldDC.ogg";
        /// <summary>
        /// List of tracks in order the game plays them.
        /// </summary>
        private static readonly List<string> _overworldTracks = new List<string>();
        
        public static List<string> OverworldTracks
        {
            get
            {
                AssetUtil.GetFilesFromDir(_overworldTracks, OW_MUSIC_FOLDER);
                return _overworldTracks;
            }
        }

        private static readonly string BOSS_MUSIC = "res://music/dungeon_bosses/";
        private static readonly List<string> _bossTracks = new List<string>();

        public static List<string> BossTracks
        {
            get
            {
                AssetUtil.GetFilesFromDir(_bossTracks, BOSS_MUSIC);
                return _bossTracks;
            }
        }

        private static readonly string DUNGEON_REAL = "res://music/dungeons_tiers/";
        private static readonly List<string> _dungeonTrackReal = new List<string>();

        private static List<string> DungeonTracksReal
        {
            get
            {
                AssetUtil.GetFilesFromDir(_dungeonTrackReal, DUNGEON_REAL);
                return _dungeonTrackReal;
            }
        }

        private static readonly List<string> _dungeonTrackDCReal = new List<string>();
        private static readonly string DUNGEON_REAL_DC = "res://music/dungoen_crawl/";
        private static List<string> DungeonTracksDCReal
        {
            get
            {
                AssetUtil.GetFilesFromDir(_dungeonTrackDCReal, DUNGEON_REAL_DC);
                return _dungeonTrackDCReal;
            }
        }

        public static readonly string BOSS_VICTORY = "res://music/boss_victory.ogg";
        public static readonly string FIRST_CUTSCENE = "res://music/cutscene.ogg";

        public static string GetOverworldTrackNormal()
        {
            int tier = PersistentGameObjects.GameObjectInstance().MaxTier;
            int index = Equations.GetTierIndexBy10(tier);
            return OverworldTracks[index];
        }

        public static string GetDungeonTrack(int tier)
        {
            // tiers 5 - 10 have special tracks
            if(tier < 5)
            {
                return "res://music/dungeons_tutorial/dungeon1-4.ogg";
            }
            else if(tier == 5)
            {
                return "res://music/dungeons_tutorial/dungeon5.ogg";
            }
            else if(tier > 5 && tier <= 9)
            {
                return "res://music/dungeons_tutorial/dungeon6-9.ogg";
            }
            else if(tier >= 10)
            {
                if(tier % 10 == 0)
                {
                    return BossTracks[(tier / 10) - 1];
                }
                else
                {
                    int index = ((tier - (tier % 10)) / 10) - 1;

                    if (index > DungeonTracksReal.Count)
                        index = 0;

                    return DungeonTracksReal[index];
                }

            }
            else
            {
                return "temp";
            }
        }

        public static string GetDungeonTrackDC(int tier)
        {
            int index = ((tier - (tier % 10)) / 10) - 1;

            if (index > DungeonTracksDCReal.Count || index < 0)
                index = 0;

            return DungeonTracksDCReal[index];
        }
    }
}
