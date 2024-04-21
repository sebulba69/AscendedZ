using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.dungeon_crawling.backend.PathMakers;
using AscendedZ.dungeon_crawling.backend.Tiles;
using Godot;
using static Godot.TextServer;

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
        private Random _rng;
        private int _totalWeight = 0;
        private int _level;
        private int _eventCount;

        private List<WeightedItem<IPathFactory>> _tileGenFunctions;
        private ITile _currentTile;

        public ITile CurrentTile { get => _currentTile; }

        public Dungeon(int level, int eventCount)
        {
            _rng = new Random();
            _level = level;
            _eventCount = eventCount;
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

            while (eventCount < _eventCount)
            {
                ITile next;

                if (generateEvent)
                {
                    next = GetEventPath(primaryDirection);
                    eventCount++;
                }
                else
                {
                    next = new MainEncounterTile(primaryDirection);
                }

                generateEvent = !generateEvent;

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
            IPathFactory itemPathFactory = new ItemPathFactory(_rng);
            IPathFactory healPathFactory = new HealPathFactory(_rng);
            IPathFactory shopPathFactory = new ShopPathFactory(_rng);

            _tileGenFunctions = new List<WeightedItem<IPathFactory>>()
            {
                new WeightedItem<IPathFactory>(itemPathFactory, 55),
                new WeightedItem<IPathFactory>(healPathFactory, 20),
                new WeightedItem<IPathFactory>(shopPathFactory, 15)
            };

            _tileGenFunctions.ForEach(tileGenFunc => _totalWeight += tileGenFunc.Weight);
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


        public void MoveRight()
        {
            if (_currentTile.Right != null)
                _currentTile = _currentTile.Right;
        }

        public void MoveLeft()
        {
            if (_currentTile.Left != null)
                _currentTile = _currentTile.Left;
        }

        public void MoveDown()
        {
            if (_currentTile.Down != null)
                _currentTile = _currentTile.Down;
        }

        public void MoveUp()
        {
            if (_currentTile.Up != null)
                _currentTile = _currentTile.Up;
        }

        /// <summary>
        /// Event paths have MainPathNodes as their root.
        /// These should always be connected to from the left
        /// if travelling right.
        /// </summary>
        /// <returns></returns>
        private ITile GetEventPath(Direction primaryDirection)
        {
            ITile eventPath;
            IPathFactory tileGenFactory = GetTileGenFactory();
            if (tileGenFactory != null)
            {
                eventPath = tileGenFactory.MakePath(primaryDirection);
            }
            else
            {
                eventPath = new MainPathTile(primaryDirection);
            }

            return eventPath;
        }

        private IPathFactory GetTileGenFactory()
        {
            int random = _rng.Next(_totalWeight);

            IPathFactory tilePathFactory = null;

            for(int f = 0; f < _tileGenFunctions.Count; f++)
            {
                var factory = _tileGenFunctions[f];
                if(random < factory.Weight)
                {
                    tilePathFactory = factory.Item;
                    if(tilePathFactory.GetType().Equals(typeof(ShopPathFactory)))
                        _tileGenFunctions.Remove(factory);
                    break;
                }

                random -= factory.Weight;
            }

            return tilePathFactory;
        }

    }
}
