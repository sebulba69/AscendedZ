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
        public static readonly string DC_BOSS_PRE = "res://music/dungeon_crawl_pre/dungeon_crawl_boss_pre.ogg";
        public static readonly string BOSS_VICTORY = "res://music/boss_victory.ogg";
        public static readonly string FIRST_CUTSCENE = "res://music/cutscene.ogg";

        public static List<string> OverworldTracks = new List<string>() 
        {
            "res://music/overworld/01. [DDS1] River of Samsara.ogg",
            "res://music/overworld/02. [KH2] Hollow Bastion.ogg",
            "res://music/overworld/03. [KH2] Underworld.ogg",
            "res://music/overworld/04. [Lunacid] Rain of Saint Ishii.ogg",
            "res://music/overworld/05. [SMT3] Sound Test.ogg",
            "res://music/overworld/06. [LoR] Caravan.ogg",
            "res://music/overworld/07. [LoR] Wasteland.ogg",
            "res://music/overworld/08. [LoG] Spring Watch.ogg",
            "res://music/overworld/09. [KH2R] This is Halloween.ogg",
            "res://music/overworld/10. [SMTIV] Tokyo.ogg"
        };

        private static List<string> DungeonTracksReal = new List<string>()
        {
            "res://music/dungeons_tiers/dungeon11-19.ogg",
            "res://music/dungeons_tiers/dungeon21-29.ogg",
            "res://music/dungeons_tiers/dungeon31-39.ogg",
            "res://music/dungeons_tiers/dungeon41-49.ogg",
            "res://music/dungeons_tiers/dungeon51-59.ogg",
            "res://music/dungeons_tiers/dungeon61-69.ogg",
            "res://music/dungeons_tiers/dungeon71-79.ogg",
            "res://music/dungeons_tiers/dungeon81-89.ogg",
            "res://music/dungeons_tiers/dungeon91-99.ogg",
        };

        public static string GetOverworldTrackNormal()
        {
            int tier = PersistentGameObjects.GameObjectInstance().MaxTier;
            int index = Equations.GetTierIndexBy10(tier);
            return OverworldTracks[index];
        }

        public static string GetDungeonTrack(int tier)
        {
            // tiers 5 - 10 have special tracks
            if(tier < 10)
            {
                return "res://music/dungeons_tutorial/dungeon1-4.ogg";
            }
            else if(tier >= 10)
            {
                if(tier % 10 == 0)
                {
                    return $"res://music/dungeon_bosses/dungeon{tier}.ogg";
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
            int index = Equations.GetTierIndexBy50(tier);
            return $"res://music/dungoen_crawl/dungeon_crawl_0{index + 1}.ogg";
        }
    }
}
