using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class ProtectorEnemyFactory : EnemyFactory
    {
        public ProtectorEnemyFactory()
        {
            _functionDictionary[EnemyNames.Ed] = MakeEd;
            _functionDictionary[EnemyNames.Otem] = MakeOtem;
            _functionDictionary[EnemyNames.Hesret] = MakeHesret;
        }

        public Enemy MakeEd()
        {
            Elements elementToVoid = Elements.Fir;

            var ed = MakeProtectorEnemy(EnemyNames.Ed, 8, elementToVoid);

            ed.Skills.Add(SkillDatabase.VoidFire);
            ed.Skills.Add(SkillDatabase.Fire1);
            ed.Skills.Add(SkillDatabase.Light1);
            ed.Skills.Add(SkillDatabase.Elec1);

            return ed;
        }

        public Enemy MakeOtem()
        {
            Elements elementToVoid = Elements.Ice;

            var otem = MakeProtectorEnemy(EnemyNames.Otem, 8, elementToVoid);

            otem.Skills.Add(SkillDatabase.VoidIce);
            otem.Skills.Add(SkillDatabase.Ice1);
            otem.Skills.Add(SkillDatabase.Dark1);
            otem.Skills.Add(SkillDatabase.Wind1);

            return otem;
        }

        public Enemy MakeHesret()
        {
            Elements elementToVoid = Elements.Wind;

            var hesret = MakeProtectorEnemy(EnemyNames.Hesret, 8, elementToVoid);

            hesret.Skills.Add(SkillDatabase.VoidWind);
            hesret.Skills.Add(SkillDatabase.Wind1);
            hesret.Skills.Add(SkillDatabase.Ice1);
            hesret.Skills.Add(SkillDatabase.Fire1);

            return hesret;
        }

        private Enemy MakeProtectorEnemy(string name, int hp, Elements elementToVoid)
        {
            var protector = new ProtectorEnemy()
            {
                Name = $"[PRCT] {name}",
                MaxHP = hp,
                Image = CharacterImageAssets.GetImage(name),
                Resistances = new ResistanceArray(),
                ElementToVoid = elementToVoid
            };

            protector.Resistances.SetResistance(ResistanceType.Wk, elementToVoid);

            return protector;
        }
    }
}
