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
                BaseName = name,
                Damage = 2,
                TargetType = TargetTypes.SINGLE_OPP,
                Element = element,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                EndupAnimation = SkillAssets.GetAnimationByElementAndTier(1, element),
                Icon = SkillAssets.GetElementIconByElementEnum(element)
            };
        }

        public static StatusSkill ElecBuff1 { get => MakeBuffSkill("Elec+", 0.25, Elements.Elec); }
        public static StatusSkill FireBuff1 { get => MakeBuffSkill("Fire+", 0.25, Elements.Fir); }
        public static StatusSkill WindBuff1 { get => MakeBuffSkill("Wind+", 0.25, Elements.Wind); }
        public static StatusSkill IceBuff1 { get => MakeBuffSkill("Ice+", 0.25, Elements.Ice); }
        
        private static StatusSkill MakeBuffSkill(string name, double amount, Elements element)
        {
            string icon = SkillAssets.GetElementIconByElementEnum(element);

            var elementBuff = new ElementBuffStatus() 
            { 
                BuffElement = element,
                Amount = amount,
                Icon = icon
            };

            StatusSkill statusSkill = MakeStatusSkill(name, elementBuff);
            statusSkill.EndupAnimation = SkillAssets.GetAnimationByElementAndTier(1, element);
            statusSkill.Icon = icon;

            return statusSkill;
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
                return MakeVoidElementSkill("Void Fire", new VoidFireStatus());
            }
        }

        public static StatusSkill VoidIce
        {
            get
            {
                return MakeVoidElementSkill("Void Ice", new VoidIceStatus());
            }
        }

        public static StatusSkill VoidWind
        {
            get
            {
                return MakeVoidElementSkill("Void Wind", new VoidWindStatus());
            }
        }

        private static StatusSkill MakeVoidElementSkill(string name, Status status)
        {
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
                BaseName = name,
                TargetType = TargetTypes.SINGLE_OPP,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                Status = status
            };
        }

        public static HealSkill Heal1 = new HealSkill()
        {
            BaseName = "Regen",
            TargetType = TargetTypes.SINGLE_TEAM,
            StartupAnimation = SkillAssets.STARTUP1_MG,
            EndupAnimation = SkillAssets.HEAL_T1,
            Icon = SkillAssets.HEAL_ICON,
            HealAmount = 5
        };

        #region Temporary Battle Skills
        public static PassSkill PASS = new PassSkill()
        {
            BaseName = "Pass",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = string.Empty,
            EndupAnimation = string.Empty,
            Icon = SkillAssets.PASS_ICON
        };

        public static RetreatSkill RETREAT = new RetreatSkill()
        {
            BaseName = "Retreat",
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
            
            skills.AddRange(new ISkill[] { Fire1, Ice1, Wind1, Elec1, Light1, Dark1, Heal1, AgroPlayer });

            if (tier > 10)
                skills.AddRange(new ISkill[] { VoidFire, VoidIce, VoidWind });

            return skills;
        }
    }
}
