using AscendedZ.skills;
using AscendedZ.statuses;
using AscendedZ.statuses.void_elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class SkillDatabase
    {
        #region Tier 1 Skills
        public static ElementSkill Elec1 { get => CreateTier1ElementSkill("Spark", Elements.Elec); }
        public static ElementSkill Fire1 { get => CreateTier1ElementSkill("Singe", Elements.Fir); }
        public static ElementSkill Ice1 { get => CreateTier1ElementSkill("Shiver", Elements.Ice); }
        public static ElementSkill Light1 { get => CreateTier1ElementSkill("Gleam", Elements.Light); }
        public static ElementSkill Wind1 { get => CreateTier1ElementSkill("Breeze", Elements.Wind); }
        public static ElementSkill Dark1 { get => CreateTier1ElementSkill("Shadow", Elements.Dark); }

        private static ElementSkill CreateTier1ElementSkill(string name, Elements element)
        {
            return new ElementSkill
            {
                Name = name,
                Damage = 2,
                TargetType = TargetTypes.SINGLE_OPP,
                Element = element,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                EndupAnimation = SkillAssets.GetAnimationByElementAndTier(1, element),
                Icon = SkillAssets.GetElementIconByElementEnum(element)
            };
        }

        #endregion
        public static StatusSkill Stun
        {
            get
            {
                StatusSkill statusSkill = MakeStatusSkill("Stun", new StunStatus());

                statusSkill.Icon = SkillAssets.STUN_ICON;
                statusSkill.EndupAnimation = SkillAssets.STUN_T1;

                return statusSkill;
            }
        }

        
        public static StatusSkill AgroEnemy
        {
            get
            {
                StatusSkill statusSkill = MakeStatusSkill("Agro", new AgroStatus());

                statusSkill.EndupAnimation = SkillAssets.AGRO;
                statusSkill.Icon = SkillAssets.AGRO_ICON;

                return statusSkill;
            }
        }

        
        public static StatusSkill AgroPlayer 
        { 
            get 
            {
                StatusSkill statusSkill = AgroEnemy;
                statusSkill.TargetType = TargetTypes.SINGLE_TEAM;

                return statusSkill;
            } 
        }

        public static StatusSkill VoidFire
        {
            get
            {
                return MakeVoidElementSkill("Void Fire", Elements.Fir);
            }
        }

        private static StatusSkill MakeVoidElementSkill(string name, Elements element)
        {
            Status status = new VoidFireStatus();
            StatusSkill statusSkill = MakeStatusSkill(name, status);

            statusSkill.Icon = status.Icon;
            statusSkill.EndupAnimation = SkillAssets.VOID_SHIELD;
            statusSkill.TargetType = TargetTypes.SINGLE_TEAM;

            return statusSkill;
        }

        private static StatusSkill MakeStatusSkill(string name, Status status)
        {
            return new StatusSkill
            {
                Name = name,
                TargetType = TargetTypes.SINGLE_OPP,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                Status = status
            };
        }

        /*
        public static StatusSkill DARK_BUFF_1 = new StatusSkill()
        {
            Name = "Dark Boost",
            TargetType = TargetTypes.SINGLE_TEAM,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.DARK_T1,
            Icon = SkillAssets.DARK_ICON,
            Status = new ElementBuffStatus(Elements.Dark, 2)
        };
        */

        public static HealSkill HEAL_1 = new HealSkill()
        {
            Name = "Regen",
            TargetType = TargetTypes.SINGLE_TEAM,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.HEAL_T1,
            Icon = SkillAssets.HEAL_ICON,
            HealAmount = 5
        };

        #region Temporary Battle Skills
        public static PassSkill PASS = new PassSkill()
        {
            Name = "Pass",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = string.Empty,
            EndupAnimation = string.Empty,
            Icon = SkillAssets.PASS_ICON
        };

        public static RetreatSkill RETREAT = new RetreatSkill()
        {
            Name = "Retreat",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = string.Empty,
            EndupAnimation = string.Empty,
            Icon = SkillAssets.RETREAT_ICON
        };
        #endregion

        /// <summary>
        /// Get all skills that can be generated for a party member.
        /// </summary>
        /// <returns></returns>
        public static List<ISkill> GetAllGeneratableSkills(int tier)
        {
            List<ISkill> skills = new List<ISkill>();
            if (tier == 1)
                skills = new List<ISkill> 
                {
                    Fire1, Ice1, Wind1, Elec1, Light1, Dark1, HEAL_1, AgroPlayer
                };

            return skills;
        }
    }
}
