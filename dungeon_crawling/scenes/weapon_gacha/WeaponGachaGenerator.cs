using AscendedZ.dungeon_crawling.combat.battledc.gbskills;
using AscendedZ.dungeon_crawling.combat.battledc.gbstatus;
using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.dungeon_crawling.scenes.weapon_gacha
{
    public class WeaponGachaGenerator
    {
        private Random _random;

        private const long DEFAULT_MIN_HP = 15;
        private const long DEFAULT_MAX_HP = 20;
        private const long DEFAULT_MIN_ATK = 5;
        private const long DEFAULT_MAX_ATK = 10;

        private readonly Dictionary<WeaponType, string> _weaponIconValuePairs = new Dictionary<WeaponType, string>()
        {
            { WeaponType.Dagger, SkillAssets.DAGGER_ICON},
            { WeaponType.Sword, SkillAssets.SWORD_ICON},
            { WeaponType.Whip, SkillAssets.WHIP_ICON},
            { WeaponType.Staff, SkillAssets.STAFF_ICON},
            { WeaponType.Bow, SkillAssets.BOW_ICON},
            { WeaponType.Greatsword, SkillAssets.GREATSWORD_ICON}
        };

        public WeaponGachaGenerator()
        {
            _random = new Random();
        }

        public List<Weapon> GenerateWeapons(int number, int tier)
        {
            List<Weapon> weapons = new List<Weapon>();

            var elements = Enum.GetValues<Elements>().ToList();

            List<Func<int, Weapon>> weaponGenFunctions = new List<Func<int, Weapon>>()  { MakeDagger, MakeSword, MakeWhip, MakeStaff, MakeBow, MakeGreatsword };

            for(int w = 0; w < number; w++)
            {
                Weapon weapon = weaponGenFunctions[_random.Next(weaponGenFunctions.Count)].Invoke(tier);
                weapon.Element = elements[_random.Next(elements.Count)];
                weapons.Add(weapon);
            }

            return weapons;
        }

        // normal HP
        // low attack
        // high critrate
        private Weapon MakeDagger(int tier)
        {
            Weapon dagger = MakeWeaponBase(tier, WeaponType.Dagger);
            dagger.CritChance = 0.5;

            return dagger;
        }

        // normal HP
        // normal attack
        // normal critrate
        private Weapon MakeSword(int tier)
        {
            Weapon sword = MakeWeaponBase(tier, WeaponType.Sword);
            return sword;
        }

        // very low attack, high hitrate (3), very high critrate
        private Weapon MakeWhip(int tier)
        {
            Weapon whip = MakeWeaponBase(tier, WeaponType.Whip);

            whip.Attack = (long)(whip.Attack * .75);
            whip.CritChance = 0.75;
            whip.HitRate = 3;

            return whip;
        }

        // very high HP, 0 crit rate
        private Weapon MakeStaff(int tier)
        {
            Weapon staff = MakeWeaponBase(tier, WeaponType.Staff);

            staff.HP *= 3;
            staff.CritChance = 0;

            return staff;
        }

        private Weapon MakeBow(int tier)
        {
            Weapon bow = MakeWeaponBase(tier, WeaponType.Bow);

            bow.Attack = (long)(bow.Attack * .75);
            bow.CritChance = 0.5;
            bow.HitRate = 2;

            return bow;
        }

        private Weapon MakeGreatsword(int tier)
        {
            Weapon greatsword = MakeSword(tier);

            greatsword.Type = WeaponType.Greatsword;
            greatsword.Icon = _weaponIconValuePairs[greatsword.Type];
            greatsword.Attack *= 3;
            greatsword.HP *= 2;
            greatsword.CritChance = 0.05;

            return greatsword;
        }

        private Weapon MakeWeaponBase(int tier, WeaponType type)
        {
            string icon = _weaponIconValuePairs[type];

            return new Weapon()
            {
                Level = tier,
                Icon = icon,
                Type = type,
                HP = GetRandomHP(tier),
                Attack = GetRandomAttack(tier)
            };
        }

        private long GetRandomHP(int tier)
        {
            long min = DEFAULT_MIN_HP + (tier * 2);
            long max = DEFAULT_MAX_HP + (tier * 2);
            
            return _random.NextInt64(min, max + 1);
        }

        private long GetRandomAttack(int tier)
        {
            long min = DEFAULT_MIN_ATK * tier;
            long max = DEFAULT_MAX_ATK * tier;

            return _random.NextInt64(min, max + 1);
        }
    }
}
