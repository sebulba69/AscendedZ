using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    public class ResistanceChangerEnemy : WeaknessHunterEnemy
    {
        private bool _alternateResistance;

        public Elements Resist1 { get; set; }
        public Elements Resist2 { get; set; }

        public ResistanceChangerEnemy() : base()
        {
            _alternateResistance = true;
            Description = $"[RCE]: Resistance Changer Enemy. Will alternate its elemental resistances between {Resist1} and {SkillDatabase.ElementalOpposites[Resist2]}.";
        }

        public override void ResetEnemyState()
        {
            Elements resist1;
            Elements resist2;

            if (_alternateResistance)
            {
                resist1 = Resist1;
                resist2 = Resist2;
            }
            else
            {
                resist1 = Resist2;
                resist2 = Resist1;
            }

            ResistanceType rtype1 = this.Resistances.GetResistance(resist1);
            ResistanceType rtype2 = this.Resistances.GetResistance(resist2);

            if(rtype1 < ResistanceType.Nu)
                this.Resistances.SetResistance(ResistanceType.Wk, resist1);

            if(rtype2 < ResistanceType.Nu)
                this.Resistances.SetResistance(ResistanceType.Rs, resist2);
            
            _alternateResistance = !_alternateResistance;
            base.ResetEnemyState();
        }
    }
}
