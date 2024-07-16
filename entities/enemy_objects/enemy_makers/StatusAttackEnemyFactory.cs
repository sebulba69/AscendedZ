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
            _functionDictionary[EnemyNames.Thylaf] = MakeThylaf;
            _functionDictionary[EnemyNames.Arwig] = MakeArwig;
            _functionDictionary[EnemyNames.Riccman] = MakeRiccman;
            _functionDictionary[EnemyNames.Gormacwen] = MakeGormacwen;
            _functionDictionary[EnemyNames.Vidwerd] = MakeVidwerd;
        }

        public Enemy MakeThylaf()
        {
            var thylaf = MakeAgroStatusEnemy(EnemyNames.Thylaf, 6);

            thylaf.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            thylaf.Skills.Add(SkillDatabase.Elec1.Clone());

            return thylaf;
        }

        public Enemy MakeArwig()
        {
            var arwig = MakeAgroStatusEnemy(EnemyNames.Arwig, 6);

            arwig.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            arwig.Skills.Add(SkillDatabase.Ice1.Clone());

            return arwig;
        }

        public Enemy MakeRiccman()
        {
            var riccman = MakeAgroStatusEnemy(EnemyNames.Riccman, 6);

            riccman.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);

            riccman.Skills.Add(SkillDatabase.Wind1.Clone());

            return riccman;
        }

        public Enemy MakeGormacwen()
        {
            var gormacwen = MakeAgroStatusEnemy(EnemyNames.Gormacwen, 10);

            gormacwen.Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);

            gormacwen.Skills.Add(SkillDatabase.FireAll.Clone());

            return gormacwen;
        }

        public Enemy MakeVidwerd()
        {
            var vidwerd = MakeAgroStatusEnemy(EnemyNames.Vidwerd, 10);

            vidwerd.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);

            vidwerd.Skills.Add(SkillDatabase.DarkAll.Clone());

            return vidwerd;
        }

        private Enemy MakeAgroStatusEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[AGRO] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new AgroStatus();
            statusAttackEnemy.Skills.Add(SkillDatabase.AgroEnemy.Clone());
            statusAttackEnemy.Description = $"[AGRO]: {statusAttackEnemy.Description}";

            return statusAttackEnemy;
        }

        private StatusAttackEnemy MakeStatusAttackEnemy(string name, int hp)
        {
            return new StatusAttackEnemy 
            {
                Name = name,
                MaxHP = hp,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
