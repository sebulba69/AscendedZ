using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.Tiles
{
    /// <summary>
    /// A tile that is not part of any side event path.
    /// </summary>
    public class MainPathTile : ITile
    {
        public string Graphic { get => ""; }
        public bool IsMainTile { get; set; } = true;
        public bool IsExit { get; set; }
        public ITile Left { get; set; }
        public ITile Right { get; set; }
        public ITile Up { get; set; }
        public ITile Down { get; set; }

        private Direction _direction;

        public MainPathTile(Direction direction)
        {
            _direction = direction;
        }

        public void Enter()
        {

        }

        public Direction GetDirection()
        {
            return _direction;
        }
    }
}
