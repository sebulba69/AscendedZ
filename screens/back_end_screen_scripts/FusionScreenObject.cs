using AscendedZ.currency;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.back_end_screen_scripts
{
    public class FusionScreenObject
    {
        private List<FusionObject> _fusions;

        private int _fusionIndex;

        public int FusionIndex { get => _fusionIndex; set => _fusionIndex = value; }
        public List<FusionObject> Fusions { get => _fusions; }
        public FusionObject DisplayFusion { get => _fusions[FusionIndex]; }

        private Currency _partyCoins;

        public int OwnedPartyCoins { get => _partyCoins.Amount; }

        public FusionScreenObject()
        {
            _fusions = new List<FusionObject>();
            _fusionIndex = 0;
            var mp = PersistentGameObjects.GameObjectInstance().MainPlayer;
            _partyCoins = mp.Wallet.Currency[SkillAssets.PARTY_COIN_ICON];
        }

        public bool TryFuse()
        {
            bool isSuccessful = false;

            if (DisplayFusion.Fusion.Skills.Count > 0)
            {
                if (GetCost() > OwnedPartyCoins)
                    return false;

                MainPlayer mainPlayer = PersistentGameObjects.GameObjectInstance().MainPlayer;

                RemoveMaterialFromMainPlayer(mainPlayer, DisplayFusion.Material1);
                RemoveMaterialFromMainPlayer(mainPlayer, DisplayFusion.Material2);

                foreach(var reserve in mainPlayer.ReserveMembers)
                {
                    if (reserve.Name.Equals(DisplayFusion.Fusion.Name))
                        return false;
                }

                _partyCoins.Amount -= GetCost();

                foreach (var skill in DisplayFusion.Fusion.Skills) 
                {
                    for (int f = 0; f < DisplayFusion.Fusion.FusionGrade; f++)
                    {
                        skill.LevelUp();
                    }
                }

                mainPlayer.ReserveMembers.Add(DisplayFusion.Fusion);

                _fusions.Remove(DisplayFusion);

                if (FusionIndex == _fusions.Count)
                    FusionIndex = _fusions.Count - 1;

                if (FusionIndex < 0)
                    FusionIndex = 0;

                isSuccessful = true;

                PersistentGameObjects.Save();
            }

            return isSuccessful;
        }

        public bool IsCorrectTier()
        {
            int grade = DisplayFusion.Fusion.FusionGrade;
            int tier = PersistentGameObjects.GameObjectInstance().MaxTier;

            int fusionTierReq = TierRequirements.GetFusionTierRequirement(grade);

            if (fusionTierReq < 0)
                return false;
            else
                return tier > fusionTierReq;
        }

        public bool IsCorrectFusionLevel()
        {
            int level = DisplayFusion.Fusion.FusionGrade * 10;
            var m1 = DisplayFusion.Material1;
            var m2 = DisplayFusion.Material2;

            return m1.Level >= level && m2.Level >= level;
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
                            if(fusions.Count > 0 && reserves.Find(r => r.Name == fusions[0].Fusion.Name) == null)
                                _fusions.AddRange(fusions);
                        }
                    }
                }
            }

            if (_fusionIndex == _fusions.Count)
                _fusionIndex = _fusions.Count - 1;

            if (_fusionIndex < 0)
                _fusionIndex = 0;

            _fusions = _fusions.OrderBy(fusion => fusion.Fusion.Name).ToList();
        }

        public int GetCost()
        {
            if (_fusions.Count == 0)
                return -1;
            else
            {
                int grade = DisplayFusion.Fusion.FusionGrade - 1;
                return grade * 50;
            }
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

            skills.AddRange(PopulateSubFusionList(DisplayFusion.Material1));
            skills.AddRange(PopulateSubFusionList(DisplayFusion.Material2));

            return skills;
        }

        private List<ISkill> PopulateSubFusionList(OverworldEntity displayFusion)
        {
            List<ISkill> skills = new List<ISkill>();
            foreach (var skill in displayFusion.Skills)
            {
                skills.Add(skill);
            }
            return skills;
        }

        public void AddOrRemoveSkillFromFusion(int selected)
        {
            List<ISkill> skills = GetFusionMaterialSkills();
            ISkill skill = skills[selected];
            var fusionSkills = DisplayFusion.Fusion.Skills;
            var searchSkill = fusionSkills.Find((fSkill) => fSkill.BaseName == skill.BaseName);

            if (searchSkill == null)
            {
                if(fusionSkills.Count < DisplayFusion.Fusion.SkillCap)
                {
                    fusionSkills.Add(skill);
                }
            }
            else
            {
                if(fusionSkills.Contains(skill)) 
                {
                    fusionSkills.Remove(searchSkill);
                }
                
            }
        }
    }
}
