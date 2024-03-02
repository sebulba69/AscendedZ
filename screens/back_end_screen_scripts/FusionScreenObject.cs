using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.back_end_screen_scripts
{
    public class FusionScreenObject
    {
        private static FusionScreenObject _fusionScreenObject;
        public static FusionScreenObject Instance()
        {
            return new FusionScreenObject();
        }

        private List<FusionObject> _fusions;

        private int _fusionIndex;

        public int FusionIndex { get => _fusionIndex; set => _fusionIndex = value; }
        public List<FusionObject> Fusions { get => _fusions; }
        public FusionObject DisplayFusion { get => _fusions[FusionIndex]; }

        public FusionScreenObject()
        {
            _fusions = new List<FusionObject>();
            _fusionIndex = 0;
        }

        public bool TryFuse()
        {
            bool isSuccessful = false;

            if (DisplayFusion.Fusion.Skills.Count > 0)
            {
                MainPlayer mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;
                RemoveMaterialFromMainPlayer(mainPlayer, DisplayFusion.Material1);
                RemoveMaterialFromMainPlayer(mainPlayer, DisplayFusion.Material2);

                foreach(var reserve in mainPlayer.ReserveMembers)
                {
                    if (reserve.Name.Equals(DisplayFusion.Fusion.Name))
                        return false;
                }

                int upgradeShardYield = (DisplayFusion.Material1.UpgradeShardYield + DisplayFusion.Material2.UpgradeShardYield) * (DisplayFusion.Fusion.Grade + 1);
                upgradeShardYield += (10 * DisplayFusion.Fusion.Skills.Count);
                
                if (FusionIndex == _fusions.Count)
                    FusionIndex = _fusions.Count - 1;

                if (FusionIndex < 0)
                    FusionIndex = 0;

                var newMember = PartyMemberGenerator.MakePartyMember(DisplayFusion.Fusion.DisplayName);

                newMember.MaxHP = (DisplayFusion.Material1.MaxHP + DisplayFusion.Material2.MaxHP) / 2;

                newMember.Level = (DisplayFusion.Material1.Level + DisplayFusion.Material2.Level) / 2;

                newMember.VorpexValue = (DisplayFusion.Material1.VorpexValue + DisplayFusion.Material2.VorpexValue) / 2;

                newMember.Skills.AddRange(DisplayFusion.Fusion.Skills);

                newMember.UpgradeShardYield = upgradeShardYield;

                mainPlayer.ReserveMembers.Add(newMember);

                _fusions.Remove(DisplayFusion);

                isSuccessful = true;

                PersistentGameObjects.Save();
            }

            return isSuccessful;
        }

        private void RemoveMaterialFromMainPlayer(MainPlayer mainPlayer, OverworldEntity material)
        {
            if (material.IsInParty)
                mainPlayer.Party.RemovePartyMember(material);

            mainPlayer.ReserveMembers.Remove(material);
        }

        public void PopulateMaterialFusionList()
        {
            _fusions.Clear();

            List<OverworldEntity> reserves = PersistentGameObjects.GameObjectInstance().MainPlayer.ReserveMembers;

            // save the materials + fusion results
            for(int m1 = 0; m1 < reserves.Count; m1++)
            {
                for(int m2 = 0; m2 < reserves.Count; m2++)
                {
                    if(m1 != m2)
                    {
                        if (!IsFusionRecipeGenerated(reserves[m1], reserves[m2]))
                        {
                            var fusions = EntityDatabase.MakeFusionEntities(reserves[m1], reserves[m2]);
                            if(fusions.Count > 0)
                                _fusions.AddRange(fusions);
                        }
                    }
                }
            }

            _fusions = _fusions.OrderBy(fusion => fusion.Fusion.Name).ToList();
        }

        private bool IsFusionRecipeGenerated(OverworldEntity m1, OverworldEntity m2)
        {
            bool isGenerated = false;
            
            foreach(var fusion in _fusions)
            {
                if(fusion.Material1 == m2 && fusion.Material2 == m1)
                {
                    isGenerated = true;
                    break;
                }
            }

            return isGenerated;
        }

        public List<ISkill> GetFusionMaterialSkills()
        {
            List<ISkill> skills = new List<ISkill>();

            skills.AddRange(DisplayFusion.Material1.Skills);
            skills.AddRange(DisplayFusion.Material2.Skills);

            return skills;
        }

        public void AddOrRemoveSkillFromFusion(int selected)
        {
            List<ISkill> skills = GetFusionMaterialSkills();
            ISkill skill = skills[selected];
            var fusionSkills = DisplayFusion.Fusion.Skills;

            if (!fusionSkills.Contains(skill))
            {
                if(fusionSkills.Count < DisplayFusion.Fusion.SkillCap)
                    fusionSkills.Add(skill);
            }
            else
            {
                fusionSkills.Remove(skill);
            }
        }
    }
}
