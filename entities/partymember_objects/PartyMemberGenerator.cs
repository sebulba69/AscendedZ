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

        public static OverworldEntity MakePartyMember(int tier)
        {
            var keys = _partyMemberElementPairs.Keys;
            string randomName = keys.ElementAt(_rng.Next(keys.Count));

            OverworldEntity member = new OverworldEntity
            {
                Name = randomName,
                Image = CharacterImageAssets.GetImage(randomName),
                MaxHP = _rng.Next(10, 17)
            };

            MakeRandomPartyMember(member, tier);
            return member;
        }

        private static void MakePremadePartyMember(OverworldEntity member)
        {
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

        private static void MakeRandomPartyMember(OverworldEntity member, int tier)
        {
            Elements resist = _partyMemberElementPairs[member.Name];
            Elements weak = _elementalOpposites[resist];

            member.Resistances.CreateResistance(ResistanceType.Rs, resist);
            member.Resistances.CreateResistance(ResistanceType.Wk, weak);

            List<ISkill> skills;

            if(tier <= 40)
                skills = GetAllSkillsWithoutWeakness(weak, SkillDatabase.GetAllGeneratableSkills(1));
            else
                throw new Exception("No generation programmed for tiers above 40 yet.");

            int numSkills = _rng.Next(1, 3);
            for (int generatedSkills = 0; generatedSkills < numSkills; generatedSkills++)
            {
                ISkill skill = skills[_rng.Next(skills.Count)];
                member.Skills.Add(skill.Clone());
            }
        }

        private static List<ISkill> GetAllSkillsWithoutWeakness(Elements weak, List<ISkill> skills)
        {
            List<ISkill> nonWeaknessSkills = skills.FindAll(skill =>
            {
                bool isWeakToElement = false;
                if (skill.Id == SkillId.Elemental)
                {
                    ElementSkill elementSkill = (ElementSkill)skill;
                    isWeakToElement = (elementSkill.Element == weak);
                }
                return !isWeakToElement;
            });

            return nonWeaknessSkills;
        }
    }
}
