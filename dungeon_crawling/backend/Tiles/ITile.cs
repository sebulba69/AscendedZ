using AscendedZ.dungeon_crawling.backend.TileEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.Tiles
{
    public interface ITile
    {
        bool EventTriggered { get; set; }
        string Graphic { get; }
        bool IsMainTile { get; set; }
        public ITile Left { get; set; }
        public ITile Right { get; set; }
        public ITile Up { get; set; }
        public ITile Down { get; set; }
        Direction GetDirection();
        ITileEvent GetTileEvent();
    }
}
