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
    public class RecruitCustomObject
    {
        // Preview UI
        public OverworldEntity SelectedEntity { get; set; }
        public ISkill SelectedSkill { get; set; }
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
            SetPreviewPartyMember(0);
        }

        public void SetPreviewPartyMember(int index)
        {
            // clear skills from the new entity since we want to customize them
            if (SelectedEntity != null)
                SelectedEntity.Skills.Clear();

            SelectedEntity = AvailableMembers[index];
            Cost = (int)(SelectedEntity.ShopCost * 1.5);

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
            Cost = SelectedEntity.ShopCost * (int)(SelectedEntity.Skills.Count * 1.25);
        }

        private bool DoesSelectedHaveSkill(int index)
        {
            ISkill skillInSelected = SelectedEntity.Skills.Find(skill => skill.Name.Equals(AvailableSkills[index].Name));
            return skillInSelected != null;
        }
    }
}
