using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.battle_entities
{
    /// <summary>
    /// We inherit from BattleEntity because we want to have different classes to differentiate our
    /// enemies from our players. It's just cleaner this way.
    /// </summary>
    public class BattlePlayer : BattleEntity
    {
        public BattlePlayer():base() 
        {
            this.Turns = 1;
        }

        public override string GetLogName()
        {
            return $"[color=mediumaquamarine]{this.Name}[/color]";
        }
    }
}
