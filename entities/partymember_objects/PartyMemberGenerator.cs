using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.partymember_objects
{

    public class PartyMemberGenerator
    {
        private static readonly Random _rng = new Random();

        private static readonly Dictionary<string, Func<OverworldEntity>> _preMadeEntities = new Dictionary<string, Func<OverworldEntity>>
        {
            { PartyNames.Locphiedon, MakeLocphiedon },
            { PartyNames.Gagar, MakeGagar },
            { PartyNames.Yuudam, MakeYuudam },
            { PartyNames.Pecheal, MakePecheal },
            { PartyNames.Toke, MakeToke },
            { PartyNames.Maxwald, MakeMaxwald },
            { PartyNames.Halvia, MakeHalvia },
        };

        /// <summary>
        /// KVPs of enemy party members and the elements they're strong to
        /// </summary>
        private static readonly Dictionary<string, Elements> _customPartyMembers = new Dictionary<string, Elements>
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
            OverworldEntity member;

            if(_preMadeEntities.ContainsKey(name))
            {
                member = _preMadeEntities[name]();
            }
            else if (_customPartyMembers.ContainsKey(name))
            {
                member = MakeOverworldEntity(name);

                Elements partyMemberElement = _customPartyMembers[name];
                member.Resistances.SetResistance(ResistanceType.Rs, partyMemberElement);
                member.Resistances.SetResistance(ResistanceType.Wk, _elementalOpposites[partyMemberElement]);
            }
            else
            {
                throw new Exception($"Party member {name} not implemented.");
            }

            return member;
        }

        #region Fusion 0 Party Members
        private static OverworldEntity MakeLocphiedon()
        {
            var member = MakeOverworldEntity(PartyNames.Locphiedon);

            member.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);
            member.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            member.Skills.Add(SkillDatabase.Wind1.Clone());

            return member;
        }

        private static OverworldEntity MakeGagar()
        {
            var member = MakeOverworldEntity(PartyNames.Gagar);

            member.Resistances.SetResistance(ResistanceType.Rs, Elements.Fir);
            member.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
            member.Skills.Add(SkillDatabase.Fire1.Clone());

            return member;
        }

        private static OverworldEntity MakeYuudam()
        {
            var member = MakeOverworldEntity(PartyNames.Yuudam);

            member.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            member.Skills.Add(SkillDatabase.Heal1.Clone());

            return member;
        }

        private static OverworldEntity MakePecheal()
        {
            var member = MakeOverworldEntity(PartyNames.Pecheal);

            member.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);
            member.Resistances.SetResistance(ResistanceType.Wk, Elements.Fir);
            member.Skills.Add(SkillDatabase.Ice1.Clone());

            return member;
        }

        private static OverworldEntity MakeToke()
        {
            var member = MakeOverworldEntity(PartyNames.Toke);

            member.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);
            member.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            member.Skills.Add(SkillDatabase.Dark1.Clone());

            return member;
        }

        private static OverworldEntity MakeMaxwald()
        {
            var member = MakeOverworldEntity(PartyNames.Maxwald);

            member.Resistances.SetResistance(ResistanceType.Rs, Elements.Light);
            member.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            member.Skills.Add(SkillDatabase.Light1.Clone());

            return member;
        }

        private static OverworldEntity MakeHalvia()
        {
            var member = MakeOverworldEntity(PartyNames.Halvia);
            member.Resistances.SetResistance(ResistanceType.Rs, Elements.Elec);
            member.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            member.Skills.Add(SkillDatabase.Elec1.Clone());
            return member;
        }
        #endregion

        private static OverworldEntity MakeFusion1Entity(string name)
        {
            var overworldEntity = MakeOverworldEntity(name);
            overworldEntity.FusionGrade = 1;
            return overworldEntity;
        }

        private static OverworldEntity MakeOverworldEntity(string name)
        {
            return new OverworldEntity
            {
                Name = name,
                Image = CharacterImageAssets.GetImage(name),
                MaxHP = 10
            };
        }
    }
}
