using AscendedZ.skills;
using AscendedZ.statuses;
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
        public static ElementSkill ELEC_1 = new ElementSkill()
        {
            Name = "Spark",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Elec,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.ELEC_T1,
            Icon = SkillAssets.ELEC_ICON
        };

        public static ElementSkill FIRE_1 = new ElementSkill()
        {
            Name = "Singe",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Fir,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.FIRE_T1,
            Icon = SkillAssets.FIRE_ICON
        };

        public static ElementSkill ICE_1 = new ElementSkill()
        {
            Name = "Shiver",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Ice,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.ICE_T1,
            Icon = SkillAssets.ICE_ICON
        };

        public static ElementSkill LIGHT_1 = new ElementSkill()
        {
            Name = "Gleam",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Light,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.LIGHT_T1,
            Icon = SkillAssets.LIGHT_ICON
        };

        public static ElementSkill WIND_1 = new ElementSkill()
        {
            Name = "Breeze",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Wind,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.WIND_T1,
            Icon = SkillAssets.WIND_ICON
        };

        public static ElementSkill DARK_1 = new ElementSkill()
        {
            Name = "Shadow",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Dark,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.DARK_T1,
            Icon = SkillAssets.DARK_ICON
        };
        #endregion

        public static StatusSkill STUN_S1 = new StatusSkill()
        {
            Name = "Stun",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.STUN_T1,
            Icon = SkillAssets.STUN_ICON,
            Status = new StunStatus(),
        };

        public static StatusSkill AGRO_ENEMY = new StatusSkill()
        {
            Name = "Agro",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.AGRO,
            Icon = SkillAssets.AGRO_ICON,
            Status = new AgroStatus()
        };

        public static StatusSkill AGRO_PLAYER = new StatusSkill()
        {
            Name = "Agro",
            TargetType = TargetTypes.SINGLE_TEAM,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.AGRO,
            Icon = SkillAssets.AGRO_ICON,
            Status = new AgroStatus()
        };

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
                    FIRE_1, ICE_1, WIND_1, ELEC_1, LIGHT_1, DARK_1, HEAL_1, AGRO_PLAYER
                };

            return skills;
        }
    }
}
