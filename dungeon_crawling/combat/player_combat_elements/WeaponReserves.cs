using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.player_combat_elements
{
    public class WeaponReserves
    {
        private const int RESERVE_CAP = 50;

        public List<Weapon> Reserves {  get; set; }
        
        public WeaponReserves() 
        { 
            Reserves = new List<Weapon>();
        }

        public bool AreReservesCapped()
        {
            return Reserves.Count == RESERVE_CAP;
        }

        public bool WillAddingGoOverReserveCap(int amount)
        {
            return amount + Reserves.Count > RESERVE_CAP;
        }

        public void Add(Weapon weapon) 
        {
            if (AreReservesCapped())
                throw new Exception("You need to check if your reserves are full before adding to them from a separate function.");

            Reserves.Add(weapon);
        }

        internal void Remove(Weapon reserve)
        {
            Reserves.Remove(reserve);
        }
    }
}
