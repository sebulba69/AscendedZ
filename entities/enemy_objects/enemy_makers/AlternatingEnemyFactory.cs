using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class AlternatingEnemyFactory: EnemyFactory
    {
        public AlternatingEnemyFactory()
        {
            _functionDictionary[EnemyNames.Conlen] = MakeConlen;
            _functionDictionary[EnemyNames.Orachar] = MakeOrachar;
            _functionDictionary[EnemyNames.Fastrobren] = MakeFastrobren;
            _functionDictionary[EnemyNames.Liamlas] = MakeLiamlas;
            _functionDictionary[EnemyNames.Fledan] = MakeFledan;
            _functionDictionary[EnemyNames.Walds] = MakeWalds;
            _functionDictionary[EnemyNames.CattuTDroni] = MakeCattuTDroni;
            _functionDictionary[EnemyNames.Aldmas] = MakeAldmas;
            _functionDictionary[EnemyNames.Fridan] = MakeFridan;
            _functionDictionary[EnemyNames.Paca] = MakePaca;
            _functionDictionary[EnemyNames.Wigfred] = MakeWigfred;
            _functionDictionary[EnemyNames.Lyley] = MakeLyley;
            _functionDictionary[EnemyNames.Acardeb] = MakeAcardeb;
            _functionDictionary[EnemyNames.Darol] = MakeDarol;
            _functionDictionary[EnemyNames.Hesbet] = MakeHesbet;
            _functionDictionary[EnemyNames.Bue] = () => MakeBu(EnemyNames.Bue, 15, Elements.Ice);
            _functionDictionary[EnemyNames.Bued] = () => MakeBu(EnemyNames.Bued, 15, Elements.Fire);
            _functionDictionary[EnemyNames.Bureen] = () => MakeBu(EnemyNames.Bureen, 15, Elements.Wind);
        }

        public Enemy MakeConlen()
        {
            var conlen = MakeAlternatingEnemy(EnemyNames.Conlen, 6 + _tierBoost);

            conlen.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            conlen.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            conlen.Skills.Add(SkillDatabase.Elec1.Clone());
            conlen.Skills.Add(SkillDatabase.Fire1.Clone());

            return conlen;
        }

        public Enemy MakeCattuTDroni()
        {
            var droni = MakeAlternatingEnemy(EnemyNames.CattuTDroni, 8);

            droni.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            droni.Resistances.SetResistance(ResistanceType.Rs, Elements.Elec);

            droni.Skills.Add(SkillDatabase.Elec1.Clone());
            droni.Skills.Add(SkillDatabase.Ice1.Clone());

            return droni;
        }

        public Enemy MakeOrachar()
        {
            var orachar = MakeAlternatingEnemy(EnemyNames.Orachar, 6);

            orachar.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            orachar.Skills.Add(SkillDatabase.Ice1.Clone());

            return orachar;
        }

        public Enemy MakeFastrobren()
        {
            var fastrobren = MakeAlternatingEnemy(EnemyNames.Fastrobren, 4);

            fastrobren.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            fastrobren.Skills.Add(SkillDatabase.Dark1.Clone());

            return fastrobren;
        }

        public Enemy MakeLiamlas()
        {
            var liamlas = MakeAlternatingEnemy(EnemyNames.Liamlas, 6);
            
            liamlas.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);

            liamlas.Skills.Add(SkillDatabase.Light1.Clone());

            return liamlas;
        }

        public Enemy MakeFledan()
        {
            var fledan = MakeAlternatingEnemy(EnemyNames.Fledan, 10);

            fledan.Resistances.SetResistance(ResistanceType.Rs, Elements.Light);
            fledan.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);

            fledan.Skills.Add(SkillDatabase.Light1.Clone());
            fledan.Skills.Add(SkillDatabase.Ice1.Clone());

            return fledan;
        }

        public Enemy MakeWalds()
        {
            var walds = MakeAlternatingEnemy(EnemyNames.Walds, 10);

            walds.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);
            walds.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);

            walds.Skills.Add(SkillDatabase.Dark1.Clone());
            walds.Skills.Add(SkillDatabase.Wind1.Clone());

            return walds;
        }

        public Enemy MakeAldmas()
        {
            var aldmas = MakeAlternatingEnemy(EnemyNames.Aldmas, 7);

            aldmas.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            aldmas.Skills.Add(SkillDatabase.FireAll.Clone());

            return aldmas;
        }

        public Enemy MakeFridan()
        {
            var fridan = MakeAlternatingEnemy(EnemyNames.Fridan, 8);

            fridan.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);

            fridan.Skills.Add(SkillDatabase.WindAll.Clone());

            return fridan;
        }

        public Enemy MakePaca() 
        { 
            var paca = MakeSupportEnemy(EnemyNames.Paca, 9);

            paca.Skills.Add(SkillDatabase.Dark1.Clone());
            paca.Skills.Add(SkillDatabase.Light1.Clone());
            paca.Skills.Add(SkillDatabase.Wind1.Clone());

            return paca;
        }

        public Enemy MakeWigfred() 
        { 
            var wigfred = MakeSupportEnemy(EnemyNames.Wigfred, 9);

            wigfred.Skills.Add(SkillDatabase.Elec1.Clone());
            wigfred.Skills.Add(SkillDatabase.Ice1.Clone());
            wigfred.Skills.Add(SkillDatabase.ElecAll.Clone());

            return wigfred;
        }

        public Enemy MakeLyley() 
        { 
            var lyley = MakeSupportEnemy(EnemyNames.Lyley, 9);

            lyley.Skills.Add(SkillDatabase.Fire1.Clone());
            lyley.Skills.Add(SkillDatabase.Wind1.Clone());
            lyley.Skills.Add(SkillDatabase.FireAll.Clone());

            return lyley;
        }

        public Enemy MakeAcardeb()
        {
            var acardeb = MakeEyeEnemy(EnemyNames.Acardeb, 11, SkillDatabase.BeastEye);

            acardeb.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            acardeb.Skills.Add(SkillDatabase.Fire1.Clone());

            return acardeb;
        }

        public Enemy MakeDarol()
        {
            var darol = MakeEyeEnemy(EnemyNames.Darol, 11, SkillDatabase.BeastEye);

            darol.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            darol.Skills.Add(SkillDatabase.Dark1.Clone());

            return darol;
        }

        public Enemy MakeHesbet()
        {
            var darol = MakeEyeEnemy(EnemyNames.Hesbet, 11, SkillDatabase.BeastEye);

            darol.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            darol.Skills.Add(SkillDatabase.Elec1.Clone());

            return darol;
        }


        public Enemy MakeBu(string name, int hp, Elements element)
        {
            var bu = MakeAlternatingEnemy(name, hp);
            bu.Resistances.SetResistance(ResistanceType.Wk, SkillDatabase.ElementalOpposites[element]);
            bu.Resistances.SetResistance(ResistanceType.Rs, element);
            bu.Skills.Add(SkillDatabase.GetSkillFromElement(element));
            bu.Turns = 2;
            return bu;
        }

        protected Enemy MakeEyeEnemy(string name, int hp, EyeSkill eyeSkill)
        {
            return new EyeEnemy
            {
                Name = $"[EYE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                EyeSkill = eyeSkill
            };
        }

        protected Enemy MakeSupportEnemy(string name, int hp)
        {
            return new SupportEnemy
            {
                Name = $"[SPRT] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }

        protected Enemy MakeAlternatingEnemy(string name, int hp)
        {
            return new AlternatingEnemy
            {
                Name = $"[AE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
