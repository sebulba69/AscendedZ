using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.player_combat_elements
{
   
    public class Weapon
    {
        private const int LEVEL_CAP = 999;

        public bool PrimaryWeapon { get; set; }
        public bool Equipped { get; set; }
        public int Level { get; set; }
        public string Icon { get; set; }
        public WeaponType Type { get; set; }
        public Elements Element { get; set; }
        public long HP { get; set; }
        public long Attack { get; set; }
        public int HitRate { get; set; } // the number of hits you get
        public double CritChance { get; set; }
        public int XP { get; set; }
        public int XPRequired { get; set; }

        public Weapon()
        {
            Level = 1;
            HitRate = 1;
            CritChance = 0.15;
            Equipped = false;
            PrimaryWeapon = false;
            XP = 0;
            XPRequired = 10;
        }

        public void AddXP(int xp)
        {
            XP += xp;
            while (XP > XPRequired) 
            {
                XP = XP - XPRequired;
                LevelUp();
            }
        }

        private void LevelUp()
        {
            if (Level + 1 > LEVEL_CAP)
                return;

            double percentageIncrease = 0.1;

            Level++;
            HP += (long)(HP * percentageIncrease) + 1;
            Attack += (long)(Attack * percentageIncrease) + 1;
            XPRequired += 10;
        }

        public string GetArmoryDisplayString()
        {
            string displayString = $"L.{Level} {Type} {Element} ({XP}/{XPRequired})\n{HP} HP ● {Attack} ATK\nHitx{HitRate} ● {CritChance * 100}% CRT";

            if (PrimaryWeapon && Equipped)
                displayString = "[PW] " + displayString;
            else if (Equipped)
                displayString = "[E] " + displayString;

            return displayString;
        }
    }
}
