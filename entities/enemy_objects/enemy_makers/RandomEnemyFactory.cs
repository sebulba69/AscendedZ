using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses.weak_element;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class RandomEnemyFactory : EnemyFactory
    {
        private List<ElementSkill> _elementalSkills;
        private List<ISkill> _buffSkills;
        private List<ISkill> _voidSkills;
        private List<string> _names;
        private List<Elements> _elements;
        private List<ResistanceType> _resistances;

        public RandomEnemyFactory() : base()
        {
            _elementalSkills = new List<ElementSkill>() 
            {
                SkillDatabase.Fire1, SkillDatabase.Ice1, SkillDatabase.Wind1, SkillDatabase.Elec1, SkillDatabase.Dark1, SkillDatabase.Light1,
                SkillDatabase.FireAll, SkillDatabase.IceAll, SkillDatabase.WindAll, SkillDatabase.ElecAll, SkillDatabase.DarkAll, SkillDatabase.LightAll
            };

            _buffSkills = new List<ISkill>() 
            {
                SkillDatabase.DarkBuff1, SkillDatabase.LightBuff1, SkillDatabase.FireBuff1,
                SkillDatabase.IceBuff1, SkillDatabase.WindBuff1, SkillDatabase.ElecBuff1
            };

            _voidSkills = new List<ISkill>() 
            {
                SkillDatabase.VoidDark, SkillDatabase.VoidLight, SkillDatabase.VoidFire,
                SkillDatabase.VoidIce, SkillDatabase.VoidWind
            };

            _names = new List<string>() 
            {
                EnemyNames.Ansung, EnemyNames.Ardeb, EnemyNames.ChAffar, EnemyNames.Charcas, EnemyNames.DrigaBoli,
                EnemyNames.Ethel, EnemyNames.FoameShorti, EnemyNames.Keri, EnemyNames.Lyelof, EnemyNames.Nanles,
                EnemyNames.ReeshiDeeme, EnemyNames.Samjaris, EnemyNames.Tily
            };

            _elements = new List<Elements>() 
            {
                Elements.Fire, Elements.Ice, Elements.Wind, Elements.Elec, Elements.Dark, Elements.Light
            };

            _resistances = new List<ResistanceType>() { ResistanceType.Wk, ResistanceType.Rs, ResistanceType.Dr };
        }

        public Enemy GenerateEnemy(Random rng)
        {
            int aiMax = 10;
            string name = _names[rng.Next(_names.Count)];
            int hp = rng.Next(10, 20);

            int ai = rng.Next(aiMax);

            Enemy enemy;

            if (ai == 0)
            {
                var swapElement = GetRandomElement(rng);

                enemy = MakeResistanceChangerEnemy(name, hp, swapElement, SkillDatabase.ElementalOpposites[swapElement]);
            }
            else if (ai == 1)
            {
                enemy = MakeWeaknessHunterEnemy(name, hp);
            }
            else if (ai == 2) 
            {
                enemy = MakeSupportEnemy(name, hp);
            }
            else if (ai == 3)
            {
                enemy = MakeBeastEye(name, hp);
            }
            else if (ai == 4)
            {
                enemy = MakeCopyCatEnemy(name, hp);

            }
            else if (ai == 5)
            {
                enemy = MakeBuffEnemy(name, hp);
                enemy.Skills.Add(_buffSkills[rng.Next(_buffSkills.Count)].Clone());
            }
            else if(ai == 6)
            {
                var voidElement = GetRandomElement(rng);
                enemy = MakeProtectorEnemy(name, hp, voidElement);
                enemy.Skills.Add(_voidSkills[rng.Next(_voidSkills.Count)].Clone());
            }
            else if (ai == 7)
            {
                enemy = MakeAgroStatusEnemy(name, hp);
            }
            else
            {
                enemy = MakeAlternatingEnemy(name, hp);
            }

            PopulateEnemyResistanceRandom(rng, enemy);
            PopulateEnemySkillsRandom(rng, enemy);
            return enemy;
        }

        private void PopulateEnemyResistanceRandom(Random rng, Enemy enemy)
        {
            int res = rng.Next(2) + 1;
            for (int r = 0; r < res; r++)
            {
                Elements element = GetRandomElement(rng);
                ResistanceType type = GetRandomResistanceType(rng);
                enemy.Resistances.SetResistance(type, element);
            }
        }

        private void PopulateEnemySkillsRandom(Random rng, Enemy enemy)
        {
            int smax = rng.Next(4);
            for (int s = 0; s < smax + 1; s++)
            {
                enemy.Skills.Add(_elementalSkills[rng.Next(_elementalSkills.Count)].Clone());
            }
        }

        private ResistanceType GetRandomResistanceType(Random rng)
        {
            return _resistances[rng.Next(_resistances.Count)];
        }

        private Elements GetRandomElement(Random rng)
        {
            return _elements[rng.Next(_elements.Count)];
        }

        private Enemy MakeAlternatingEnemy(string name, int hp)
        {
            return new AlternatingEnemy
            {
                Name = $"[AE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }

        private Enemy MakeBuffEnemy(string name, int hp)
        {
            return new BuffEnemy
            {
                Name = $"[BUFF] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }

        private Enemy MakeBeastEye(string name, int hp)
        {
            return MakeEyeEnemy(name, hp, SkillDatabase.BeastEye);
        }

        private Enemy MakeEyeEnemy(string name, int hp, EyeSkill eyeSkill)
        {
            return new EyeEnemy
            {
                Name = $"[EYE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                EyeSkill = eyeSkill
            };
        }

        private Enemy MakeSupportEnemy(string name, int hp)
        {
            return new SupportEnemy
            {
                Name = $"[SPRT] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }

        private Enemy MakeCopyCatEnemy(string name, int hp)
        {
            return new CopyCatEnemy
            {
                Name = $"[CC] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }

        private Enemy MakeProtectorEnemy(string name, int hp, Elements elementToVoid)
        {
            var protector = new ProtectorEnemy()
            {
                Name = $"[PRCT] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                ElementToVoid = elementToVoid
            };

            return protector;
        }

        private Enemy MakeResistanceChangerEnemy(string name, int hp, Elements resist1, Elements resist2)
        {
            var resistChangerEnemy = new ResistanceChangerEnemy
            {
                Name = $"[RCE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                Resist1 = resist1,
                Resist2 = resist2
            };

            resistChangerEnemy.Resistances.SetResistance(ResistanceType.Rs, resist1);
            resistChangerEnemy.Resistances.SetResistance(ResistanceType.Wk, resist2);

            return resistChangerEnemy;
        }

        private Enemy MakeWeaknessHunterEnemy(string name, int hp)
        {
            return new WeaknessHunterEnemy
            {
                Name = $"[WEX] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }

        private Enemy MakeAgroStatusEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[AGRO] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new AgroStatus();
            statusAttackEnemy.Skills.Add(SkillDatabase.AgroEnemy.Clone());
            statusAttackEnemy.Description = $"[AGRO]: {statusAttackEnemy.Description}";

            return statusAttackEnemy;
        }

        private StatusAttackEnemy MakeStatusAttackEnemy(string name, int hp)
        {
            return new StatusAttackEnemy
            {
                Name = name,
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
