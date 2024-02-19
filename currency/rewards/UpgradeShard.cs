using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public partial class UpgradeShard : Currency
    {
        public UpgradeShard()
        {
            this.Name = "Upgrade Shard";
            this.Icon = SkillAssets.UPGRADESHARD_ICON;
        }
    }
}
