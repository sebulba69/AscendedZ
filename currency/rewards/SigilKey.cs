using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency.rewards
{
    public class SigilKey : Currency
    {
        public int Level { get; set; }

        public SigilKey()
        {
            this.Level = 1;
            this.Name = "Sigil Key";
            this.Icon = ArtAssets.SIGILKEY_ICON;
        }
    }
}
