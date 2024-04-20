using AscendedZ.dungeon_crawling.backend.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.PathMakers
{
    public class ShopPathFactory : IPathFactory
    {
        private Random _rng;

        public ShopPathFactory(Random rng)
        {
            _rng = rng;
        }

        public ITile MakePath()
        {
            ITile startOfPath = new MainPathTile();
            ITile shopTile = new ShopTile();

            if (_rng.Next(0, 1) == 1)
            {
                startOfPath.Up = shopTile;
                shopTile.Down = startOfPath;
            }
            else
            {
                startOfPath.Down = shopTile;
                shopTile.Up = startOfPath;
            }

            return startOfPath;
        }
    }
}
