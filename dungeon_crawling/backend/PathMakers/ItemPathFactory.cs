using AscendedZ.dungeon_crawling.backend.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.PathMakers
{
    public class ItemPathFactory : IPathFactory
    {
        private Random _rng;

        public ItemPathFactory(Random rng)
        {
            _rng = rng;
        }

        public ITile MakePath()
        {
            ITile startOfPath = new MainPathTile();
            ITile encounterTile = new EncounterTile();
            ITile itemTile = new ItemTile();

            // left or right
            if (_rng.Next(0, 1) == 1)
            {
                encounterTile.Left = itemTile;
                itemTile.Right = encounterTile;
            }
            else
            {
                encounterTile.Right = itemTile;
                itemTile.Left = encounterTile;
            }

            // down
            if (_rng.Next(0, 1) == 1)
            {
                startOfPath.Down = encounterTile;
                encounterTile.Up = startOfPath;
            }
            else
            {
                startOfPath.Up = encounterTile;
                encounterTile.Down = startOfPath;
            }

            return startOfPath;
        }
    }
}
