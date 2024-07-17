using AscendedZ.entities.enemy_objects.bosses;
using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    /// <summary>
    /// A factory for unique enemies with 1-off appearances.
    /// </summary>
    public class UniqueEnemyFactory : EnemyFactory
    {
        public UniqueEnemyFactory()
        {
            _functionDictionary[EnemyNames.Harbinger] = MakeHarbinger;
            _functionDictionary[EnemyNames.Elliot_Onyx] = MakeElliot;
            _functionDictionary[EnemyNames.Sable_Vonner] = MakeVonner;
            _functionDictionary[EnemyNames.Cloven_Umbra] = MakeUmbra;
            _functionDictionary[EnemyNames.Ashen_Ash] = MakeAshen;
            _functionDictionary[EnemyNames.Ethel_Aura] = MakeEthelAura;
            _functionDictionary[EnemyNames.Kellam_Von_Stein] = MakeKellamVonStein;
            _functionDictionary[EnemyNames.Drace_Skinner] = MakeDraceSkinner;
            _functionDictionary[EnemyNames.Jude_Stone] = MakeJudeStone;
            _functionDictionary[EnemyNames.Drace_Razor] = MakeDraceRazor;
        }

        public Enemy MakeHarbinger()
        {
            return new Harbinger();
        }

        public Enemy MakeElliot()
        {
            return new ElliotOnyx();
        }

        public Enemy MakeVonner()
        {
            return new SableVonner();
        }

        public Enemy MakeUmbra()
        {
            return new ClovenUmbra();
        }

        public Enemy MakeAshen()
        {
            return new AshenAsh();
        }

        public Enemy MakeKellamVonStein()
        {
            return new KellamVonStein();
        }

        public Enemy MakeEthelAura()
        {
            var ethel = MakeBossHellAI(EnemyNames.Ethel_Aura, 3);

            ethel.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Wind);

            ethel.MainAttackElements.Add(skills.Elements.Elec);
            ethel.MainAttackElements.Add(skills.Elements.Dark);

            ethel.Skills.Add(SkillDatabase.Elec1.Clone());
            ethel.Skills.Add(SkillDatabase.ElecAll.Clone());
            ethel.Skills.Add(SkillDatabase.Dark1.Clone());
            ethel.Skills.Add(SkillDatabase.VoidWind.Clone());

            ethel.VoidElementsIndexes.Add(3);

            return ethel;
        }

        public Enemy MakeDraceSkinner()
        {
            var draceSkinner = MakeBossHellAI(EnemyNames.Drace_Skinner, 4);

            draceSkinner.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Dark);

            draceSkinner.MainAttackElements.Add(skills.Elements.Light);
            draceSkinner.MainAttackElements.Add(skills.Elements.Elec);
            draceSkinner.MainAttackElements.Add(skills.Elements.Fire);
            draceSkinner.MainAttackElements.Add(skills.Elements.Wind);
            draceSkinner.MainAttackElements.Add(skills.Elements.Ice);

            draceSkinner.Skills.Add(SkillDatabase.Light1.Clone());
            draceSkinner.Skills.Add(SkillDatabase.Elec1.Clone());
            draceSkinner.Skills.Add(SkillDatabase.Fire1.Clone());
            draceSkinner.Skills.Add(SkillDatabase.WindAll.Clone());
            draceSkinner.Skills.Add(SkillDatabase.Ice1.Clone());

            return draceSkinner;
        }

        public Enemy MakeJudeStone()
        {
            var judeStone = MakeBossHellAI(EnemyNames.Jude_Stone, 3);

            judeStone.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Light);
            judeStone.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Fire);

            judeStone.MainAttackElements.Add(skills.Elements.Dark);
            judeStone.MainAttackElements.Add(skills.Elements.Ice);

            judeStone.Skills.Add(SkillDatabase.IceAll.Clone());
            judeStone.Skills.Add(SkillDatabase.DarkAll.Clone());
            judeStone.Skills.Add(SkillDatabase.Dark1.Clone());
            judeStone.Skills.Add(SkillDatabase.VoidFire.Clone());
            judeStone.Skills.Add(SkillDatabase.WeakIce.Clone());

            judeStone.VoidElementsIndexes.Add(3);
            judeStone.VoidElementsIndexes.Add(4);

            return judeStone;
        }
        public Enemy MakeDraceRazor()
        {
            return new DraceRazor();
        }

        private BossHellAI MakeBossHellAI(string name, int turns)
        {
            return new BossHellAI()
            {
                Name = name,
                Image = CharacterImageAssets.GetImagePath(name),
                MaxHP = EntityDatabase.GetBossHP(name),
                Resistances = new ResistanceArray(),
                Turns = turns
            };
        }
    }
}
