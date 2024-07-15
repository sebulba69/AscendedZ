using AscendedZ.dungeon_crawling.backend.TileEvents;
using AscendedZ.dungeon_crawling.backend.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    public class DungeonGenerator
    {
        private int _eventCount, _totalWeight;
        private List<WeightedItem<PathType>> _pathTypes;
        private Tile[,] _tiles;
        private Random _rng;

        public Tile Start { get; set; }

        public DungeonGenerator(int tier) 
        { 
            _eventCount = Equations.GetDungeonCrawlEncounters(tier);
            int dimensions = (_eventCount * 2);
            _tiles = new Tile[dimensions, dimensions];

            for (int row = 0; row < dimensions; row++) 
                for (int col = 0; col < dimensions; col++) 
                    _tiles[row, col] = new Tile(row, col);

            _rng = new Random();
            _totalWeight = 0;
            _pathTypes = new List<WeightedItem<PathType>>()
            {
                new WeightedItem<PathType>(PathType.Item, 65),
                new WeightedItem<PathType>(PathType.Heal, 15)
            };

            _pathTypes.ForEach(item => _totalWeight += item.Weight);
        }

        public Tile[,] Generate()
        {
            int length = _tiles.GetLength(0);
            int x = _rng.Next(length);
            int y = _rng.Next(length);
            Start = _tiles[x, y];
            Start.IsPartOfMaze = true;
            Start.Visited = true;

            List<Tile> maze = new List<Tile>();
            List<Tile> walls = new List<Tile>();
            walls.AddRange(GetAdjacentWalls(Start, length));

            while (walls.Count > 0) 
            {
                Tile wall = walls[_rng.Next(walls.Count)];
                
                List<Tile> adjacent = GetAdjacentWalls(wall, length);
                int visitedCount = 0;

                foreach (var tile in adjacent)
                {
                    if (tile.Visited)
                        visitedCount++;
                }

                if(visitedCount == 1)
                {
                    wall.IsPartOfMaze = true;
                    wall.Visited = true;
                    walls.AddRange(adjacent);
                    maze.Add(wall);
                }

                walls.Remove(wall);
            }

            List<TileEventId> generatedPathTypes = new List<TileEventId>();
            for (int e = 0; e < _eventCount; e++) 
            {
                var mazeTile = maze[_rng.Next(1, maze.Count - 1)];
                PathType path = GetPathType();

                if(path == PathType.Item)
                {
                    SetTileToItemTile(mazeTile);
                    generatedPathTypes.Add(mazeTile.TileEventId);
                }

                if (path == PathType.Heal)
                {
                    SetTileToHealing(mazeTile);
                    generatedPathTypes.Add(mazeTile.TileEventId);
                }   
            }

            SetTileToExit(maze[maze.Count - 1]);
            generatedPathTypes.Add(TileEventId.Exit);

            var nonEventTiles = maze.FindAll(tiles => !generatedPathTypes.Contains(tiles.TileEventId));
            for (int e = 0; e < _eventCount; e++)
                SetTileToEncounter(nonEventTiles[_rng.Next(nonEventTiles.Count)]);

            return _tiles;
        }

        /// <summary>
        /// Get adjacent walls. Should only be called once.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private List<Tile> GetAdjacentWalls(Tile tile, int length)
        {
            List<Tile> walls = new List<Tile>();

            int x = tile.X;
            int y = tile.Y;

            // look left
            if (y - 1 >= 0)
            {
                Tile left = _tiles[x, y - 1];
                walls.Add(left);
            }

            // look right
            if (y + 1 < length)
            {
                Tile right = _tiles[x, y + 1];
                walls.Add(right);
            }

            // look up
            if (x - 1 >= 0)
            {
                Tile up = _tiles[x - 1, y];
                walls.Add(up);
            }

            // look down
            if (x + 1 < length)
            {
                Tile down = _tiles[x + 1, y];
                walls.Add(down);
            }

            return walls;
        }

        private void SetTileToItemTile(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/item.png";
            TileEventId id = TileEventId.Item;

            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private void SetTileToHealing(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/health.png";
            TileEventId id = TileEventId.Heal;

            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private void SetTileToEncounter(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/encounter.png";
            TileEventId id = TileEventId.Encounter;

            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private void SetTileToExit(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/exit.png";
            TileEventId id = TileEventId.Exit;

            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private PathType GetPathType()
        {
            int random = _rng.Next(_totalWeight);

            PathType pathType = PathType.Item;

            for (int f = 0; f < _pathTypes.Count; f++)
            {
                var factory = _pathTypes[f];
                if (random < factory.Weight)
                {
                    pathType = factory.Item;
                    break;
                }

                random -= factory.Weight;
            }

            return pathType;
        }
    }
}
