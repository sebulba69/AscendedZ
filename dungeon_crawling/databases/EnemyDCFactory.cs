using AscendedZ.dungeon_crawling.combat;
using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.dungeon_crawling.combat.battledc.gbskills;
using AscendedZ.entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.databases
{
    public class EnemyDCFactory
    {
        private int _tier;
        private Random _rng;

        public EnemyDCFactory(int tier, Random rng)
        {
            _tier = tier;
            _rng = rng;
        }

        public BEnemyDC MakeEnemy()
        {
            BEnemyDC enemy = new BEnemyDC(_tier);

            List<string> images = new List<string>()
            {
                EnemyNames.Anrol,
                EnemyNames.Conlen,
                EnemyNames.David,
                EnemyNames.Nanfrea,
                EnemyNames.CattuTDroni
            };

            List<Elements> elements = new List<Elements>() 
            {
                Elements.Ice,
                Elements.Elec,
                Elements.Wind,
                Elements.Fire,
                Elements.Wind
            };

            int index = _rng.Next(images.Count);

            enemy.Turns = 1;
            enemy.Image = CharacterImageAssets.GetImagePath(images[index]);
            enemy.Skills.Add(MakeElementSkill(elements[index]));

            return enemy;
        }

        private GBSkill MakeElementSkill(Elements element)
        {
            GBSkill skill = new GBSkill();

            skill.Name = $"{element} Burst";
            skill.Element = element;
            skill.Value = (2 * _tier) + 5;
            skill.Icon = SkillAssets.GetElementIconByElementEnum(element);
            skill.Type = GBSkillType.EnemyElement;
            skill.TargetType = GBTargetType.Opponent;

            return skill;
        }
    }
}
