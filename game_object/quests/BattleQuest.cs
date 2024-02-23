using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object.quests
{
    /// <summary>
    /// Check this quest when you're at the end of a battle.
    /// </summary>
    public class BattleQuest : Quest
    {
        private List<string> _requiredPartyMembers = new List<string>();
        private int _minReqPartySize = 0;
        private int _reqTurnCount = 0;

        /// <summary>
        /// Tier to repeat.
        /// </summary>
        public int Tier { get; set; }

        /// <summary>
        /// List of PartyNames
        /// </summary>
        public List<string> RequiredPartyMembers { get => _requiredPartyMembers; set => _requiredPartyMembers = value; }
        
        public int MinReqPartySize { get => _minReqPartySize; set => _minReqPartySize = value; }
        
        public int ReqTurnCount { get => _reqTurnCount; set => _reqTurnCount = value; }
    }
}
