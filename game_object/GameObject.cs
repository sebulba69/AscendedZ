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
        private const int TIER_CAP = 41;
        private const int TIER_CAP_DC = 999;

        private bool _oldAlphaSave = true;
        private int _tier = 1;
        private int _tierDC = 1;
        private int _maxTier = 1;
        private int _maxTierDC = 1;
        private bool _partyMemberObtained = false;
        private bool _upgradeShardsUnlocked = false;

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
        /// The current floor you're on as displayed to you by the UI.
        /// </summary>
        public int TierDC
        {
            get => _tierDC;
            set
            {
                _tierDC = value;
                if (_tierDC >= MaxTierDC)
                    _tierDC = MaxTierDC;

                if (_tierDC < 1)
                    _tierDC = 1;
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

        /// <summary>
        /// The highest possible dungeon crawling tier you can get to at your current point in the game.
        /// </summary>
        public int MaxTierDC
        {
            get => _maxTierDC;
            set
            {
                _maxTierDC = value;
                if (_maxTierDC > TIER_CAP_DC)
                    _maxTierDC = TIER_CAP_DC;
            }
        }

        public bool UpgradeShardsUnlocked { get => _upgradeShardsUnlocked; set => _upgradeShardsUnlocked = value; }

        public int TierCap { get => TIER_CAP; }

        public bool PartyMemberObtained { get => _partyMemberObtained; set => _partyMemberObtained = value; }

        public MainPlayer MainPlayer { get; set; }

        public MusicObject MusicPlayer { get; set; }

        public QuestObject QuestObject { get; set; }
        public ProgressFlagObject ProgressFlagObject { get; set; }
        /// <summary>
        /// Randomly generated encounters saved for re-use. Starts at Tier 6.
        /// </summary>
        public List<List<string>> Encounters { get; set; }

        public bool OldAlphaSave { get => _oldAlphaSave; set => _oldAlphaSave = value; }

        public GameObject()
        {
            if (MusicPlayer == null)
                MusicPlayer = new MusicObject();

            if (QuestObject == null)
                QuestObject = new QuestObject();

            if (ProgressFlagObject == null)
                ProgressFlagObject = new ProgressFlagObject();

            if (Encounters == null)
                Encounters = new List<List<string>>();
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
