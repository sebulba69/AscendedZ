using AscendedZ.currency;
using AscendedZ.entities.partymember_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities
{
    /// <summary>
    /// The main character that controls a set of 3 PartyMembers.
    /// This character does not participate directly in combat
    /// and is meant to function more as a way of keeping track of
    /// the player's current progress.
    /// </summary>
    public class MainPlayer : Entity
    {
        public Wallet Wallet { get; set; }
        public PlayerParty Party { get; set; }

        public MainPlayer()
        {
            if (Wallet == null)
                Wallet = new Wallet();

            if (Party == null)
                Party = new PlayerParty();
        }

        /// <summary>
        /// Reserve party members not in battle.
        /// </summary>
        public List<OverworldEntity> ReserveMembers { get; set; } = new();

        public bool IsPartyMemberOwned(string partyMemberName)
        {
            bool isPartyMemberOwned = false;
            int count = this.ReserveMembers.FindAll(member => member.Name.Equals(partyMemberName)).Count;
            isPartyMemberOwned = count > 0;
            
            // check player party
            if(!isPartyMemberOwned)
            {
                foreach(var member in this.Party.Party)
                {
                    if(member!=null 
                        && member.Name.Equals(partyMemberName))
                    {
                        isPartyMemberOwned = true;
                        break;
                    }
                }
            }

            return isPartyMemberOwned;
        }
    }
}
