using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.dungeon_crawling.backend.TileEvents;
using AscendedZ.dungeon_crawling.backend.Tiles;

public enum Direction
{
    Left, Up, Right, Down
}

namespace AscendedZ.dungeon_crawling.backend
{
    /// <summary>
    /// Dungeon that exists as a straight line.
    /// Contains 20 encounters divided into groups.
    /// 20 tiles from left to right
    ///     Encounters laid out like so (None, Encounter, None, Event, None, Encounter)
    /// </summary>
    public class Dungeon
    {
        public EventHandler<TileEventId> TileEventTriggered;
        private DungeonGenerator _genenerator;
        private List<WeightedItem<PathType>> _pathTypes;

        private Tile[,] _dungeon;
        private Tile _currentTile;

        public Tile[,] Tiles { get => _dungeon; }
        public Tile Current { get => _currentTile; }

        public Dungeon(int tier)
        {
            _genenerator = new DungeonGenerator(tier);
        }

        public void Generate()
        {
            _dungeon = _genenerator.Generate();
            _currentTile = _genenerator.Start;
        }

        public void MoveUp()
        {
            if(_currentTile.X - 1 >= 0)
            {
                if (Tiles[_currentTile.X - 1, _currentTile.Y].IsPartOfMaze)
                {
                    _currentTile = Tiles[_currentTile.X - 1, _currentTile.Y];
                    CheckTileEvent();
                }
            }
        }

        public void MoveDown()
        {
            if (_currentTile.X + 1 < Tiles.GetLength(0))
            {
                if (Tiles[_currentTile.X + 1, _currentTile.Y].IsPartOfMaze)
                {
                    _currentTile = Tiles[_currentTile.X + 1, _currentTile.Y];
                    CheckTileEvent();
                }
            }
        }


        public void MoveLeft()
        {
            if (_currentTile.Y - 1 >= 0)
            {
                if (Tiles[_currentTile.X, _currentTile.Y - 1].IsPartOfMaze)
                {
                    _currentTile = Tiles[_currentTile.X, _currentTile.Y - 1];
                    CheckTileEvent();
                }
            }
        }

        public void MoveRight()
        {
            if (_currentTile.Y + 1 < Tiles.GetLength(1))
            {
                if (Tiles[_currentTile.X, _currentTile.Y + 1].IsPartOfMaze)
                {
                    _currentTile = Tiles[_currentTile.X, _currentTile.Y + 1];
                    CheckTileEvent();
                }
            }
        }

        private void CheckTileEvent()
        {
            if (!_currentTile.EventTriggered)
            {
                _currentTile.EventTriggered = true;
                TileEventTriggered?.Invoke(this, _currentTile.TileEventId);
            }
        }

    }
}
