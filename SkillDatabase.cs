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
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.ELEC_T1,
            Icon = ArtAssets.ELEC_ICON
        };

        public static ElementSkill FIRE_1 = new ElementSkill()
        {
            Name = "Singe",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Fir,
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.FIRE_T1,
            Icon = ArtAssets.FIRE_ICON
        };

        public static ElementSkill ICE_1 = new ElementSkill()
        {
            Name = "Shiver",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Ice,
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.ICE_T1,
            Icon = ArtAssets.ICE_ICON
        };

        public static ElementSkill LIGHT_1 = new ElementSkill()
        {
            Name = "Gleam",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Light,
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.LIGHT_T1,
            Icon = ArtAssets.LIGHT_ICON
        };

        public static ElementSkill WIND_1 = new ElementSkill()
        {
            Name = "Breeze",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Wind,
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.WIND_T1,
            Icon = ArtAssets.WIND_ICON
        };

        public static ElementSkill DARK_1 = new ElementSkill()
        {
            Name = "Shadow",
            Damage = 2,
            TargetType = TargetTypes.SINGLE_OPP,
            Element = Elements.Dark,
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.DARK_T1,
            Icon = ArtAssets.DARK_ICON
        };
        #endregion

        public static StatusSkill STUN_1 = new StatusSkill()
        {
            Name = "Stun",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.STUN_T1,
            Icon = ArtAssets.STUN_ICON,
            Status = new StunStatus()
        };

        public static StatusSkill DARK_BUFF_1 = new StatusSkill()
        {
            Name = "Dark Boost",
            TargetType = TargetTypes.SINGLE_TEAM,
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.DARK_T1,
            Icon = ArtAssets.DARK_ICON,
            Status = new ElementBuffStatus(Elements.Dark, 2)
        };

        public static HealSkill HEAL_1 = new HealSkill()
        {
            Name = "Regen",
            TargetType = TargetTypes.SINGLE_TEAM,
            StartupAnimation = ArtAssets.STARTUP1_MG,
            EndupAnimation = ArtAssets.HEAL_T1,
            Icon = ArtAssets.HEAL_ICON,
            HealAmount = 5
        };

        #region Temporary Battle Skills
        public static PassSkill PASS = new PassSkill()
        {
            Name = "Pass",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = string.Empty,
            EndupAnimation = string.Empty,
            Icon = ArtAssets.PASS_ICON
        };

        public static RetreatSkill RETREAT = new RetreatSkill()
        {
            Name = "Retreat",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = string.Empty,
            EndupAnimation = string.Empty,
            Icon = ArtAssets.RETREAT_ICON
        };
        #endregion
    }
}
