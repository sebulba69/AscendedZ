using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.Tiles
{
    public class ItemTile : ITile
    {
        public string Graphic { get => "res://dungeon_crawling/art_assets/entity_icons/item.png"; }
        public bool IsMainTile { get; set; } = false;
        public bool IsExit { get; set; }
        public ITile Left { get; set; }
        public ITile Right { get; set; }
        public ITile Up { get; set; }
        public ITile Down { get; set; }

        public void Enter()
        {
            
        }

        public virtual Direction GetDirection()
        {
            throw new NotImplementedException();
        }
    }
}
