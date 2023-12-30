using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class GameObject
    {
        private const int TIER_CAP = 9;

        private int _tier = 1;
        private int _maxTier = 1;

        /// <summary>
        /// Path for saving
        /// </summary>
        public string SavePath { get; set; }

        public List<SaveEntry> SaveCache { get; set; }

        /// <summary>
        /// The current floor you're on as displayed to you by the UI.
        /// </summary>
        public int Tier
        {
            get => _tier;
            set
            {
                _tier = value;
                if (_tier >= MaxTier)
                    _tier = MaxTier;

                if (_tier < 1)
                    _tier = 1;
            }
        }

        /// <summary>
        /// The highest possible tier you can get to at your current point in the game.
        /// </summary>
        public int MaxTier
        {
            get => _maxTier;
            set
            {
                _maxTier = value;
                if (_maxTier > TIER_CAP)
                    _maxTier = TIER_CAP;
            }
        }

        public MainPlayer MainPlayer { get; set; }

        public MusicObject MusicPlayer { get; set; }

        /// <summary>
        /// Randomly generated encounters saved for re-use. Starts at Tier 6.
        /// </summary>
        public List<List<string>> Encounters { get; set; }

        public GameObject()
        {
            if (MusicPlayer == null)
                MusicPlayer = new MusicObject();

            if (Encounters == null)
                Encounters = new List<List<string>>();
        }

        /// <summary>
        /// Load the save cache from your save file, or creat it if none exist.
        /// </summary>
        public void Initialize(string saveCachePath)
        {
            SaveCache = JsonUtil.LoadObject<List<SaveEntry>>(saveCachePath);

            // if we didn't load anything, then initialize a new savecache
            if (SaveCache == null)
            {
                SaveCache = new List<SaveEntry>();
            }
        }

        public void SaveSaveCache(string saveCachePath)
        {
            JsonUtil.SaveObject(SaveCache, saveCachePath);
        }

        public List<BattlePlayer> MakeBattlePlayerListFromParty()
        {
            List<BattlePlayer> players = new List<BattlePlayer>();
            foreach (var member in MainPlayer.Party.Party)
            {
                if (member != null)
                {
                    players.Add(member.MakeBattlePlayer());
                }
            }
            return players;
        }


    }
}
