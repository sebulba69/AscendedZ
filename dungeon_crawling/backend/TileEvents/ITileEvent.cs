using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.TileEvents
{
    public interface ITileEvent
    {
        public TileEventId Id { get; }
    }
}
