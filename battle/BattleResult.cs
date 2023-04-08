using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle
{
    public enum BattleResultType { Wk, Rs, Nu, Dr, Normal, HPGain, StatusApplied, Pass, Retreat }

    /// <summary>
    /// This is the class the UI is going to use to know what information needs to be shown on the screen
    /// after an interaction
    /// </summary>
    public class BattleResult
    {
        public BattleResultType ResultType { get; set; }

        /// <summary>
        /// Can represent Damage taken or HP gained
        /// </summary>
        public int HPChanged { get; set; }
        public BattleEntity User { get; set; }
        public BattleEntity Target { get; set; }
        public ISkill SkillUsed { get; set; }

        public BattleResult() {}

        public string GetResultString()
        {
            string result;

            if (ResultType == BattleResultType.Wk)
                result = "WEAK";
            else if (ResultType == BattleResultType.Rs)
                result = "RESIST";
            else if (ResultType == BattleResultType.Nu)
                result = "VOID";
            else
                result = string.Empty;

            return result;
        }
    }
}
