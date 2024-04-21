using AscendedZ.dungeon_crawling.backend.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.PathMakers
{
    public class HealPathFactory : PathFactory, IPathFactory
    {
        public string Graphic { get => ""; }
        public HealPathFactory(Random rng) : base(rng) { }

        public ITile MakePath(Direction direction)
        {
            ITile startOfPath = new MainPathTile(direction);
            ITile healTile = new HealTile();

            MakePathFromDirection(startOfPath, healTile, direction);

            return startOfPath;
        }
    }
}
