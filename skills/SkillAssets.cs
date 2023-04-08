using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    /// <summary>
    /// This file contains a collection of Skill-related enums
    /// </summary>
    
    // Target types -- each TargetType is from the perspective of the user.
    // all opponents, all teammates, single opponent, single teammate
    public enum TargetTypes
    {
        SINGLE_OPP, SINGLE_TEAM
    };

    // Elements
    public enum Elements
    {
        Fir, Ice, Elec, Wind, Dark, Light, None
    }
}
