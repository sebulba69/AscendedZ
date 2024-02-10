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
        private const int TIER_CAP = 21;

        private int _tier = 1;
        private int _maxTier = 1;
        private bool _partyMemberObtained = false;

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

        public int TierCap { get => TIER_CAP; }

        public bool PartyMemberObtained { get => _partyMemberObtained; set => _partyMemberObtained = value; }

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
