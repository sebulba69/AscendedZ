using AscendedZ.dungeon_crawling.backend.TileEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.Tiles
{
    public class ItemTile : ITile
    {
        public bool EventTriggered { get; set; } = false;
        public string Graphic { get => "res://dungeon_crawling/art_assets/entity_icons/item.png"; }
        public bool IsMainTile { get; set; } = false;
        public ITile Left { get; set; }
        public ITile Right { get; set; }
        public ITile Up { get; set; }
        public ITile Down { get; set; }

        public virtual Direction GetDirection()
        {
            throw new NotImplementedException();
        }

        public ITileEvent GetTileEvent()
        {
            return new ItemEvent() { Tile = this };
        }
    }
}
