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
            Description = $"[RCE]: Resistance Changer Enemy. Will alternate its elemental resistances between {Resist1} and {Resist2}.";
        }

        public override void ResetEnemyState()
        {
            if (_alternateResistance)
            {
                this.Resistances.SetResistance(ResistanceType.Wk, Resist1);
                this.Resistances.SetResistance(ResistanceType.Rs, Resist2);
            }
            else
            {
                this.Resistances.SetResistance(ResistanceType.Wk, Resist2);
                this.Resistances.SetResistance(ResistanceType.Rs, Resist1);
            }

            _alternateResistance = !_alternateResistance;
            base.ResetEnemyState();
        }
    }
}
