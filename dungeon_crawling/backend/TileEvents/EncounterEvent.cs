using AscendedZ.dungeon_crawling.backend.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.TileEvents
{
    public class EncounterEvent : ITileEvent
    {
        public TileEventId Id { get => TileEventId.Encounter; }
        public MainEncounterTile Tile { get; set; }
    }
}
