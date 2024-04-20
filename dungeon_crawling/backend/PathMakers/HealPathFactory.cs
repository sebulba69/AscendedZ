using AscendedZ.dungeon_crawling.backend.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.PathMakers
{
    public class HealPathFactory : IPathFactory
    {
        private Random _rng;

        public HealPathFactory(Random rng)
        {
            _rng = rng;
        }

        public ITile MakePath()
        {
            ITile startOfPath = new MainPathTile();
            ITile encounterTile = new EncounterTile();
            ITile itemTile = new ItemTile();
            ITile healTile = new HealTile();

            encounterTile.Left = itemTile;
            encounterTile.Right = healTile;
            itemTile.Right = encounterTile;
            healTile.Left = encounterTile;

            if (_rng.Next(0, 1) == 1)
            {
                startOfPath.Up = encounterTile;
                encounterTile.Down = startOfPath;
            }
            else
            {
                startOfPath.Down = encounterTile;
                encounterTile.Up = startOfPath;
            }

            return startOfPath;
        }
    }
}
