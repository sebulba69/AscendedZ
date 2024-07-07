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

        private const long DEFAULT_MIN_HP = 5;
        private const long DEFAULT_MAX_HP = 10;
        private const long DEFAULT_MIN_ATK = 10;
        private const long DEFAULT_MAX_ATK = 20;

        private readonly Dictionary<WeaponType, string> _weaponIconValuePairs = new Dictionary<WeaponType, string>()
        {
            { WeaponType.Dagger, SkillAssets.DAGGER_ICON},
            { WeaponType.Sword, SkillAssets.SWORD_ICON},
            { WeaponType.Flail, SkillAssets.FLAIL_ICON},
            { WeaponType.Axe, SkillAssets.AXE_ICON},
            { WeaponType.Whip, SkillAssets.WHIP_ICON},
            { WeaponType.Staff, SkillAssets.STAFF_ICON},
            { WeaponType.Bow, SkillAssets.BOW_ICON},
            { WeaponType.Crossbow, SkillAssets.CROSSBOW_ICON},
            { WeaponType.Flintlock, SkillAssets.FLINTLOCK_ICON},
            { WeaponType.Claw, SkillAssets.CLAW_ICON},
            { WeaponType.Spear, SkillAssets.SPEAR_ICON},
            { WeaponType.Greatsword, SkillAssets.GREATSWORD_ICON},
            { WeaponType.Hammer, SkillAssets.HAMMER_ICON}
        };

        public WeaponGachaGenerator()
        {
            _random = new Random();
        }

        public List<Weapon> GenerateWeapons(int number, int tier)
        {
            List<Weapon> weapons = new List<Weapon>();

            var elements = Enum.GetValues<Elements>().ToList();

            List<Func<int, Weapon>> weaponGenFunctions = new List<Func<int, Weapon>>() 
            {
                MakeDagger, MakeSword, MakeFlail,
                MakeAxe, MakeWhip, MakeStaff,
                MakeBow, MakeCrossbow, MakeFlintlock,
                MakeClaw, MakeSpear, MakeGreatsword,
                MakeHammer
            };

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
            dagger.Attack /= 2;
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

        // high attack, low HP, low critRate
        private Weapon MakeFlail(int tier)
        {
            Weapon flail = MakeWeaponBase(tier, WeaponType.Flail);

            flail.HP /= 2;
            flail.Attack *= 2;
            flail.CritChance = 0.05;

            return flail;
        }

        // very high attack, very low HP, low critrate
        private Weapon MakeAxe(int tier)
        {
            Weapon axe = MakeWeaponBase(tier, WeaponType.Axe);

            axe.HP /= 3;
            axe.Attack *= 3;
            axe.CritChance = 0.5;

            return axe;
        }

        // very low attack, very low HP, high hitrate (3), very high critrate
        private Weapon MakeWhip(int tier)
        {
            Weapon whip = MakeWeaponBase(tier, WeaponType.Whip);

            whip.HP /= 3;
            whip.Attack /= 3;
            whip.CritChance = 0.75;
            whip.HitRate = 3;

            return whip;
        }

        // very high HP, 0 crit rate
        private Weapon MakeStaff(int tier)
        {
            Weapon staff = MakeWeaponBase(tier, WeaponType.Staff);

            staff.HP *= 3;
            staff.Attack /= 2;
            staff.CritChance = 0;

            return staff;
        }

        private Weapon MakeBow(int tier)
        {
            Weapon bow = MakeWeaponBase(tier, WeaponType.Bow);

            bow.HP /= 3;
            bow.Attack *= 2;
            bow.CritChance = 0.5;
            bow.HitRate = 2;

            return bow;
        }

        private Weapon MakeCrossbow(int tier)
        {
            Weapon crossbow = MakeBow(tier);

            crossbow.Type = WeaponType.Crossbow;
            crossbow.Icon = _weaponIconValuePairs[crossbow.Type];
            crossbow.HitRate = 1;
            crossbow.HP *= 2;

            return crossbow;
        }

        private Weapon MakeFlintlock(int tier)
        {
            Weapon flintlock = MakeBow(tier);
            Weapon crossbow = MakeCrossbow(tier);

            flintlock.Type = WeaponType.Flintlock;
            flintlock.Icon = _weaponIconValuePairs[flintlock.Type];
            flintlock.HP += crossbow.HP;
            flintlock.Attack += crossbow.Attack;
            flintlock.CritChance += crossbow.CritChance;
            flintlock.HitRate = 1;

            return flintlock;
        }

        private Weapon MakeClaw(int tier)
        {
            Weapon claw = MakeDagger(tier);

            claw.Type = WeaponType.Claw;
            claw.Icon = _weaponIconValuePairs[claw.Type];
            claw.HitRate = 3;
            claw.CritChance = 0.75;

            return claw;
        }

        private Weapon MakeSpear(int tier)
        {
            Weapon spear = MakeClaw(tier);

            
            spear.Type = WeaponType.Spear;
            spear.Icon = _weaponIconValuePairs[spear.Type];
            spear.HitRate = 1;
            spear.CritChance = 0.5;
            spear.Attack *= 3;

            return spear;
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

        private Weapon MakeHammer(int tier)
        {
            Weapon hammer = MakeGreatsword(tier);

            hammer.Type = WeaponType.Hammer;
            hammer.Icon = _weaponIconValuePairs[hammer.Type];
            hammer.CritChance = 0;

            return hammer;
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
