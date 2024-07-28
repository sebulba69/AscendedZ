using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AscendedZ.entities.battle_entities
{
    public enum EntityType
    {
        Player, Enemy
    }

    /// <summary>
    /// This class exists exclusively in the Battle Scene and will get disposed of
    /// when the battle ends.
    /// </summary>
    public class BattleEntity
    {
        private int _maxHP;
        private int _hp;
        private bool _isActiveEntity = false;
        public int Turns { get; set; }

        public EntityType Type { get; protected set; }

        public int HP
        {
            get => _hp;
            set
            {
                _hp = value;
                if (_hp < 0)
                    _hp = 0;

                if (_hp > MaxHP)
                    _hp = MaxHP;
            }
        }
        public int MaxHP
        {
            get => _maxHP;
            set
            {
                _maxHP = value;
                HP = _maxHP;
            }
        }
        public string Name { get; set; }
        public string BaseName { get; set; }
        public string Image { get; set; }
        public bool CanAttack { set; get; }
        public List<ISkill> Skills { get; set; } = new();
        public ResistanceArray Resistances { get; set; } = new();
        public BattleEntityStatuses StatusHandler { get; set; } = new();
        public bool IsActiveEntity { get => _isActiveEntity; set => _isActiveEntity = value; }

        public double[] ElementDamageModifiers { get; set; }

        public BattleEntity()
        {
            this.CanAttack = true;
            ElementDamageModifiers = new double[6];
            for(int i = 0; i < ElementDamageModifiers.Length; i++)
                ElementDamageModifiers[i] = 0;
        }

        public virtual BattleResult ApplyElementSkill(ElementSkill skill)
        {
            BattleResult result = new BattleResult()
            {
                HPChanged = skill.Damage,
                Target = this,
                SkillUsed = skill
            };

            if (this.Resistances.IsDrainElement(skill.Element))
            {
                this.HP += skill.Damage;

                result.ResultType = BattleResultType.Dr;
            }
            else if (this.Resistances.IsNullElement(skill.Element))
            {
                result.HPChanged = 0;
                result.ResultType = BattleResultType.Nu;
            }
            else if (this.Resistances.IsResistantToElement(skill.Element))
            {
                int damage = (int)(skill.Damage * 0.75);
                this.HP -= damage;

                result.HPChanged = damage;
                result.ResultType = BattleResultType.Rs;
            }
            else if (this.Resistances.IsWeakToElement(skill.Element))
            {
                int damage = skill.Damage + (int)(skill.Damage * 0.5);
                this.HP -= damage;

                result.HPChanged = damage;
                result.ResultType = BattleResultType.Wk;
            }
            else
            {
                this.HP -= skill.Damage;
                result.ResultType = BattleResultType.Normal;
            }

            if (skill.BaseName == SkillDatabase.DracoTherium.BaseName) 
            {
                StatusHandler.RemoveStatus(this, StatusId.PoisonStatus);
            }

            if(HP == 0)
            {
                StatusHandler.Clear();
            }

            return result;
        }

        public virtual BattleResult ApplyHealingSkill(HealSkill skill)
        {
            this.HP += skill.HealAmount;
            var result = new BattleResult()
            {
                HPChanged = skill.HealAmount,
                Target = this,
                SkillUsed = skill,
                ResultType = BattleResultType.HPGain
            };

            return result;
        }
    }
}
