using AscendedZ.currency;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.upgrade_screen
{
    public class UpgradeScreenObject
    {
        private OverworldEntity _selected;
        private List<OverworldEntity> _reserves;
        private Wallet _wallet;

        public UpgradeScreenObject()
        {
            GameObject gameObject = PersistentGameObjects.GameObjectInstance();
            var mainPlayer = gameObject.MainPlayer;

            _reserves = mainPlayer.ReserveMembers;
            _wallet = mainPlayer.Wallet;
            _selected = _reserves[0];
        }

        public void ChangeSelected(int selected)
        {
            _selected = _reserves[selected];
        }

        public void Upgrade()
        {
            int cost = _selected.VorpexValue;
            Currency vorpex = _wallet.Currency[SkillAssets.VORPEX_ICON];

            if (vorpex.Amount >= cost)
            {
                vorpex.Amount -= cost;
                _selected.LevelUp();
                PersistentGameObjects.Save();
            }
        }

        public List<UpgradeItemListDisplay> GetUpgradeItemListDisplays() 
        {
            List<UpgradeItemListDisplay> displays = new List<UpgradeItemListDisplay>();

            foreach (var member in _reserves) 
            {
                UpgradeItemListDisplay display = new UpgradeItemListDisplay()
                {
                    PartyMemberEntry = $"{member.DisplayName} [{member.VorpexValue} VC]",
                    PartyMemberImage = member.Image
                };

                displays.Add(display);
            }

            return displays;
        }

        public UpgradeScreenDisplay GetUpgradeScreenDisplay()
        {
            return new UpgradeScreenDisplay() 
            {
                DisplayName = _selected.DisplayName,
                Image = _selected.Image,
                SelectedUpgradeString = _selected.GetUpgradeString(),
                VorpexAmount = _wallet.Currency[SkillAssets.VORPEX_ICON].Amount.ToString()
            };
        }
    }
}
