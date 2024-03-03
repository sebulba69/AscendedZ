using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    /// <summary>
    /// Dungeon Tile
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The layer above other tiles this node is on
        /// </summary>
        public int Layer { get; set; }
        public int Value { get; set; }
        public Tile Left { get; set; }
        public Tile Right { get; set; }
        public bool Occupied { get; set; }
        /// <summary>
        /// Tiles on lower layers connected to this one
        /// </summary>
        public List<Tile> Connected { get; set; }

        public Tile()
        {
            if (Connected == null)
            {
                Connected = new List<Tile>();
                Occupied = false;
            }
        }
    }
}
