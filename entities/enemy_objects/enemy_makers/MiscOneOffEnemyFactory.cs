using AscendedZ.entities.enemy_objects.bosses;
using AscendedZ.entities.enemy_objects.misc_one_offs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class MiscOneOffEnemyFactory : EnemyFactory
    {
        public MiscOneOffEnemyFactory()
        {
            _functionDictionary[EnemyNames.CATTUTDRONI] = MakeCattuTDroni;
            _functionDictionary[EnemyNames.HARBINGER] = MakeHarbinger;
        }

        public Enemy MakeCattuTDroni()
        {
            return new CattuTDroni();
        }

        public Enemy MakeHarbinger()
        {
            return new Harbinger();
        }
    }
}
