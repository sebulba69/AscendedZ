using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    public class DungeonGenerator
    {
        private int _maxTileCount = 12;
        private int _tileCount = 1;
        private bool _maxTileCountReached = false;
        private bool _exitSet = false;

        private Random _rng;
        public int MaxTileCount { get => _maxTileCount; set => _maxTileCount = value; }
        public int TileCount { get => _tileCount; set => _tileCount = value; }
        public bool MaxTileCountReached { get => _maxTileCountReached; set => _maxTileCountReached = value; }
        
        public DungeonGenerator()
        {
            _rng = new Random();
        }

        public void Generate(Tile tile)
        {
            if (TileCount >= MaxTileCount)
                return;

            int numPaths = GetNumberOfPaths();
            TileCount += numPaths;

            if (numPaths == 0)
            {
                if (!_exitSet)
                {
                    tile.IsExit = true;
                    _exitSet = true;
                } 
                return;
            }
            else if (numPaths == 1)
            {
                if (_rng.Next(2) == 0)
                {
                    AddLeft(tile);
                    GenerateR(tile.Left);
                }
                else
                {
                    AddRight(tile);
                    GenerateR(tile.Right);
                }
            }
            else
            {
                AddLeft(tile);
                AddRight(tile);

                GenerateR(tile.Left);
                GenerateR(tile.Right);
            }
        }

        private void AddLeft(Tile tile)
        {
            int layer = tile.Layer + 1;
            int value = tile.Value - 1;

            Tile existing = FindExistingTile(layer, value);

            if (existing == null)
            {
                tile.Left = new Tile() { Layer = layer, Value = value };
                tile.Left.BottomRight = tile;
            }
            else
            {
                existing.BottomRight = tile;
            }
        }

        private void AddRight(Tile tile)
        {
            int layer = tile.Layer + 1;
            int value = tile.Value + 1;

            Tile existing = FindExistingTile(layer, value);
            if (existing == null)
            {
                tile.Right = new Tile() { Layer = layer, Value = value };
                tile.Right.BottomLeft = tile;
            }
            else
            {
                existing.BottomLeft = tile;
            }
        }

        private int GetNumberOfPaths()
        {
            int numPaths = _rng.Next(1, 3);
            int addedPathCount = numPaths + TileCount;
            if (addedPathCount >= MaxTileCount)
            {
                numPaths = addedPathCount - MaxTileCount;
                numPaths--;
                if (numPaths < 0)
                    numPaths = 0;
            }
            return numPaths;
        }

        private Tile FindExistingTile(int value, int layer)
        {
            Tile outputTile = new Tile() { Layer = -1 };
            FindExistingTileRecursive(Root, outputTile, value, layer);

            if (outputTile.Layer == -1)
                return null;
            else
                return outputTile;
        }

        private void FindExistingTileRecursive(Tile searchTile, Tile outputTile, int value, int layer)
        {
            if (outputTile.Layer != -1)
                return;

            if (searchTile.Value == value && searchTile.Layer == layer)
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
