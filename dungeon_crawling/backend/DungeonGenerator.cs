
using AscendedZ.entities;
using AscendedZ.game_object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    public class DungeonGenerator
    {
        private bool _bossEncounter;
        private int _eventCount, _totalWeight, _tier;
        private List<WeightedItem<PathType>> _pathTypes;
        private Tile[,] _tiles;
        private Random _rng;

        public int Encounters { get; set; }
        public Tile Start { get; set; }
        public Tile Exit { get; set; }

        public List<SpecialTile> SpecialTiles { get; set; }

        public DungeonGenerator(int tier) 
        {
            _tier = tier;
            _eventCount = Equations.GetDungeonCrawlEvents(tier) * 2;
            int dimensions = _eventCount;

            if (_tier < 10)
            {
                _eventCount /= 2;
                Encounters = (_eventCount / 2) + 1;
            }
            else
            {
                Encounters = (_eventCount / 4) + 1;
            }
            
            if (tier % 50 == 0)
            {
                _bossEncounter = true;
                dimensions = 5;
            }
            
            _tiles = new Tile[dimensions, dimensions];

            for (int row = 0; row < dimensions; row++) 
                for (int col = 0; col < dimensions; col++) 
                    _tiles[row, col] = new Tile(row, col);

            SpecialTiles = new List<SpecialTile>();

            _rng = new Random();
            _totalWeight = 0;
            _pathTypes = new List<WeightedItem<PathType>>()
            {
                new WeightedItem<PathType>(PathType.Item, 45),
                new WeightedItem<PathType>(PathType.Heal, 25)
            };

            if (dimensions > 4)
            {
                _pathTypes.Add(new WeightedItem<PathType>(PathType.PotOfGreed, 20));
                _pathTypes.Add(new WeightedItem<PathType>(PathType.SpecialItem, 20));
            }

            if(dimensions > 6)
            {
                _pathTypes.Add(new WeightedItem<PathType>(PathType.BuceOrb, 20));
                _pathTypes.Add(new WeightedItem<PathType>(PathType.SpecialEncounter, 50));
            }

            _pathTypes.ForEach(item => _totalWeight += item.Weight);
        }

        public Tile[,] Generate()
        {
            if (!_bossEncounter)
            {
                return GenerateDungeonNormal();
            }
            else
            {
                int column = 2;
                for(int r = 0; r < _tiles.GetLength(0); r++)
                {
                    _tiles[r, column].IsPartOfMaze = true;
                }

                var encounter = EntityDatabase.MakeBattleEncounter(_tier + 5, true);
                Start = _tiles[_tiles.GetLength(0) - 1, column];
                var end = _tiles[0, column];
                var boss = _tiles[1, column];
                var dialog = _tiles[2, column];

                SetTileToExit(end);
                SetTileToEncounter(boss);

                var dcCutscenes = PersistentGameObjects.GameObjectInstance().ProgressFlagObject.DCCutsceneSeen;
                if((_tier/50) - 1 >= dcCutscenes.Count)
                {
                    SetTileToDialog(dialog, encounter[0].Name, encounter[0].Image);
                }

                boss.Graphic = encounter[0].Image;
                Encounters = 1;
                return _tiles;
            }
        }

        private Tile[,] GenerateDungeonNormal()
        {
            int length = _tiles.GetLength(0);
            int x = _rng.Next(length);
            int y = _rng.Next(length);
            Start = _tiles[x, y];
            Start.IsPartOfMaze = true;
            Start.Visited = true;
            Start.TileEventId = TileEventId.Start;

            List<Tile> maze = new List<Tile>();
            List<Tile> openTiles = new List<Tile>();
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

                if (visitedCount == 1)
                {
                    wall.IsPartOfMaze = true;
                    wall.Visited = true;
                    walls.AddRange(adjacent);
                    maze.Add(wall);
                    if(wall.TileEventId == TileEventId.None)
                        openTiles.Add(wall);
                }

                walls.Remove(wall);
            }

            List<TileEventId> generatedPathTypes = new List<TileEventId>();

            generatedPathTypes.Add(TileEventId.Start);
            openTiles.Remove(Start);
            for (int e = 0; e < _eventCount; e++)
            {
                var mazeTile = openTiles[_rng.Next(openTiles.Count)];
                PathType path = GetPathType();

                switch (path)
                {
                    case PathType.Item:
                        SetTileToItemTile(mazeTile);
                        RemoveGeneratedTileFromOpen(openTiles, generatedPathTypes, mazeTile);
                        break;

                    case PathType.SpecialItem:
                        SetTileToSpecialItemTile(mazeTile);
                        RemoveGeneratedTileFromOpen(openTiles, generatedPathTypes, mazeTile);
                        break;

                    case PathType.SpecialEncounter:
                        SetTileToSpecialEncounter(mazeTile);
                        RemoveGeneratedTileFromOpen(openTiles, generatedPathTypes, mazeTile);
                        break;

                    case PathType.Heal:
                        SetTileToHealing(mazeTile);
                        RemoveGeneratedTileFromOpen(openTiles, generatedPathTypes, mazeTile);
                        break;

                    case PathType.BuceOrb:
                        SetTileToBuceOrb(mazeTile);
                        RemoveGeneratedTileFromOpen(openTiles, generatedPathTypes, mazeTile);
                        break;

                    case PathType.PotOfGreed:
                        SetTileToPotOfGreed(mazeTile);
                        RemoveGeneratedTileFromOpen(openTiles, generatedPathTypes, mazeTile);
                        break;

                }
            }

            int dimensions = _tiles.GetLength(0);

            if(dimensions > 4)
            {
                int tpNumber = _tier/10;

                for(int t = 0; t < tpNumber; t++)
                {
                    var tile1 = openTiles[_rng.Next(openTiles.Count)];
                    openTiles.Remove(tile1);
                    var tile2 = openTiles[_rng.Next(openTiles.Count)];
                    openTiles.Remove(tile2);

                    SetTilesToTeleporters(tile1, tile2);
                    generatedPathTypes.Add(tile1.TileEventId);
                }
            }

            if(dimensions > 8)
            {
                var fountain = openTiles[_rng.Next(openTiles.Count)];
                SetTileToFountain(fountain);
                generatedPathTypes.Add(fountain.TileEventId);
                openTiles.Remove(fountain);

                var miner = openTiles[_rng.Next(openTiles.Count)];
                SetMinerTile(miner);
                generatedPathTypes.Add(miner.TileEventId);
                openTiles.Remove(miner);

                if (_tier % 3 == 0)
                {
                    var boss = openTiles[_rng.Next(openTiles.Count)];
                    SetTileToSpecialBossEncounter(boss);
                    generatedPathTypes.Add(boss.TileEventId);
                    openTiles.Remove(boss);
                }
                   
            }

            var exit = openTiles[_rng.Next(openTiles.Count)];
            SetTileToExit(exit);
            generatedPathTypes.Add(TileEventId.Exit);
            openTiles.Remove(exit);

            int encounterCount = Encounters * 2;
            for (int e = 0; e < encounterCount; e++)
            {
                SetTileToEncounter(openTiles[_rng.Next(openTiles.Count)]);
            }

            return _tiles;
        }

        private void RemoveGeneratedTileFromOpen(List<Tile> openTiles, List<TileEventId> generatedPathTypes, Tile mazeTile)
        {
            generatedPathTypes.Add(mazeTile.TileEventId);
            openTiles.Remove(mazeTile);
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

        private void SetTileToDialog(Tile tile, string name, string image)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/dialog.png";
            TileEventId id = TileEventId.BossDialog;
            tile.Entity = name;
            tile.EntityImage = image;
            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private void SetMinerTile(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/miner.png";
            TileEventId id = TileEventId.Miner;

            tile.Graphic = graphic;
            tile.TileEventId = id;

            SpecialTiles.Add(new SpecialTile() { Graphic = tile.Graphic, Coordinates = new int[2] { tile.X, tile.Y }, Id = tile.TileEventId });
        }

        private void SetTileToItemTile(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/item.png";
            TileEventId id = TileEventId.Item;

            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private void SetTileToSpecialItemTile(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/special_item.png";
            TileEventId id = TileEventId.SpecialItem;

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

        private void SetTileToSpecialEncounter(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/special_encounter.png";
            TileEventId id = TileEventId.SpecialEncounter;

            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private void SetTileToSpecialBossEncounter(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/bossdoor.png";
            TileEventId id = TileEventId.SpecialBossEncounter;

            tile.Graphic = graphic;
            tile.TileEventId = id;

            SpecialTiles.Add(new SpecialTile() { Graphic = tile.Graphic, Coordinates = new int[2] { tile.X, tile.Y }, Id = tile.TileEventId });
        }

        private void SetTileToExit(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/exit.png";
            TileEventId id = TileEventId.Exit;

            tile.Graphic = graphic;
            tile.TileEventId = id;

            Exit = tile;

            SpecialTiles.Add(new SpecialTile() { Graphic = tile.Graphic, Coordinates = new int[2] { tile.X, tile.Y }, Id = tile.TileEventId });
        }

        private void SetTileToPotOfGreed(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/pot_of_greed.png";
            TileEventId id = TileEventId.PotOfGreed;

            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private void SetTileToBuceOrb(Tile tile) 
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/buceorb.png";
            TileEventId id = TileEventId.Orb;

            tile.Graphic = graphic;
            tile.TileEventId = id;
        }

        private void SetTileToFountain(Tile tile)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/bucetain.png";
            TileEventId id = TileEventId.Fountain;

            tile.Graphic = graphic;
            tile.TileEventId = id;

            SpecialTiles.Add(new SpecialTile() { Graphic = tile.Graphic, Coordinates = new int[2] { tile.X, tile.Y }, Id = tile.TileEventId });
        }

        private void SetTilesToTeleporters(Tile tile1, Tile tile2)
        {
            string graphic = "res://dungeon_crawling/art_assets/entity_icons/portal.png";
            TileEventId id = TileEventId.Portal;

            tile1.TPLocation = new int[] { tile2.X, tile2.Y };
            tile2.TPLocation = new int[] { tile1.X, tile1.Y };

            tile1.Graphic = graphic;
            tile1.TileEventId = id;

            tile2.Graphic = graphic;
            tile2.TileEventId = id;
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
