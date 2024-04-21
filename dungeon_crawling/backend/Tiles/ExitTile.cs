using AscendedZ.dungeon_crawling.backend.TileEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.Tiles
{
    public class ExitTile : ITile
    {
        public bool EventTriggered { get; set; } = false;
        public string Graphic { get => "res://dungeon_crawling/art_assets/entity_icons/exit.png"; }
        public bool IsMainTile { get; set; } = true;
        public ITile Left { get; set; }
        public ITile Right { get; set; }
        public ITile Up { get; set; }
        public ITile Down { get; set; }

        private Direction _direction;

        public ExitTile(Direction direction)
        {
            _direction = direction;
        }

        public Direction GetDirection()
        {
            return _direction;
        }

        public ITileEvent GetTileEvent()
        {
            return new ExitTileEvent();
        }
    }
}
