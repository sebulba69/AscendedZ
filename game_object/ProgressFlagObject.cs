using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class ProgressFlagObject
    {
        private bool _customPartyMembersViewed = false;

        public bool CustomPartyMembersViewed { get => _customPartyMembersViewed; set => _customPartyMembersViewed = value; }
    }
}
