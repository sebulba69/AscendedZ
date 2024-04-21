using AscendedZ.dungeon_crawling.backend.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.PathMakers
{
    public class ItemPathFactory : PathFactory, IPathFactory
    {
        public ItemPathFactory(Random rng) : base(rng) {}

        public ITile MakePath(Direction direction)
        {
            ITile startOfPath = new MainPathTile(direction);
            ITile itemTile = new ItemTile();

            MakePathFromDirection(startOfPath, itemTile, direction);

            return startOfPath;
        }


    }
}
