using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.partymember_objects
{
    public class PlayerParty
    {
        private const int MAX = 4;
        private OverworldEntity[] _party;

        public OverworldEntity[] Party
        {
            get { return _party; }
            set { _party = value; }
        }

        public PlayerParty()
        {
            _party = new OverworldEntity[MAX];
        }

        /// <summary>
        /// Swap party members between slots.
        /// This function should only be used during battle.
        /// </summary>
        /// <param name="slot1"></param>
        /// <param name="slot2"></param>
        public void SwapSlots(int slot1, int slot2)
        {
            OverworldEntity party1 = _party[slot1];
            _party[slot1] = _party[slot2];
            _party[slot2] = party1;
        }
    }
}
