using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.WebSocketPeer;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// Protects other enemy weaknesses except itself.
    /// If its shield was cast, it tries to hit a weakness.
    /// The void skill must always be at slot 0.
    /// All other skills are between slots 1 - N.
    /// </summary>
    public class ProtectorEnemy : WeaknessHunterEnemy
    {
        private const int VOID_SKILL = 0;
        private bool _useVoidSkill;
        public Elements ElementToVoid { get; set; }

        public ProtectorEnemy()
        {
            _useVoidSkill = true;
            Description = "Class: Protector Enemy\nDescription: Alternates between hitting weaknesses and covering an ally's weakness.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            // by default, run the weakness hunter ai
            EnemyAction action = base.GetNextAction(battleSceneObject);

            List<Enemy> enemies = battleSceneObject.AliveEnemies.FindAll(e => !e.StatusHandler.HasStatus(StatusId.VoidElementStatus) && !e.Name.Equals(e.Name));
            if(enemies.Count > 0 && _useVoidSkill)
            {
                Enemy target = enemies.Find(e => e.Resistances.IsWeakToElement(ElementToVoid));
                if(target == null)
                {
                    action.Target = enemies[_rng.Next(0, enemies.Count)];
                }
                else
                {
                    action.Target = target;
                }
                action.Skill = Skills[VOID_SKILL];
            }

            // use the void skill every other turn
            _useVoidSkill = !_useVoidSkill;
            return action;
        }
    }
}
