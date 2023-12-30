using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class StatusAttackEnemyFactory : EnemyFactory
    {
        public StatusAttackEnemyFactory()
        {
            _functionDictionary[EnemyNames.THYLAF] = MakeThylaf;
            _functionDictionary[EnemyNames.ARWIG] = MakeArwig;
            _functionDictionary[EnemyNames.RICCMAN] = MakeRiccman;
        }

        public Enemy MakeThylaf()
        {
            var thylaf = MakeStatusAttackEnemy(EnemyNames.THYLAF, 6);

            thylaf.Resistances.CreateResistance(ResistanceType.Wk, Elements.Wind);

            thylaf.Skills.Add(SkillDatabase.ELEC_1.Clone());

            return thylaf;
        }

        public Enemy MakeArwig()
        {
            var arwig = MakeStatusAttackEnemy(EnemyNames.ARWIG, 6);

            arwig.Resistances.CreateResistance(ResistanceType.Wk, Elements.Fir);

            arwig.Skills.Add(SkillDatabase.ICE_1.Clone());

            return arwig;
        }

        public Enemy MakeRiccman()
        {
            var riccman = MakeStatusAttackEnemy(EnemyNames.RICCMAN, 6);

            riccman.Resistances.CreateResistance(ResistanceType.Wk, Elements.Elec);

            riccman.Skills.Add(SkillDatabase.WIND_1.Clone());

            return riccman;
        }

        private Enemy MakeStatusAttackEnemy(string name, int hp)
        {
            return new StatusAttackEnemy 
            {
                Name = name,
                MaxHP = hp,
                Image = EnemyImageAssets.GetEnemyImage(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
