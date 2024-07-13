using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public class MinionShard : Currency
    {
        public MinionShard()
        {
            Name = SkillAssets.MINION_SHARD;
            Icon = Name;
        }
    }
}
