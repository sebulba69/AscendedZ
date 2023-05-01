using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class GameObject
    {
        private string _currentSong = "";
        private AudioStreamPlayer _streamPlayer;

        private const int TIER_CAP = 5;

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
                if(_tier >= MaxTier)
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

        public GameObject()
        {
            _streamPlayer = new AudioStreamPlayer();
        }

        /// <summary>
        /// Load the save cache from your save file, or creat it if none exist.
        /// </summary>
        public void Initialize(string saveCachePath)
        {
            this.SaveCache = JsonUtil.LoadObject<List<SaveEntry>>(saveCachePath);

            // if we didn't load anything, then initialize a new savecache
            if (this.SaveCache == null)
            {
                this.SaveCache = new List<SaveEntry>();
            }
        }

        public void SaveSaveCache(string saveCachePath)
        {
            JsonUtil.SaveObject<List<SaveEntry>>(this.SaveCache, saveCachePath);
        }

        public List<BattlePlayer> MakeBattlePlayerListFromParty()
        {
            List<BattlePlayer> players = new List<BattlePlayer>();
            foreach(var member in this.MainPlayer.Party.Party)
            {
                if (member != null)
                {
                    players.Add(member.MakeBattlePlayer());
                }
            }
            return players;
        }

        /// <summary>
        /// Change the current stream player with a new one.
        /// This will stop the current music playing so we can host new music from
        /// the new Stream Player. This will usually be needed when scene hopping.
        /// </summary>
        /// <param name="streamPlayer"></param>
        public void SetStreamPlayer(AudioStreamPlayer streamPlayer)
        {
            if (_streamPlayer != null && _streamPlayer.Equals(streamPlayer))
                return;

            // stop the ongoing stream player if it's playing
            if (_streamPlayer.Playing)
                _streamPlayer.Stop();

            _streamPlayer = streamPlayer;
        }

        public void PlayMusic(string music)
        {
            if(music != _currentSong)
            {
                _currentSong = music;
                _streamPlayer.Stream = ResourceLoader.Load<AudioStream>(music);
                _streamPlayer.Play();
            }
        }
    }
}
