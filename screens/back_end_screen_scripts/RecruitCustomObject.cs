using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.back_end_screen_scripts
{
    public class RecruitCustomObject
    {
        private int _baseCost;

        // Preview UI
        public OverworldEntity SelectedEntity { get; set; }
        public int Cost { get; set; }

        // Left/Right List Boxes
        public List<OverworldEntity> AvailableMembers { get; set; }
        public List<ISkill> AvailableSkills { get; set; }

        private GameObject _gameObject;

        public RecruitCustomObject()
        {
            AvailableMembers = new List<OverworldEntity>();
            AvailableSkills = new List<ISkill>();
        }

        public void Initialize()
        {
            _gameObject = PersistentGameObjects.GameObjectInstance();
            AvailableMembers = EntityDatabase.MakeShopVendorWares(_gameObject.MaxTier, true);

            for (int i = 0; i < _gameObject.ShopLevel; i++)
                foreach (var member in AvailableMembers)
                    member.LevelUp();

            SetPreviewPartyMember(0);
        }

        public void SetPreviewPartyMember(int index)
        {
            // clear skills from the new entity since we want to customize them
            int costBoost = 0;
            if (SelectedEntity != null)
            {
                SelectedEntity.Skills.Clear();
                costBoost = SelectedEntity.Skills.Count();
            }

            SelectedEntity = AvailableMembers[index];
            Cost = SelectedEntity.Skills.Count + _baseCost;

            List<ISkill> skills = SkillDatabase.GetAllGeneratableSkills(_gameObject.MaxTier);
            AvailableSkills = skills.FindAll(skill => 
            {
                bool isValidSkill = true;

                if(skill.Id == SkillId.Elemental)
                {
                    ElementSkill elementSkill = (ElementSkill)skill;
                    isValidSkill = !(SelectedEntity.Resistances.IsWeakToElement(elementSkill.Element)); // returns true if weak, but that makes it invalid, so we invert it to false
                }

                return isValidSkill;
            });

            int shopLevel = _gameObject.ShopLevel;
            for (int i = 0; i < shopLevel; i++)
            {
                foreach (var skill in AvailableSkills)
                    skill.LevelUp();
            }

            _baseCost = (int)(_gameObject.ShopLevel * 1.5) + 1;
            _baseCost += costBoost;
        }

        public void AddSkill(int index)
        {
            if (SelectedEntity.Skills.Count == 2)
                return;

            if(!DoesSelectedHaveSkill(index))
            {
                SelectedEntity.Skills.Add(AvailableSkills[index].Clone());
                UpdateCost();
            }  
        }

        public void RemoveSkill(int index)
        {
            if (SelectedEntity.Skills.Count == 0)
                return;

            if (DoesSelectedHaveSkill(index))
            {
                SelectedEntity.Skills.RemoveAll(skill =>
                {
                    return AvailableSkills[index].Name.Equals(skill.Name);
                });

                UpdateCost();
            }
        }

        private void UpdateCost()
        {
            Cost = SelectedEntity.Skills.Count + _baseCost;
        }

        private bool DoesSelectedHaveSkill(int index)
        {
            ISkill skillInSelected = SelectedEntity.Skills.Find(skill => skill.Name.Equals(AvailableSkills[index].Name));
            return skillInSelected != null;
        }
    }
}
