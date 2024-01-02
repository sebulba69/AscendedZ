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
        private static readonly Random _rng = new Random();

        /// <summary>
        /// KVPs of enemies and the elements they're strong to
        /// </summary>
        private static readonly Dictionary<string, Elements> _partyMemberElementPairs = new Dictionary<string, Elements> 
        {
            { EnemyNames.CONLEN, Elements.Elec },
            { EnemyNames.ORAHCAR, Elements.Ice },
            { PartyNames.ANDMOND, Elements.Dark },
            { PartyNames.JOAN, Elements.Wind },
            { PartyNames.TYHERE, Elements.Fir },
            { PartyNames.PARIA, Elements.Ice }
        };

        private static readonly Dictionary<Elements, Elements> _elementalOpposites = new Dictionary<Elements, Elements> 
        {
            { Elements.Fir, Elements.Ice },
            { Elements.Ice, Elements.Fir },
            { Elements.Wind, Elements.Elec },
            { Elements.Elec, Elements.Wind },
            { Elements.Light, Elements.Dark },
            { Elements.Dark, Elements.Light }
        };

        public static OverworldEntity MakePartyMember(string name)
        {
            OverworldEntity member = new OverworldEntity
            {
                Name = name,
                Image = CharacterImageAssets.GetImage(name),
                MaxHP = 10
            };
            MakePremadePartyMember(member);
            return member;
        }

        private static void MakePremadePartyMember(OverworldEntity member)
        {
            string name = member.Name;
            if (name == PartyNames.LOCPHIEDON)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
                member.Skills.Add(SkillDatabase.Wind1.Clone());
            }
            else if (name == PartyNames.GAGAR)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Fir);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
                member.Skills.Add(SkillDatabase.Fire1.Clone());
            }
            else if (name == PartyNames.YUUDAM)
            {
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
                member.Skills.Add(SkillDatabase.HEAL_1.Clone());
            }
            else if (name == PartyNames.PECHEAL)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Fir);
                member.Skills.Add(SkillDatabase.Ice1.Clone());
            }
            else if (name == PartyNames.TOKE)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
                member.Skills.Add(SkillDatabase.Dark1.Clone());
            }
            else if (name == PartyNames.MAXWALD)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Light);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
                member.Skills.Add(SkillDatabase.Light1.Clone());
            }
            else if (name == PartyNames.HALVIA)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Elec);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
                member.Skills.Add(SkillDatabase.Elec1.Clone());
            }
            else
            {
                throw new Exception($"Party member {name} not implemented.");
            }
        }
    }
}
