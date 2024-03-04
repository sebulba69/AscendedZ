using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    public class FloorGenerator
    {
        private const int LAYER_CAP = 5;

        private int _maxTileCount = 10;
        private int _tileCount = 1;
        private bool _maxTileCountReached = false;
        private bool _exitSet = false;
        private Tile _root;

        private Random _rng;
        public int MaxTileCount { get => _maxTileCount; set => _maxTileCount = value; }
        public int TileCount { get => _tileCount; set => _tileCount = value; }
        public bool MaxTileCountReached { get => _maxTileCountReached; set => _maxTileCountReached = value; }
        
        public FloorGenerator(Tile root)
        {
            _rng = new Random();
            _root = root;
        }

        public void Generate(Tile tile)
        {
            if (TileCount >= MaxTileCount)
                return;

            int numPaths = GetNumberOfPaths();

            if(numPaths == 0)
            {
                if (!_exitSet && TileCount >= MaxTileCount)
                {
                    tile.IsExit = true;
                    _exitSet = true;
                }
            }
            else
            {
                int dir = _rng.Next(4);
                HashSet<int> directions = new HashSet<int>();
                for (int i = 0; i < numPaths; i++)
                {
                    while (!directions.Contains(dir))
                    {
                        directions.Add(dir);
                        switch (dir)
                        {
                            case 0:
                                AddLeft(tile);
                                Generate(tile.Left);
                                break;
                            case 1:
                                AddRight(tile);
                                Generate(tile.Right);
                                break;
                            case 2:
                                AddBLeft(tile);
                                Generate(tile.BottomLeft);
                                break;
                            case 3:
                                AddBRight(tile);
                                Generate(tile.BottomRight);
                                break;
                        }
                    }
                    dir = _rng.Next(4);
                }
            }
        }

        private void AddLeft(Tile tile)
        {
            int layer = tile.Y;
            int value = tile.X - 1;

            Tile existing = FindExistingTile(_root, layer, value);

            if (existing == null)
            {
                tile.Left = new Tile() { Y = layer, X = value };
                tile.Left.BottomRight = tile;
                TileCount++;
            }
            else
            {
                existing.BottomRight = tile;
            }
        }

        private void AddBLeft(Tile tile)
        {
            int layer = tile.Y - 1;
            int value = tile.X;

            Tile existing = FindExistingTile(_root, layer, value);

            if (existing == null)
            {
                tile.BottomLeft = new Tile() { Y = layer, X = value };
                tile.BottomLeft.Right = tile;
                TileCount++;
            }
            else
            {
                existing.Right = tile;
            }
        }

        private void AddRight(Tile tile)
        {
            int layer = tile.Y;
            int value = tile.X + 1;

            Tile existing = FindExistingTile(_root, layer, value);
            if (existing == null)
            {
                tile.Right = new Tile() { Y = layer, X = value };
                tile.Right.BottomLeft = tile;
                TileCount++;
            }
            else
            {
                existing.BottomLeft = tile;
            }
        }

        private void AddBRight(Tile tile)
        {
            int layer = tile.Y + 1;
            int value = tile.X;

            Tile existing = FindExistingTile(_root, layer, value);
            if (existing == null)
            {
                tile.BottomRight = new Tile() { Y = layer, X = value };
                tile.BottomRight.Left = tile;
                TileCount++;
            }
            else
            {
                existing.Left = tile;
            }
        }

        private int GetNumberOfPaths()
        {
            int numPaths = _rng.Next(1, 5);
            int addedPathCount = numPaths + TileCount;
            if (addedPathCount >= MaxTileCount)
            {
                numPaths = addedPathCount - MaxTileCount;
                if (numPaths < 0)
                    numPaths = 0;
            }
            return numPaths;
        }

        private Tile FindExistingTile(Tile root, int value, int layer)
        {
            Tile outputTile = new Tile() { Y = -1 };
            FindExistingTileRecursive(root, outputTile, value, layer);

            if (outputTile.Y == -1)
                return null;
            else
                return outputTile;
        }

        private void FindExistingTileRecursive(Tile searchTile, Tile outputTile, int value, int layer)
        {
            if (outputTile.Y != -1)
                return;

            if (searchTile.X == value && searchTile.Y == layer)
            {
                outputTile = searchTile;
                return;
            }

            if (searchTile.Left != null)
                FindExistingTileRecursive(searchTile.Left, outputTile, value, layer);

            if (searchTile.Right != null)
                FindExistingTileRecursive(searchTile.Right, outputTile, value, layer);
        }
    }
}
