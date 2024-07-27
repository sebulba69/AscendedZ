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
using System.Xml.Linq;
using Godot;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class RandomEnemyFactory : EnemyFactory
    {
        private Random _rng;
        private List<ElementSkill> _elementalSkills;
        private List<StatusSkill> _miscStatuses;
        private List<ISkill> _voidSkills;
        private List<string> _names;
        private List<Elements> _elements;
        private List<ResistanceType> _resistances;
        private List<string> _bossNames;

        public RandomEnemyFactory() : base()
        {
            _elementalSkills = new List<ElementSkill>() 
            {
                SkillDatabase.Fire1, SkillDatabase.Ice1, SkillDatabase.Wind1, SkillDatabase.Elec1, SkillDatabase.Dark1, SkillDatabase.Light1,
                SkillDatabase.FireAll, SkillDatabase.IceAll, SkillDatabase.WindAll, SkillDatabase.ElecAll, SkillDatabase.DarkAll, SkillDatabase.LightAll
            };

            _miscStatuses = new List<StatusSkill>() 
            {
                SkillDatabase.Poison, SkillDatabase.Stun
            };

            _voidSkills = new List<ISkill>() 
            {
                SkillDatabase.VoidDark, SkillDatabase.VoidLight, SkillDatabase.VoidFire,
                SkillDatabase.VoidIce, SkillDatabase.VoidWind, SkillDatabase.VoidElec
            };

            _names = new List<string>() 
            {
                EnemyNames.Ansung, EnemyNames.Ardeb, EnemyNames.ChAffar, EnemyNames.Charcas, EnemyNames.DrigaBoli,
                EnemyNames.Ethel, EnemyNames.FoameShorti, EnemyNames.Keri, EnemyNames.Lyelof, EnemyNames.Nanles,
                EnemyNames.ReeshiDeeme, EnemyNames.Samjaris, EnemyNames.Tily, EnemyNames.Hahere
            };

            _bossNames = new List<string>() 
            {
                EnemyNames.Algrools, EnemyNames.Gos, EnemyNames.Laltujass, EnemyNames.Pool, EnemyNames.Pool, EnemyNames.Qibrel,
                EnemyNames.Sirgopes, EnemyNames.Suamgu, EnemyNames.Vrasrohd, EnemyNames.Vrosh
            };

            _elements = new List<Elements>() 
            {
                Elements.Fire, Elements.Ice, Elements.Wind, Elements.Elec, Elements.Dark, Elements.Light
            };

            _resistances = new List<ResistanceType>() { ResistanceType.Wk, ResistanceType.Rs, ResistanceType.Dr };

            _rng = new Random();

            foreach(string name in _names)
            {
                _functionDictionary.Add(name, GenerateEnemy);
            }
        }

        public Enemy GenerateEnemy()
        {
            int aiMax = 10;
            string name = _names[_rng.Next(_names.Count)];
            int hp = _rng.Next(10, 20);

            int ai = _rng.Next(aiMax);
            Enemy enemy;

            var skills = new List<ISkill>();

            if (ai == 0)
            {
                var swapElement = GetRandomElement(_rng);
                var opposite = SkillDatabase.ElementalOpposites[swapElement];

                enemy = MakeResistanceChangerEnemy(name, hp, swapElement, opposite);

                skills.AddRange(_elementalSkills.FindAll(e => e.Element == swapElement));
                skills.AddRange(_elementalSkills.FindAll(e => e.Element == opposite));
            }
            else if (ai == 1)
            {
                enemy = MakeWeaknessHunterEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 2) 
            {
                enemy = MakeSupportEnemy(name, hp);
               
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 3)
            {
                var wexElement = GetRandomElement(_rng);
                enemy = MakeBeastEye(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);

                enemy.Resistances.SetResistance(ResistanceType.Wk, wexElement);

                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 4)
            {
                enemy = MakeCopyCatEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 5)
            {
                var buffElement = GetRandomElement(_rng);
                enemy = MakeBuffEnemy(name, hp);

                skills.Add(_miscStatuses.Find(b => b.BaseName.Contains(buffElement.ToString())));
                skills.AddRange(_elementalSkills.FindAll(e => e.Element == buffElement));
                
                PopulateEnemyResistanceRandom(_rng, enemy);
                enemy.Resistances.SetResistance(ResistanceType.Rs, buffElement);
            }
            else if(ai == 6)
            {
                var voidElement = GetRandomElement(_rng);
                enemy = MakeProtectorEnemy(name, hp, voidElement);
                var voidSkill = _voidSkills.Find(v => v.BaseName.Contains(voidElement.ToString())).Clone();
                enemy.Skills.Add(voidSkill);
                PopulateEnemyResistanceRandom(_rng, enemy);
                enemy.Resistances.SetResistance(ResistanceType.Wk, voidElement);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 7)
            {
                enemy = MakeAgroStatusEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else
            {
                enemy = MakeAlternatingEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }

            if (skills.Count > 0)
                foreach (var skill in skills)
                    enemy.Skills.Add(skill.Clone());

            enemy.MaxHP += 2;

            return enemy;
        }

        public Enemy GenerateBoss(int tier)
        {
            string name = _bossNames[_rng.Next(_bossNames.Count)];
            int turns = _rng.Next(2, 5);

            var bhai = MakeBossHellAI(name, turns);
            bhai.MaxHP = EntityDatabase.GetBossHPRandom(tier);

            int res = _rng.Next(2) + 1;
            List<Elements> wex = new List<Elements>();
            for (int r = 0; r < res; r++)
            {
                Elements element = GetRandomElement(_rng);
                ResistanceType type = GetRandomResistanceType(_rng);
                bhai.Resistances.SetResistance(type, element);

                if(type == ResistanceType.Wk)
                    wex.Add(element);
            }

            if(wex.Count > 0)
            {
                int addBeastEye = _rng.Next(1, 101);
                if (addBeastEye <= 35)
                    bhai.Skills.Add(SkillDatabase.BeastEye);

                foreach (var voidElement in wex)
                {
                    int voidWex = _rng.Next(1, 101);
                    if(voidWex <= 45)
                        bhai.Skills.Add(_voidSkills.Find(v => v.BaseName.Contains(voidElement.ToString())).Clone());
                }
            }

            PopulateEnemySkillsRandom(_rng, bhai);
            return bhai;
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
            int smax = rng.Next(2, 4);

            for (int s = 0; s < smax + 1; s++)
            {
                enemy.Skills.Add(_elementalSkills[rng.Next(_elementalSkills.Count)].Clone());
            }

            if(rng.Next(100) < 25)
                enemy.Skills.Add(_miscStatuses[rng.Next(_miscStatuses.Count)]);
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
            var support = new SupportEnemy
            {
                Name = $"[SPRT] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };

            support.LevelUpCompensation(((_tierBoost / 3) * 10) / 2);

            return support;
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

        private BossHellAI MakeBossHellAI(string name, int turns)
        {
            var bhai = new BossHellAI()
            {
                Name = name,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                Turns = turns
            };

            return bhai;
        }
    }
}
