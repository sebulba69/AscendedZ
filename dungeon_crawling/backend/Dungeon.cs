using System;
using System.Collections.Generic;


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
        public Tile Exit { get=> _genenerator.Exit; }
        public bool CanLeave { get => EncounterCount <= 0; }

        public int EncounterCount { get=> _genenerator.Encounters; }

        public Dungeon(int tier)
        {
            _genenerator = new DungeonGenerator(tier);
        }

        public void Generate()
        {
            _dungeon = _genenerator.Generate();
            _currentTile = _genenerator.Start;
        }

        public void MoveDirection(int x, int y, bool isTeleport = false)
        {
            _currentTile = _dungeon[x, y];
            if(!isTeleport)
                CheckTileEvent();
        }

        public void Mine(int x, int y)
        {
            _dungeon[x, y].IsPartOfMaze = true;
        }

        public void ProcessEncounter()
        {
            _genenerator.Encounters--;
            if(_genenerator.Encounters <= 0)
            {
                _genenerator.Encounters = 0;
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
