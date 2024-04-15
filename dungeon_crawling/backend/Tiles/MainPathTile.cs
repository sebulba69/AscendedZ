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
    public class MainPathTile : NormalTile 
    {
        public MainPathTile()
        {
            IsMainTile = true;
        }
    }
}
