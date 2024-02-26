using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class ClovenUmbra : WeaknessHunterEnemy
    {
        private bool _voidUsed;

        public ClovenUmbra() : base()
        {
            Name = EnemyNames.Cloven_Umbra;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 3;
            _isBoss = true;
            _voidUsed = false;

            Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            Skills.Add(SkillDatabase.VoidIce.Clone());
            Skills.Add(SkillDatabase.Elec1.Clone());
            Skills.Add(SkillDatabase.Fire1.Clone());
            Skills.Add(SkillDatabase.Dark1.Clone());
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = base.GetNextAction(battleSceneObject);

            if (!_voidUsed)
            {
                action.Skill = Skills[0];
                action.Target = this;
                _voidUsed = true;
            }
            else if(_voidUsed && action.Skill.Name.Contains("Void"))
            {
                int skill = _rng.Next(1, 4);
                action.Skill = Skills[skill];
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _voidUsed = false;
            base.ResetEnemyState();
        }
    }
}
