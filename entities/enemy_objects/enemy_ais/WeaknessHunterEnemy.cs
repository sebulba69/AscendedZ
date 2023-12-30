using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// An enemy who targets the first instance of a party member with a weakness to a skill it has.
    /// </summary>
    public class WeaknessHunterEnemy : AlternatingEnemy
    {
        public WeaknessHunterEnemy() : base()
        {
        }

        /// <summary>
        /// WeaknessHunters always pick targets that are weak to the skill they choose
        /// unless the agro status is in place.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public override BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            BattleEntity target = base.GetNextTarget(battleSceneObject);

            // if the agro status is not in effect,
            if(!_isAgroOverride)
            {
                List<BattlePlayer> partyMembers = battleSceneObject.AlivePlayers;
                if(_selectedSkill.Id == SkillId.Elemental)
                {
                    ElementSkill elementSkill = (ElementSkill)_selectedSkill;
                    
                    foreach(BattlePlayer player in partyMembers)
                    {
                        if (player.Resistances.IsWeakToElement(elementSkill.Element))
                        {
                            target = player;
                            break;
                        }
                    }
                }
            }

            return target;  
        }

        public override void ResetEnemyState()
        {
            CurrentMove = 0;
        }
    }
}
