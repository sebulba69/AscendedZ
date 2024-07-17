using AscendedZ.skills;
using AscendedZ.statuses;
using AscendedZ.statuses.buff_elements;
using AscendedZ.statuses.void_elements;
using AscendedZ.statuses.weak_element;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ
{
    public class SkillDatabase
    {
        public static readonly Dictionary<Elements, Elements> ElementalOpposites = new Dictionary<Elements, Elements>
        {
            { Elements.Fire, Elements.Ice },
            { Elements.Ice, Elements.Fire },
            { Elements.Wind, Elements.Elec },
            { Elements.Elec, Elements.Wind },
            { Elements.Light, Elements.Dark },
            { Elements.Dark, Elements.Light }
        };

        #region Tiered Skills
        public static ElementSkill Elec1 { get => CreateSingleHitElementSkill("Spark", Elements.Elec); }
        public static ElementSkill Fire1 { get => CreateSingleHitElementSkill("Singe", Elements.Fire); }
        public static ElementSkill Ice1 { get => CreateSingleHitElementSkill("Shiver", Elements.Ice); }
        public static ElementSkill Light1 { get => CreateSingleHitElementSkill("Gleam", Elements.Light); }
        public static ElementSkill Wind1 { get => CreateSingleHitElementSkill("Breeze", Elements.Wind); }
        public static ElementSkill Dark1 { get => CreateSingleHitElementSkill("Shadow", Elements.Dark); }

        public static ElementSkill ElecAll { get => CreateMultiHitElementSkill("Zap", Elements.Elec); }
        public static ElementSkill FireAll { get => CreateMultiHitElementSkill("Flame", Elements.Fire); }
        public static ElementSkill IceAll { get => CreateMultiHitElementSkill("Ice", Elements.Ice); }
        public static ElementSkill LightAll { get => CreateMultiHitElementSkill("Beam", Elements.Light); }
        public static ElementSkill WindAll { get => CreateMultiHitElementSkill("Gust", Elements.Wind); }
        public static ElementSkill DarkAll { get => CreateMultiHitElementSkill("Darkness", Elements.Dark); }

        public static ElementSkill DracoTherium { get => CreateMultiHitElementSkill("Draco Therium", Elements.Dark); } // unique skill for Kellam

        private static readonly Dictionary<Elements, ElementSkill> SingleHitElementSkills = new Dictionary<Elements, ElementSkill>()
        {
            { Elements.Elec, Elec1 },
            { Elements.Fire, Fire1 },
            { Elements.Ice, Ice1 },
            { Elements.Light, Light1 },
            { Elements.Wind, Wind1 },
            { Elements.Dark, Dark1 }
        };

        private static readonly Dictionary<Elements, ElementSkill> AllHitElementSkills = new Dictionary<Elements, ElementSkill>() 
        {
            { Elements.Elec, ElecAll },
            { Elements.Fire, FireAll },
            { Elements.Ice, IceAll },
            { Elements.Light, LightAll },
            { Elements.Wind, WindAll },
            { Elements.Dark, DarkAll }
        };

        private static ElementSkill CreateSingleHitElementSkill(string name, Elements element)
        {
            return MakeNewSingleHitElement(name, element, 2, 1);
        }

        private static ElementSkill CreateMultiHitElementSkill(string name, Elements element)
        {
            var s = MakeNewSingleHitElement(name, element, 1, 2);
            s.TargetType = TargetTypes.OPP_ALL;
            return s;
        }

        public static ISkill GetSkillFromElement(Elements element)
        {
            var skills = new ElementSkill[]{ Fire1, Ice1, Elec1, Wind1, Dark1, Light1 };
            return skills[(int)element].Clone();
        }

        private static ElementSkill MakeNewSingleHitElement(string name, Elements element, int damage, int tier)
        {
            return new ElementSkill
            {
                BaseName = name,
                Damage = damage,
                TargetType = TargetTypes.SINGLE_OPP,
                Element = element,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                EndupAnimation = SkillAssets.GetAnimationByElementAndTier(tier, element),
                Icon = SkillAssets.GetElementIconByElementEnum(element),
                Tier = tier
            };
        }

        public static StatusSkill ElecBuff1 { get => MakeBuffSkill("Elec+", new BuffElecStatus { Amount = 0.25 }); }
        public static StatusSkill FireBuff1 { get => MakeBuffSkill("Fire+", new BuffFireStatus { Amount = 0.25 }); }
        public static StatusSkill WindBuff1 { get => MakeBuffSkill("Wind+", new BuffWindStatus { Amount = 0.25 }); }
        public static StatusSkill IceBuff1 { get => MakeBuffSkill("Ice+", new BuffIceStatus { Amount = 0.25 }); }
        
        private static StatusSkill MakeBuffSkill(string name, ElementBuffStatus status)
        {
            StatusSkill statusSkill = MakeStatusSkill(name, status);
            statusSkill.EndupAnimation = SkillAssets.GetAnimationByElementAndTier(1, status.BuffElement);
            statusSkill.Icon = SkillAssets.GetElementIconByElementEnum(status.BuffElement);

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
                return MakeChangeElementSkill("Void Fire", new VoidFireStatus());
            }
        }
        public static StatusSkill VoidIce
        {
            get
            {
                return MakeChangeElementSkill("Void Ice", new VoidIceStatus());
            }
        }
        public static StatusSkill VoidWind
        {
            get
            {
                return MakeChangeElementSkill("Void Wind", new VoidWindStatus());
            }
        }
        public static StatusSkill WeakFire
        {
            get
            {
                var s = MakeChangeElementSkill("Fire-", new WeakFireStatus());
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill WeakIce
        {
            get
            {
                var s = MakeChangeElementSkill("Ice-", new WeakIceStatus());
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill WeakElec
        {
            get
            {
                var s = MakeChangeElementSkill("Elec-", new WeakElecStatus());
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill RemoveWeakElec
        {
            get
            {
                var s = MakeChangeElementSkill("Remove Elec-", new WeakElecStatus());
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_TEAM;
                return s;
            }
        }

        public static StatusSkill RemoveWeakFire
        {
            get
            {
                var s = MakeChangeElementSkill("Remove Fire-", new WeakFireStatus());
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_TEAM;
                return s;
            }
        }

        public static StatusSkill RemoveVoidWind
        {
            get
            {
                var s = MakeChangeElementSkill("Remove Void Wind", new VoidWindStatus());
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill RemoveVoidFire
        {
            get
            {
                var s = MakeChangeElementSkill("Remove Void Fire", new VoidFireStatus());
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill RemoveVoidIce
        {
            get
            {
                var s = MakeChangeElementSkill("Remove Void Ice", new VoidIceStatus());
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill Poison
        {
            get
            {
                var poison = MakeStatusSkill("Poison", new PoisonStatus());
                poison.Icon = SkillAssets.POISON_ICON;
                poison.EndupAnimation = SkillAssets.AGRO;
                return poison;
            }
        }

        private static StatusSkill MakeChangeElementSkill(string name, Status status)
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

        public static HealSkill Heal1 { get => MakeHealSkill("Regen", 5, 1); } 
        public static HealSkill Heal2 { get => MakeHealSkill("Regen+", 10, 2);    }
        public static HealSkill Heal3 { get => MakeHealSkill("Regen X", 20, 3);   }
        public static HealSkill Heal4 { get => MakeHealSkill("Regen Z", 30, 4);   }
        public static HealSkill Heal5 { get => MakeHealSkill("Regen ASC", 40, 5); }

        public static HealSkill Revive1 { get => MakeHealSkill("Revive", 5, 1, true); }
        public static HealSkill Revive2 { get => MakeHealSkill("Revive+", 10, 2, true); }
        public static HealSkill Revive3 { get => MakeHealSkill("Revive X", 20, 3, true); }
        public static HealSkill Revive4 { get => MakeHealSkill("Revive Z", 30, 4, true); }
        public static HealSkill Revive5 { get => MakeHealSkill("Revive ASC", 40, 5, true); }

        public static HealSkill Heal1All { get => MakeAllHeal("Allgen", 2, 1); }
        public static HealSkill Heal2All { get => MakeAllHeal("Allgen+", 4, 1); }
        public static HealSkill Heal3All { get => MakeAllHeal("Allgen X", 8, 1); }
        public static HealSkill Heal4All { get => MakeAllHeal("Allgen Z", 12, 1); }
        public static HealSkill Heal5All { get => MakeAllHeal("Allgen ASC", 15, 1); }

        private static HealSkill MakeAllHeal(string name, int amount, int tier, bool isRevive = false)
        {
            var hs = MakeHealSkill(name, amount, tier, isRevive);
            hs.TargetType = TargetTypes.TEAM_ALL;
            return hs;
        }

        private static HealSkill MakeHealSkill(string name, int amount, int tier, bool isRevive = false)
        {
            return new HealSkill()
            {
                BaseName = name,
                TargetType = (!isRevive) ? TargetTypes.SINGLE_TEAM : TargetTypes.SINGLE_TEAM_DEAD,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                EndupAnimation = SkillAssets.HEAL_T1,
                Icon = SkillAssets.HEAL_ICON,
                HealAmount = amount,
                Tier = tier
            };
        }


        #region Temporary Battle Skills
        public static PassSkill Pass = new PassSkill()
        {
            BaseName = "Pass",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = string.Empty,
            EndupAnimation = string.Empty,
            Icon = SkillAssets.PASS_ICON
        };

        public static RetreatSkill Retreat = new RetreatSkill()
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
            
            skills.AddRange(new ISkill[] { Fire1, Ice1, Wind1, Elec1, Light1, Dark1, Heal1 });

            if (tier > TierRequirements.TIER2_STRONGER_ENEMIES)
                skills.AddRange(new ISkill[] { VoidFire, VoidIce, VoidWind });

            if (tier > TierRequirements.QUESTS_PARTY_MEMBERS_UPGRADE)
                skills.AddRange(new ISkill[] { Revive1, RemoveVoidWind, RemoveVoidIce });

            if (tier > TierRequirements.QUESTS_ALL_FUSION_MEMBERS)
                skills.AddRange(new ISkill[] { RemoveWeakElec, RemoveWeakFire });

            if (tier > TierRequirements.ALL_HIT_SKILLS)
                skills.AddRange(new ISkill[] { Heal1All });

            return skills;
        }
    }
}
