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
        public int TileCount { get => _tileCount; }
        public bool MaxTileCountReached { get => _maxTileCountReached; set => _maxTileCountReached = value; }
        
        public FloorGenerator(Tile root)
        {
            _rng = new Random();
            _root = root;
        }

        public void Generate(Tile root)
        {
            Stack<Tile> stack = new Stack<Tile>();
            HashSet<int> directions = new HashSet<int>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                int paths = GetNumberOfPaths();
                Tile tile = stack.Pop();

                GetRandomDirections(directions, paths);
                foreach (int dir in directions)
                {
                    Tile tileToAdd = null;
                    switch (dir)
                    {
                        case 0:
                            AddLeft(tile);
                            tileToAdd = tile.Left;
                            break;
                        case 1:
                            AddRight(tile);
                            tileToAdd = tile.Right;
                            break;
                        case 2:
                            AddBLeft(tile);
                            tileToAdd = tile.BottomLeft;
                            break;
                        case 3:
                            AddBRight(tile);
                            tileToAdd = tile.BottomRight;
                            break;
                    }
                    
                    if(tileToAdd != null)
                        stack.Push(tileToAdd);
                }
            }
        }
        
        private int GetNumberOfPaths()
        {
            int numPaths = _rng.Next(1, 5);
            int addedPathCount = numPaths + _tileCount;
            if (addedPathCount >= MaxTileCount)
            {
                numPaths = addedPathCount - MaxTileCount;
                if (numPaths < 0)
                    numPaths = 0;
            }
            return numPaths;
        }

        private void GetRandomDirections(HashSet<int> directions, int paths)
        {
            directions.Clear();
            for (int i = 0; i < paths; i++)
            {
                int dir = _rng.Next(4);

                while (directions.Contains(dir))
                    dir = _rng.Next(4);

                directions.Add(dir);
            }
        }

        private void AddLeft(Tile tile)
        {
            int layer = tile.Y;
            int value = tile.X - 1;

            Tile existing = FindExistingTile(_root, layer, value);

            if (existing == null)
            {
                if (_tileCount + 1 >= MaxTileCount)
                    return;

                tile.Left = new Tile() { Y = layer, X = value };
                tile.Left.BottomRight = tile;
                _tileCount++;
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
                if (_tileCount + 1 >= MaxTileCount)
                    return;

                tile.BottomLeft = new Tile() { Y = layer, X = value };
                tile.BottomLeft.Right = tile;
                _tileCount++;
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
                if(_tileCount + 1 >= MaxTileCount)
                    return;
                
                tile.Right = new Tile() { Y = layer, X = value };
                tile.Right.BottomLeft = tile;
                _tileCount++;
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
                if (_tileCount + 1 >= MaxTileCount)
                    return;

                tile.BottomRight = new Tile() { Y = layer, X = value };
                tile.BottomRight.Left = tile;
                _tileCount++;
            }
            else
            {
                existing.Left = tile;
            }
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
