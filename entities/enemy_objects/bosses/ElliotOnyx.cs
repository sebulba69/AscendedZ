using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class ElliotOnyx : Enemy
    {
        private const int SHIELD = 0; // state + index
        private const int ATTACK = 1;

        private const int ICE = 1;
        private const int WIND = 2;

        private int _state;
        private int _nextTarget;

        public ElliotOnyx() : base()
        {
            _isBoss = true;

            Name = EnemyNames.ELLIOT_ONYX;
            Image = CharacterImageAssets.GetImage(Name);
            MaxHP = 80;
            Turns = 3;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Fir);

            Skills.Add(SkillDatabase.VoidFire.Clone());
            Skills.Add(SkillDatabase.Ice1.Clone());
            Skills.Add(SkillDatabase.Wind1.Clone());

            _state = ATTACK;
            _nextTarget = 0;
        }

        public override BattleResult ApplyElementSkill(ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(skill);

            if (result.ResultType == BattleResultType.Wk)
                _state = SHIELD;

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            if(_state == SHIELD)
            {
                action.Skill = Skills[SHIELD];
                action.Target = this;

                _state = ATTACK;
            }
            else
            {
                List<BattlePlayer> players = battleSceneObject.AlivePlayers;

                if (_nextTarget == players.Count)
                    _nextTarget = players.Count - 1;

                action.Target = players[_nextTarget];

                if (action.Target.Resistances.IsWeakToElement(Elements.Ice))
                    action.Skill = Skills[ICE];
                else if (action.Target.Resistances.IsWeakToElement(Elements.Wind))
                    action.Skill = Skills[WIND];
                else
                    action.Skill = Skills[_rng.Next(ICE, WIND + 1)];

                _nextTarget++;
                if (_nextTarget == players.Count)
                    _nextTarget = 0;
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _state = ATTACK;
        }
    }
}
