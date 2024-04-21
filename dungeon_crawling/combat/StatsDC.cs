using AscendedZ.json_interface_converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat
{
    /// <summary>
    /// Boosts applied to stats
    /// </summary>
    public class StatsDC
    {
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger HP { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger MP { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger AttackRate { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger HealAmount { get; set; } = new BigInteger(0);

        public int CriticalRate { get; set; } = 10;

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger CriticalBoost { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger WeaknessBoost { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger Fire { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger Ice { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger Wind { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger Elec { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger Dark { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger Light { get; set; } = new BigInteger(0);
    }
}
