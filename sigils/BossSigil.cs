using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.sigils
{
    public class BossSigil
    {
        /// <summary>
        /// XP Required for this sigil to be usable. Starts at value
        /// provided on initialization, ends at 0.
        /// </summary>
        public int XPRequiredForUse { get; set; }

        /// <summary>
        /// Key (the name of the boss) for summoning a specific boss.
        /// </summary>
        public string BossKey { get; set; }
    }
}
