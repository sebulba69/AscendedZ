using AscendedZ.dungeon_crawling.combat.skillsdc;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public class BDCEntity
    {
        public string Image { get; set; }
        protected StatsDC _statsDC;
        public StatsDC Stats { get => _statsDC; }

        public BDCEntity(StatsDC statsDC)
        {
            _statsDC = statsDC;
        }

        public void ApplySkill(SkillDC skill)
        {
            switch (skill.Id)
            {
                case SkillId.Elemental:
                    BigInteger damage = skill.Value;

                    if (Stats.Weak.Contains(skill.Element))
                        damage *= 2;

                    if (Stats.Resist.Contains(skill.Element))
                        damage /= 2;

                    Stats.HP -= damage;

                    break;

                case SkillId.Healing:
                    BigInteger health = skill.Value;
                    Stats.HP += health;
                    break;
            }
        }
    }
}
