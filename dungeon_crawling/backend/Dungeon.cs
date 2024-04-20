using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.dungeon_crawling.backend.PathMakers;
using AscendedZ.dungeon_crawling.backend.Tiles;
using Godot;

namespace AscendedZ.dungeon_crawling.backend
{
    public class WeightedItem<E>
    {
        private E _item;
        private int _weight;

        public E Item { get => _item; }
        public int Weight { get => _weight; }

        public WeightedItem(E item, int weight)
        {
            _item = item;
            _weight = weight;  
        }
    }

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

            IPathFactory itemPathFactory = new ItemPathFactory(_rng);
            IPathFactory healPathFactory = new HealPathFactory(_rng);
            IPathFactory shopPathFactory = new ShopPathFactory(_rng);

            _tileGenFunctions = new List<WeightedItem<IPathFactory>>() 
            {
                new WeightedItem<IPathFactory>(itemPathFactory, 35),
                new WeightedItem<IPathFactory>(healPathFactory, 20),
                new WeightedItem<IPathFactory>(shopPathFactory, 15),
            };

            _tileGenFunctions.ForEach(tileGenFunc => _totalWeight += tileGenFunc.Weight);
        }

        public void Start()
        {
            List<ITile> mainPathTiles = new List<ITile>();

            _currentTile = new MainPathTile();
            ITile tile = _currentTile;
            bool mainTile = false;
            bool generateEvent = true;
            int eventCount = 0;

            while(eventCount < _eventCount)
            {
                ITile right;
                if (mainTile)
                {
                    right = new MainPathTile();
                }
                else
                {
                    ITile eventTile;

                    if (generateEvent)
                    {
                        eventTile = GetEventPath();
                        eventCount++;
                    }
                    else
                    {
                        eventTile = new MainEncounterTile();
                    }

                    right = eventTile;
                    generateEvent = !generateEvent;
                }

                tile.Right = right;
                right.Left = tile;
                tile = right;

                mainTile = !mainTile;
            }

            ITile last = new MainPathTile() { IsExit = true };
            tile.Right = last;
            last.Left = tile;
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
        private ITile GetEventPath()
        {
            ITile eventPath;
            IPathFactory tileGenFactory = GetTileGenFactory();
            if (tileGenFactory != null)
            {
                eventPath = tileGenFactory.MakePath();
            }
            else
            {
                eventPath = new MainPathTile();
            }

            return eventPath;
        }

        private IPathFactory GetTileGenFactory()
        {
            int random = _rng.Next(_totalWeight);

            IPathFactory tilePathFactory = null;

            foreach(var factory in _tileGenFunctions)
            {
                if(random < factory.Weight)
                {
                    tilePathFactory = factory.Item;
                    break;
                }

                random -= factory.Weight;
            }

            return tilePathFactory;
        }

    }
}
