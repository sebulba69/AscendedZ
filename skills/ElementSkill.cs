using AscendedZ.battle;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.skills
{
    public partial class ElementSkill : ISkill 
    {
        public SkillId Id => SkillId.Elemental;
        private int _damage;
        private int _damageModifier = 0;
        private int _tier = 1;
        private int _level = 0;
        private string _baseName;


        public string Name
        {
            get
            {
                if (_level == 0)
                    return _baseName;
                else
                    if (_level < int.MaxValue - 1)
                        return $"{_baseName} +{_level}";
                    else
                        return $"{_baseName} +MAX";
            }
        }
        public string BaseName { get => _baseName; set => _baseName = value; }
        public TargetTypes TargetType { get; set; }
        public Elements Element { get; set; }
        public int Level { get => _level; set => _level = value; }
        public int Tier { get => _tier; set => _tier = value; }
        public int Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                _damage = value;
            }
        }

        public string StartupAnimation { get; set; }
        public string EndupAnimation { get; set; }
        public string Icon { get; set; }

        public BattleResult ProcessSkill(BattleEntity user, BattleEntity target)
        {
            return target.ApplyElementSkill(user, this);
        }

        public BattleResult ProcessSkill(BattleEntity user, List<BattleEntity> targets)
        {
            BattleResult all = targets[0].ApplyElementSkill(user, this);

            all.Target = null;
            all.AllHPChanged.Add(all.HPChanged);
            all.Results.Add(all.ResultType);
            all.Targets.Add(targets[0]);

            // Wk, Rs, Nu, Dr, Norm, Guard, Tech, TechWk, Evade
            int[] bResultRanking = { 2, 1, 3, 4, 0, 1, 2, 2, 3 };

            int compare = bResultRanking[(int)all.ResultType];
            for (int i = 1; i < targets.Count; i++)
            {
                BattleResult result = ProcessSkill(user, targets[i]);

                all.Results.Add(result.ResultType);

                int integerResult = bResultRanking[(int)result.ResultType];

                if(compare < integerResult)
                {
                    compare = integerResult;
                    all.ResultType = result.ResultType;
                }
                    

                all.AllHPChanged.Add(result.HPChanged);
                all.Targets.Add(targets[i]);
            }

            return all;
        }


        public string GetBattleDisplayString()
        {
            return $"{this.Name} ({this.Damage})";
        }

        public override string ToString()
        {
            return $"[{this.Element.ToString()}] {this.Name} ({this.Damage})";
        }

        public void LevelUp()
        {
            int boost = GetBoostValue(Level);
            try
            {
                _level++;
            }
            catch (Exception)
            {
                _level = int.MaxValue - 1;
            }
            
            try
            {
                this.Damage += boost;
            }
            catch(Exception)
            {
                this.Damage = int.MaxValue - 1;
            }
        }

        public string GetUpgradeString()
        {
            return $"{ToString()} → {this.Damage + GetBoostValue(Level)}";
        }

        private int GetBoostValue(int level)
        {
            int boost = (level + 1 / 2) + 1;
            if (boost == 0)
                boost = 1;

            return boost;
        }

        public ISkill Clone()
        {
            return new ElementSkill()
            {
                BaseName = this.BaseName,
                Damage = this.Damage,
                TargetType = this.TargetType,
                Element = this.Element,
                StartupAnimation = this.StartupAnimation,
                EndupAnimation = this.EndupAnimation,
                Icon = this.Icon,
                Level = this.Level,
                Tier = this.Tier
            };
        }


    }
}
