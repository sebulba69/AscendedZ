using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class Harbinger : Enemy
    {
        private BossScript _script;

        public Harbinger() : base()
        {
            _isBoss = true;

            Name = "Harbinger, Mangler of Legs";

            MaxHP = 50;
            Image = "res://enemy_pics/newpicture125.png";

            Resistances = new ResistanceArray();

            Resistances.CreateResistance(ResistanceType.Wk, Elements.Wind);

            Skills.Add(SkillDatabase.ELEC_1.Clone()); // 0
            Skills.Add(SkillDatabase.ICE_1.Clone()); // 1
            Skills.Add(SkillDatabase.STUN_1.Clone()); // 2
            Skills.Add(SkillDatabase.DARK_1.Clone()); // 3
            Skills.Add(SkillDatabase.DARK_BUFF_1.Clone()); // 4

            Turns = 2;

            _script = new HarbingerScript(this.Skills);
        }

        public override ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            return _script.GetNextMove(battleSceneObject);
        }

        /// <summary>
        /// Harbinger's target is decided when picking a skill
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            return _script.Target;
        }

        private bool HasWeaknessToCurrentElements(BattlePlayer p)
        {
            return (p.Resistances.IsWeakToElement(Elements.Elec) || p.Resistances.IsWeakToElement(Elements.Ice));
        }

        public override void ResetEnemyState()
        {
            _script.ResetEnemyMove();
        }
    }
}
