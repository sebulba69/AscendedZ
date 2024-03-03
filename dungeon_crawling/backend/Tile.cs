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
        private bool _occupied = false;
        private bool _isExit = false;
        public Tile Left { get; set; }
        public Tile Right { get; set; }
        public Tile BottomLeft { get; set; }
        public Tile BottomRight { get; set; }
        public int Layer { get; set; }
        public int Value { get; set; }
        public bool Occupied { get => _occupied; set => _occupied = value; }
        public bool IsExit { get => _isExit; set => _isExit = value; }
    }
}
