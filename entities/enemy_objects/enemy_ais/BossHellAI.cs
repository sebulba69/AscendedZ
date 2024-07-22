using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// Generic AI for boss battles so I don't have to hard script every single fight.
    /// </summary>
    public class BossHellAI : Enemy
    {
        private int _phase, _mainElement, _move;

        private List<Func<EnemyAction, ISkill>> _script;

        public List<Elements> MainAttackElements { get; set; }
        public List<int> VoidElementsIndexes { get; set; }
        public List<int> WeaknessStatusIndexes { get; set; }
        public List<int> BuffIndexes { get; set; }
        public int HealIndex { get; set; }

        public BossHellAI() : base()
        {
            MainAttackElements = new List<Elements>();
            VoidElementsIndexes = new List<int>();
            WeaknessStatusIndexes = new List<int>();
            BuffIndexes = new List<int>();

            _phase = 0;
            _mainElement = 0;
            _move = 0;
            _isBoss = true;
            _script = new List<Func<EnemyAction, ISkill>>() { GetWeaknessSkill, GetAttackSkill, GetBuff, GetVoidSkill  };
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction() { Target = GetRandomAlivePlayer(battleSceneObject) };

            action.Skill = _script[_phase](action);

            _phase++;
            if(_phase >= _script.Count)
                _phase = 0;

            return action;
        }

        private ISkill GetAttackSkill(EnemyAction action)
        {
            Elements element = MainAttackElements[_mainElement];
            
            _mainElement++;
            if (_mainElement >= MainAttackElements.Count)
                _mainElement = 0;

            List<ISkill> matches = Skills.FindAll(skill => 
            {
                if(skill.Id == SkillId.Elemental)
                {
                    ElementSkill e = (ElementSkill)skill;
                    return e.Element == element;
                }
                else
                {
                    return false;
                }
            });

            return matches[_rng.Next(matches.Count)];
        }

        private ISkill GetVoidSkill(EnemyAction action)
        {
            if(_move < VoidElementsIndexes.Count)
            {
                action.Target = this;
                return Skills[VoidElementsIndexes[_move]];
            }
            else
            {
                return GetAttackSkill(action);
            }
        }

        private ISkill GetWeaknessSkill(EnemyAction action)
        {
            if(_move < WeaknessStatusIndexes.Count)
            {
                return Skills[WeaknessStatusIndexes[_move]];
            }
            else
            {
                return GetAttackSkill(action);
            }
        }

        private ISkill GetBuff(EnemyAction action)
        {
            if(_move < BuffIndexes.Count)
            {
                action.Target = this;
                return Skills[BuffIndexes[_move]];
            }
            else
            {
                return GetAttackSkill(action);
            }
        }

        public override void ResetEnemyState()
        {
            _move++;
            int max = Math.Max(WeaknessStatusIndexes.Count, VoidElementsIndexes.Count);
            if (_move >= max)
                _move = 0;
        }
    }
}
