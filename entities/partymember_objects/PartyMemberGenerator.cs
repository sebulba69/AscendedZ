using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.partymember_objects
{

    public class PartyMemberGenerator
    {
        public static OverworldEntity MakePartyMember(string name, bool isPreMade = false)
        {
            OverworldEntity member = new OverworldEntity
            {
                Name = name,
                Image = PlayerPartyAssets.PartyMemberPics[name]
            };

            if (isPreMade)
            {
                MakePremadePartyMember(member);
            }
            else
            {
                member = new OverworldEntity();
                throw new NotImplementedException("Need a randomized party generator.");
            }

            return member;
        }

        private static void MakePremadePartyMember(OverworldEntity member)
        {
            member.VorpexValue = 1;
            member.MaxHP = 10;

            string name = member.Name;
            if (name == PartyNames.LOCPHIEDON)
            {
                member.Resistances.CreateResistance(ResistanceType.Rs, Elements.Wind);
                member.Resistances.CreateResistance(ResistanceType.Wk, Elements.Elec);
                member.Skills.Add(SkillDatabase.WIND_1.Clone());
            }
            else if (name == PartyNames.GAGAR)
            {
                member.Resistances.CreateResistance(ResistanceType.Rs, Elements.Fir);
                member.Resistances.CreateResistance(ResistanceType.Wk, Elements.Ice);
                member.Skills.Add(SkillDatabase.FIRE_1.Clone());
            }
            else if (name == PartyNames.YUUDAM)
            {
                member.Resistances.CreateResistance(ResistanceType.Wk, Elements.Dark);
                member.Skills.Add(SkillDatabase.HEAL_1.Clone());
            }
            else if (name == PartyNames.PECHEAL)
            {
                member.Resistances.CreateResistance(ResistanceType.Rs, Elements.Ice);
                member.Resistances.CreateResistance(ResistanceType.Wk, Elements.Fir);
                member.Skills.Add(SkillDatabase.ICE_1.Clone());
            }
            else if (name == PartyNames.TOKE)
            {
                member.Resistances.CreateResistance(ResistanceType.Rs, Elements.Dark);
                member.Resistances.CreateResistance(ResistanceType.Wk, Elements.Light);
                member.Skills.Add(SkillDatabase.DARK_1.Clone());
            }
            else if (name == PartyNames.MAXWALD)
            {
                member.Resistances.CreateResistance(ResistanceType.Rs, Elements.Light);
                member.Resistances.CreateResistance(ResistanceType.Wk, Elements.Dark);
                member.Skills.Add(SkillDatabase.LIGHT_1.Clone());
            }
            else if (name == PartyNames.HALVIA)
            {
                member.Resistances.CreateResistance(ResistanceType.Rs, Elements.Elec);
                member.Resistances.CreateResistance(ResistanceType.Wk, Elements.Wind);
                member.Skills.Add(SkillDatabase.ELEC_1.Clone());
            }
            else
            {
                throw new Exception($"Party member {name} not implemented.");
            }
        }
    }
}
