﻿using AscendedZ.skills;
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
        public static ElementSkill Elec1 { get => CreateTier1ElementSkill("Spark", Elements.Elec); }
        public static ElementSkill Fire1 { get => CreateTier1ElementSkill("Singe", Elements.Fire); }
        public static ElementSkill Ice1 { get => CreateTier1ElementSkill("Shiver", Elements.Ice); }
        public static ElementSkill Light1 { get => CreateTier1ElementSkill("Gleam", Elements.Light); }
        public static ElementSkill Wind1 { get => CreateTier1ElementSkill("Breeze", Elements.Wind); }
        public static ElementSkill Dark1 { get => CreateTier1ElementSkill("Shadow", Elements.Dark); }

        public static ElementSkill Elec2 { get => CreateTier2ElementSkill("Zap", Elements.Elec); }
        public static ElementSkill Fire2 { get => CreateTier2ElementSkill("Flame", Elements.Fire); }
        public static ElementSkill Ice2 { get => CreateTier2ElementSkill("Ice", Elements.Ice); }
        public static ElementSkill Light2 { get => CreateTier2ElementSkill("Beam", Elements.Light); }
        public static ElementSkill Wind2 { get => CreateTier2ElementSkill("Gust", Elements.Wind); }
        public static ElementSkill Dark2 { get => CreateTier1ElementSkill("Darkness", Elements.Dark); }

        public static ElementSkill Elec3 { get => CreateTier3ElementSkill("Lightning", Elements.Elec); }
        public static ElementSkill Fire3 { get => CreateTier3ElementSkill("Inferno", Elements.Fire); }
        public static ElementSkill Ice3 { get => CreateTier3ElementSkill("Frost Cannon", Elements.Ice); }
        public static ElementSkill Light3 { get => CreateTier3ElementSkill("Holy Light", Elements.Light); }
        public static ElementSkill Wind3 { get => CreateTier3ElementSkill("Storm", Elements.Wind); }
        public static ElementSkill Dark3 { get => CreateTier3ElementSkill("Abyss", Elements.Dark); }

        public static ElementSkill Elec4 { get => CreateTier4ElementSkill("Thor's Hammer", Elements.Elec); }
        public static ElementSkill Fire4 { get => CreateTier4ElementSkill("Hellfire", Elements.Fire); }
        public static ElementSkill Ice4 { get => CreateTier4ElementSkill("Cold Wave", Elements.Ice); }
        public static ElementSkill Light4 { get => CreateTier4ElementSkill("Heavenly Gaze", Elements.Light); }
        public static ElementSkill Wind4 { get => CreateTier4ElementSkill("Tornado of Souls", Elements.Wind); }
        public static ElementSkill Dark4 { get => CreateTier4ElementSkill("Infinite Despair", Elements.Dark); }

        public static ElementSkill Elec5 { get => CreateTier5ElementSkill("Smite", Elements.Elec); }
        public static ElementSkill Fire5 { get => CreateTier5ElementSkill("Incinerate", Elements.Fire); }
        public static ElementSkill Ice5 { get => CreateTier5ElementSkill("Ice Age", Elements.Ice); }
        public static ElementSkill Light5 { get => CreateTier5ElementSkill("God's Wrath", Elements.Light); }
        public static ElementSkill Wind5 { get => CreateTier5ElementSkill("Winds of Time", Elements.Wind); }
        public static ElementSkill Dark5 { get => CreateTier5ElementSkill("Unending Depths", Elements.Dark); }


        private static readonly Dictionary<Elements, ElementSkill> Tier1ElementSkills = new Dictionary<Elements, ElementSkill>()
        {
            { Elements.Elec, Elec1 },
            { Elements.Fire, Fire1 },
            { Elements.Ice, Ice1 },
            { Elements.Light, Light1 },
            { Elements.Wind, Wind1 },
            { Elements.Dark, Dark1 }
        };

        private static readonly Dictionary<Elements, ElementSkill> Tier2ElementSkills = new Dictionary<Elements, ElementSkill>() 
        {
            { Elements.Elec, Elec2 },
            { Elements.Fire, Fire2 },
            { Elements.Ice, Ice2 },
            { Elements.Light, Light2 },
            { Elements.Wind, Wind2 },
            { Elements.Dark, Dark2 }
        };

        private static readonly Dictionary<Elements, ElementSkill> Tier3ElementSkills = new Dictionary<Elements, ElementSkill>()
        {
            { Elements.Elec, Elec3 },
            { Elements.Fire, Fire3 },
            { Elements.Ice, Ice3 },
            { Elements.Light, Light3 },
            { Elements.Wind, Wind3 },
            { Elements.Dark, Dark3 }
        };

        private static readonly Dictionary<Elements, ElementSkill> Tier4ElementSkills = new Dictionary<Elements, ElementSkill>()
        {
            { Elements.Elec, Elec4 },
            { Elements.Fire, Fire4 },
            { Elements.Ice, Ice4 },
            { Elements.Light, Light4 },
            { Elements.Wind, Wind4 },
            { Elements.Dark, Dark4 }
        };

        private static readonly Dictionary<Elements, ElementSkill> Tier5ElementSkills = new Dictionary<Elements, ElementSkill>()
        {
            { Elements.Elec, Elec5 },
            { Elements.Fire, Fire5 },
            { Elements.Ice, Ice5 },
            { Elements.Light, Light5 },
            { Elements.Wind, Wind5 },
            { Elements.Dark, Dark5 }
        };

        /// <summary>
        /// Get Skill assuming Tier is base 1 -> 5
        /// </summary>
        /// <param name="tier"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ElementSkill GetSkillByElement(int tier, Elements element)
        {
            tier--;

            ElementSkill[] eSkills =
            {
                Tier1ElementSkills[element],
                Tier2ElementSkills[element],
                Tier3ElementSkills[element],
                Tier4ElementSkills[element],
                Tier5ElementSkills[element]
            };

            return eSkills[tier];
        }

        public static ElementSkill GetNextTierOfElementSkill(int skillTier, ElementSkill skill)
        {
            ElementSkill[] eSkills = 
            { 
                skill, 
                Tier2ElementSkills[skill.Element],
                Tier3ElementSkills[skill.Element],
                Tier4ElementSkills[skill.Element],
                Tier5ElementSkills[skill.Element]
            };

            try
            {
                return eSkills[skillTier];
            }
            catch (Exception) 
            { 
                return skill; 
            }
        }

        public static HealSkill GetNextTierOfHealSkill(int skillTier, TargetTypes targetType)
        {
            if(targetType == TargetTypes.SINGLE_TEAM_DEAD)
            {
                return new HealSkill[] { Revive1, Revive2, Revive3, Revive4, Revive5 }[skillTier];
            }
            else if(targetType == TargetTypes.TEAM_ALL)
            {
                return new HealSkill[] { Heal1All, Heal2All, Heal3All, Heal4All, Heal5All }[skillTier];
            }
            else
            {
                return new HealSkill[] { Heal1, Heal2, Heal3, Heal4, Heal5 }[skillTier];
            }

        }

        private static ElementSkill CreateTier1ElementSkill(string name, Elements element)
        {
            return MakeNewSingleHitElement(name, element, 2, 1);
        }

        private static ElementSkill CreateTier2ElementSkill(string name, Elements element)
        {
            return MakeNewSingleHitElement(name, element, 5, 2);
        }

        private static ElementSkill CreateTier3ElementSkill(string name, Elements element)
        {
            return MakeNewSingleHitElement(name, element, 8, 3);
        }

        private static ElementSkill CreateTier4ElementSkill(string name, Elements element)
        {
            return MakeNewSingleHitElement(name, element, 12, 4);
        }

        private static ElementSkill CreateTier5ElementSkill(string name, Elements element)
        {
            // _ascendedLevel * 2 gets added to this each time so it's 8 (but really 18)
            return MakeNewSingleHitElement(name, element, 15, 5);
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
