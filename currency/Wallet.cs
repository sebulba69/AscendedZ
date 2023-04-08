using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency
{
    /// <summary>
    /// An object meant for storing complex currency objects.
    /// </summary>
    public partial class Wallet
    {
        public Dictionary<string, Currency> Currency { get; set; } = new();

        public Wallet() {}
    }
}
