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
            { EnemyNames.Conlen, Elements.Elec },
            { EnemyNames.Orachar, Elements.Ice },
            { PartyNames.Andmond, Elements.Dark },
            { PartyNames.Joan, Elements.Wind },
            { PartyNames.Tyhere, Elements.Fir },
            { PartyNames.Paria, Elements.Ice }
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
            if (name == PartyNames.Locphiedon)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
                member.Skills.Add(SkillDatabase.Wind1.Clone());
            }
            else if (name == PartyNames.Gagar)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Fir);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
                member.Skills.Add(SkillDatabase.Fire1.Clone());
            }
            else if (name == PartyNames.Yuudam)
            {
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
                member.Skills.Add(SkillDatabase.Heal1.Clone());
            }
            else if (name == PartyNames.Pecheal)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Fir);
                member.Skills.Add(SkillDatabase.Ice1.Clone());
            }
            else if (name == PartyNames.Toke)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
                member.Skills.Add(SkillDatabase.Dark1.Clone());
            }
            else if (name == PartyNames.Maxwald)
            {
                member.Resistances.SetResistance(ResistanceType.Rs, Elements.Light);
                member.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
                member.Skills.Add(SkillDatabase.Light1.Clone());
            }
            else if (name == PartyNames.Halvia)
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
