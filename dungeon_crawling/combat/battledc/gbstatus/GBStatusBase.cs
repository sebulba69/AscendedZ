using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbstatus
{
    public class GBStatusBase
    {
        protected string _name, _icon;
        protected GBEntity _owner;

        public GBEntity Owner { get { return _owner; } }
        public string Name { get { return _name; } }
        public string Icon { get { return _icon; } }
    }
}
