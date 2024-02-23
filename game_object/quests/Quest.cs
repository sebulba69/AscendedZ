using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object.quests
{
    /// <summary>
    /// Do not make an instance of this class. This is just to hold common
    /// properties shared among quests.
    /// </summary>
    public class Quest
    {
        private bool _completed = false;
        private bool _registered = false;
        private int _vorpexReward = 10;

        public bool Completed { get => _completed; set => _completed = value; }
        public bool Registered { get => _registered; set => _registered = value; }
        public int VorpexReward { get => _vorpexReward; set => _vorpexReward = value; }
    }
}
