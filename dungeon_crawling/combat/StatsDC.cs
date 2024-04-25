using AscendedZ.dungeon_crawling.combat.skillsdc;
using AscendedZ.json_interface_converters;
using AscendedZ.resistances;
using AscendedZ.skills;
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
        public BigInteger Level { get; set; } = 1;

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger HP { get; set; } = new BigInteger(0);
        
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger MP { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger AP { get; set; } = 0;

        public List<Elements> Weak { get; set; } = new();
        public List<Elements> Resist { get; set; } = new();
        
        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger AttackRate { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger CriticalRate { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger HealAmount { get; set; } = new BigInteger(0);

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

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger XP { get; set; } = new BigInteger(0);

        [JsonConverter(typeof(BigIntegerConverter))]
        public BigInteger XPRequired { get; set; } = new BigInteger(10);

        public List<SkillDC> Skills { get; set; } = new List<SkillDC>();

        public void AddXP(BigInteger xp)
        {
            XP += xp;

            if (XP >= XPRequired)
            {
                LevelUp();
            }
        }

        public void ApplyStats(StatsDC baseStats)
        {
            baseStats.HP += HP;
            baseStats.MP += MP;
            baseStats.AP += AP;

            baseStats.AttackRate += AttackRate;
            baseStats.CriticalRate += CriticalRate;
            baseStats.CriticalBoost += CriticalBoost;
            baseStats.HealAmount += HealAmount;

            baseStats.Fire += Fire;
            baseStats.Ice += Ice;
            baseStats.Wind += Wind;
            baseStats.Elec += Elec;
            baseStats.Light += Light;
            baseStats.Dark += Dark;

            baseStats.Weak.AddRange(Weak);
            baseStats.Resist.AddRange(Resist);

            baseStats.Skills.AddRange(Skills);
        }

        public void RemoveStats(StatsDC baseStats)
        {
            baseStats.HP -= HP;
            baseStats.MP -= MP;
            baseStats.AP -= AP;

            baseStats.AttackRate -= AttackRate;
            baseStats.CriticalRate -= CriticalRate;
            baseStats.CriticalBoost -= CriticalBoost;
            baseStats.HealAmount -= HealAmount;

            baseStats.Fire -= Fire;
            baseStats.Ice -= Ice;
            baseStats.Wind -= Wind;
            baseStats.Elec -= Elec;
            baseStats.Light -= Light;
            baseStats.Dark -= Dark;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            
            builder.AppendLine($"Level: {Level}");

            AddStatToString(builder, "HP", HP);
            AddStatToString(builder, "MP", MP);
            AddStatToString(builder, "AP", AP);

            AddStatToString(builder, "Attack Rate", AttackRate);
            AddStatToString(builder, "Critical Rate", CriticalRate);
            AddStatToString(builder, "Weakness Boost", WeaknessBoost);
            AddStatToString(builder, "Heal Amount", HealAmount);

            builder.AppendLine($"Weak: {string.Join(", ", Weak)}");
            builder.AppendLine($"Resist: {string.Join(", ", Resist)}");

            AddStatToString(builder, "Fire", Fire);
            AddStatToString(builder, "Ice", Ice);
            AddStatToString(builder, "Wind", Wind);
            AddStatToString(builder, "Elec", Elec);
            AddStatToString(builder, "Dark", Dark);
            AddStatToString(builder, "Light", Light);

            builder.AppendLine(string.Join(", ", Skills));

            return builder.ToString();
        }

        private void LevelUp()
        {
            Level++;

            HP = LevelStat(HP, 5);
            MP = LevelStat(MP, 5);
            AP = LevelStat(AP, 1);

            AttackRate = LevelStat(AttackRate, 1);
            CriticalRate = LevelStat(CriticalRate, 1);
            WeaknessBoost = LevelStat(WeaknessBoost, 1);
            HealAmount = LevelStat(HealAmount, 1);

            Fire = LevelStat(Fire, 1);
            Ice = LevelStat(Ice, 1);
            Wind = LevelStat(Wind, 1);
            Elec = LevelStat(Elec, 1);
            Dark = LevelStat(Dark, 1);
            Light = LevelStat(Light, 1);

            foreach (var skill in Skills)
                skill.LevelUp();

            XP -= XPRequired;
            XPRequired *= 2;
        }

        private void AddStatToString(StringBuilder builder, string name, BigInteger stat)
        {
            if (stat > 0)
            {
                builder.AppendLine($"{name}: {stat}");
            }
        }

        private BigInteger LevelStat(BigInteger stat, BigInteger levelAmount)
        {
            BigInteger levelValue = 0;

            if(stat != 0)
            {
                levelAmount = stat + levelAmount;
            }

            return levelValue;
        }


    }
}
