using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class AshenAsh : Enemy
    {
        private int _phase;
        private bool _randomAttack;

        // phase, skill
        private Dictionary<int, ElementSkill> elements;

        public AshenAsh() : base()
        {
            Name = EnemyNames.Ashen_Ash;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 3;
            _isBoss = true;
            _randomAttack = true;

            _phase = 0;
            Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);

            Skills.Add(SkillDatabase.Wind2.Clone());
            Skills.Add(SkillDatabase.Ice1.Clone());
            Skills.Add(SkillDatabase.Light2.Clone());
            Skills.Add(SkillDatabase.Fire1.Clone());
        }

        public override BattleResult ApplyElementSkill(ElementSkill skill)
        {
            var result = base.ApplyElementSkill(skill);

            IncrementPhase();

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction { Skill = Skills[_phase] };

            var agro = GetTargetAffectedByAgro(battleSceneObject);

            if (_isAgroOverride)
            {
                action.Target = agro;
            }
            else
            {
                List<BattlePlayer> partyMembers = battleSceneObject.AlivePlayers;
                partyMembers = OrderByHP();
                if (_phase % 2 == 0)
                {
                    // highest HP
                    action.Target = partyMembers[partyMembers.Count - 1];
                }
                else
                {
                    // lowest HP
                    action.Target = partyMembers[0];
                }

                List<BattlePlayer> OrderByHP()
                {
                    return partyMembers.OrderBy(p => (p.HP / p.MaxHP) * 100).ToList();
                }
            }

            IncrementPhase();
            return action;
        }

        private void IncrementPhase()
        {
            ElementSkill skill = (ElementSkill)Skills[_phase];

            int evenNumCheck = (_phase + 1) % 2;

            if(evenNumCheck == 0)
            {
                this.Resistances.SetResistance(ResistanceType.None, skill.Element);
            }
            else
            {
                this.Resistances.SetResistance(ResistanceType.None, SkillDatabase.ElementalOpposites[skill.Element]);
            }

            _phase++;
            if (_phase == Skills.Count)
                _phase = 0;

            skill = (ElementSkill)Skills[_phase];
            
            if ((evenNumCheck == 0))
            {
                this.Resistances.SetResistance(ResistanceType.Nu, skill.Element);
            }
            else
            {
                this.Resistances.SetResistance(ResistanceType.Wk, SkillDatabase.ElementalOpposites[skill.Element]);
            }
        }

        public override void ResetEnemyState() 
        {
        }
    }
}
