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

        public int CurrentXP { get; set; }

        /// <summary>
        /// Key (the name of the boss) for summoning a specific boss.
        /// Generate in a separate class, not here.
        /// </summary>
        public string BossKey { get; set; }

        public BossSigil()
        {
            CurrentXP = 0;
        }

        public int GetCurrentXPPercentage()
        {
            return (int)((double)CurrentXP / XPRequiredForUse * 100);
        }

        public override string ToString()
        {
            string doneString = string.Empty;

            if (XPRequiredForUse == 0)
                doneString = $" [DONE]";
            else
                doneString = $" [{XPRequiredForUse}]";

            return $"{BossKey}{doneString}";
        }
    }
}
