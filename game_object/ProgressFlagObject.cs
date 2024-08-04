using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class ProgressFlagObject
    {
        public bool CustomPartyMembersViewed { get; set; }
        public bool PrimaryWeaponEquippedForFirstTime { get; set; }
        public List<bool> DCCutsceneSeen { get; set; }

        public ProgressFlagObject()
        {
            DCCutsceneSeen = new List<bool>();
        }
    }
}
