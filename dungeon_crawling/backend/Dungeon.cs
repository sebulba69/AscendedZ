using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public EventHandler<ITileEvent> TileEventTriggered;

        private Random _rng;
        private int _totalWeight = 0;
        private int _tier;
        private int _eventCount;

        private ITile _currentTile;
        private PathFactory _pathFactory;
        private List<WeightedItem<PathType>> _pathTypes;

        public ITile CurrentTile { get => _currentTile; }

        public Dungeon(int tier)
        {
            _rng = new Random();
            _tier = tier;
            _eventCount = Equations.GetDungeonCrawlEncounters(tier);
            _pathFactory = new PathFactory(_rng);
        }

        public void Generate()
        {
            List<ITile> mainPathTiles = new List<ITile>();

            SetPathFactories();

            Direction primaryDirection = (Direction)_rng.Next(0, Enum.GetNames(typeof(Direction)).Length);
            Direction alternateUpDirection = (_rng.Next(0, 2) == 1) ? Direction.Left : Direction.Right;
            Direction alternateLeftDirection = (_rng.Next(0, 2) == 1) ? Direction.Up : Direction.Down;

            _currentTile = new MainPathTile(primaryDirection);
            ITile tile = _currentTile;
            bool generateEvent = false;
            int eventCount = 0;
            int wait = 0;

            while (eventCount < _eventCount)
            {
                ITile next;

                if (generateEvent)
                {
                    next = _pathFactory.MakePath(primaryDirection, GetPathType());
                }
                else
                {
                    if (wait < 2)
                    {
                        next = new MainPathTile(primaryDirection);
                    }
                    else
                    {
                        next = new MainEncounterTile(primaryDirection);
                        eventCount++;
                    }

                    wait++;
                }

                if(wait == 3)
                {
                    generateEvent = !generateEvent;
                    wait = 0;
                }
                else if(generateEvent)
                    generateEvent = false;

                tile = AttachTiles(tile, next, primaryDirection);

                if(!generateEvent)
                {
                    if(primaryDirection == Direction.Up || primaryDirection == Direction.Down)
                    {
                        primaryDirection = alternateUpDirection;
                    }
                    else
                    {
                        primaryDirection = alternateLeftDirection;
                    }
                }
            }

            ITile last = new ExitTile(primaryDirection);
            tile = AttachTiles(tile, last, primaryDirection);
        }

        private void SetPathFactories()
        {
            _totalWeight = 0;

            _pathTypes = new List<WeightedItem<PathType>>()
            {
                new WeightedItem<PathType>(PathType.Item, 55),
                new WeightedItem<PathType>(PathType.Heal, 20),
                // new WeightedItem<PathType>(PathType.Blacksmith, 15)
            };

            _pathTypes.ForEach(tileGenFunc => _totalWeight += tileGenFunc.Weight);
        }

        private ITile AttachTiles(ITile tile, ITile next, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    tile.Up = next;
                    next.Down = tile;
                    break;
                case Direction.Down:
                    tile.Down = next;
                    next.Up = tile;
                    break;
                case Direction.Left:
                    tile.Left = next;
                    next.Right = tile;
                    break;
                case Direction.Right:
                    tile.Right = next;
                    next.Left = tile;
                    break;
            }

            return next;
        }

        public void MoveDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    if (_currentTile.Up != null)
                    {
                        _currentTile = _currentTile.Up;
                    }
                    break;
                case Direction.Down:
                    if (_currentTile.Down != null)
                    {
                        _currentTile = _currentTile.Down;
                    }
                    break;
                case Direction.Left:
                    if (_currentTile.Left != null)
                    {
                        _currentTile = _currentTile.Left;
                    }
                    break;
                case Direction.Right:
                    if (_currentTile.Right != null)
                    {
                        _currentTile = _currentTile.Right;
                    }
                    break;
            }

            CheckTileEvent();
        }

        private void CheckTileEvent()
        {
            if (!_currentTile.EventTriggered)
            {
                _currentTile.EventTriggered = true;
                ITileEvent tileEvent = _currentTile.GetTileEvent();
                TileEventTriggered?.Invoke(this, tileEvent);
            }
        }

        private PathType GetPathType()
        {
            int random = _rng.Next(_totalWeight);

            PathType pathType = PathType.Item;

            for(int f = 0; f < _pathTypes.Count; f++)
            {
                var factory = _pathTypes[f];
                if(random < factory.Weight)
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
