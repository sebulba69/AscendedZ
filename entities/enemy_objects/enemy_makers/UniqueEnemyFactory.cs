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
            _functionDictionary[EnemyNames.Everit_Pickerin] = MakeEveritPickerin;
            _functionDictionary[EnemyNames.Ocura] = MakeOcura;
            _functionDictionary[EnemyNames.Emush] = MakeEmush;
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

        public Enemy MakeEveritPickerin()
        {
            return new EveritPickerin();
        }

        public Enemy MakeEthelAura()
        {
            var ethel = MakeBossHellAI(EnemyNames.Ethel_Aura, 2);

            ethel.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Wind);

            ethel.Skills.Add(SkillDatabase.Elec1.Clone());
            ethel.Skills.Add(SkillDatabase.ElecAll.Clone());
            ethel.Skills.Add(SkillDatabase.Dark1.Clone());
            ethel.Skills.Add(SkillDatabase.VoidWind.Clone());

            return ethel;
        }

        public Enemy MakeDraceSkinner()
        {
            var draceSkinner = MakeBossHellAI(EnemyNames.Drace_Skinner, 3);

            draceSkinner.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Dark);

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

            judeStone.Skills.Add(SkillDatabase.IceAll.Clone());
            judeStone.Skills.Add(SkillDatabase.DarkAll.Clone());
            judeStone.Skills.Add(SkillDatabase.Dark1.Clone());
            judeStone.Skills.Add(SkillDatabase.VoidFire.Clone());
            judeStone.Skills.Add(SkillDatabase.VoidLight.Clone());
            judeStone.Skills.Add(SkillDatabase.WeakIce.Clone());
            judeStone.Skills.Add(SkillDatabase.Ice1.Clone());

            return judeStone;
        }

        public Enemy MakeOcura()
        {
            var ocura = MakeBossHellAI(EnemyNames.Ocura, 3, true);

            ocura.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Dark);
            ocura.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Light);

            ocura.Skills.Add(SkillDatabase.Elec1.Clone());
            ocura.Skills.Add(SkillDatabase.Fire1.Clone());
            ocura.Skills.Add(SkillDatabase.Ice1.Clone());
            ocura.Skills.Add(SkillDatabase.Wind1.Clone());
            ocura.Skills.Add(SkillDatabase.Dark1.Clone());
            ocura.Skills.Add(SkillDatabase.Light1.Clone());

            return ocura;
        }


        public Enemy MakeEmush()
        {
            var emush = MakeBossHellAI(EnemyNames.Emush, 4, true);

            emush.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Fire);
            emush.Resistances.SetResistance(ResistanceType.Dr, skills.Elements.Ice);
            emush.Resistances.SetResistance(ResistanceType.Dr, skills.Elements.Wind);
            emush.Skills.Add(SkillDatabase.Wind1.Clone());
            emush.Skills.Add(SkillDatabase.Ice1.Clone());
            emush.Skills.Add(SkillDatabase.Poison.Clone());
            emush.Skills.Add(SkillDatabase.IceAll.Clone());
            emush.Skills.Add(SkillDatabase.WindAll.Clone());
            
            return emush;
        }

        public Enemy MakeDraceRazor()
        {
            return new DraceRazor();
        }

        private BossHellAI MakeBossHellAI(string name, int turns, bool dCrawl = false)
        {
            var bhai = new BossHellAI()
            {
                Name = name,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                Turns = turns
            };

            if (dCrawl)
                bhai.MaxHP = EntityDatabase.GetBossHPDC(name);
            else
                bhai.MaxHP = EntityDatabase.GetBossHP(name);

            return bhai;
        }
    }
}
