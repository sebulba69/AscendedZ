using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private const int MAX_TILES = 20;
        private int _totalWeight = 0;
        private int _level;
        private bool _doorFlagSet = false;

        private List<WeightedItem<Func<ITile>>> _tileGenFunctions;
        private ITile _currentTile;

        public ITile CurrentTile { get => _currentTile; }

        public Dungeon(int level)
        {
            _rng = new Random();
            _level = level;

            _tileGenFunctions = new List<WeightedItem<Func<ITile>>>() 
            {
                new WeightedItem<Func<ITile>>(MakeItemPath, 35),
                new WeightedItem<Func<ITile>>(MakeHealPath, 20),
                new WeightedItem<Func<ITile>>(MakeShopPath, 15),
                new WeightedItem<Func<ITile>>(MakeDoorPath, 35)
            };

            _tileGenFunctions.ForEach(tileGenFunc => _totalWeight += tileGenFunc.Weight);
        }

        public void Start()
        {
            List<ITile> mainPathTiles = new List<ITile>();
            bool generateEvent = (_rng.Next(2) % 2 == 0);

            List<int> eventIndexes = new List<int>();

            _currentTile = new MainPathTile();
            ITile tile = _currentTile;
            for (int t = 0; t < MAX_TILES; t++)
            {
                ITile right;
                if (t % 2 == 0)
                {
                    right = new MainPathTile();
                }
                else
                {
                    ITile eventTile;

                    if (!generateEvent)
                    {
                        eventTile = new MainEncounterTile();
                    }
                    else
                    {
                        eventTile = GetEventPath();
                        eventIndexes.Add(t);
                    }

                    right = eventTile;
                    generateEvent = !generateEvent;
                }

                tile.Right = right;
                right.Left = tile;
                tile = right;
            }
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

        #region Tile Gen Functions
        /// <summary>
        /// Event paths have MainPathNodes as their root.
        /// These should always be connected to from the left
        /// if travelling right.
        /// </summary>
        /// <returns></returns>
        private ITile GetEventPath()
        {
            ITile eventPath;
            if (!_doorFlagSet)
            {
                Func<ITile> tileGenFunction = GetTileGenFunction();
                if (tileGenFunction != null)
                {
                    eventPath = tileGenFunction.Invoke();
                }
                else
                {
                    eventPath = new MainPathTile();
                }
            }
            else
            {
                eventPath = MakeItemPath();
            }

            return eventPath;
        }

        private Func<ITile> GetTileGenFunction()
        {
            int random = _rng.Next(_totalWeight);
            
            Func<ITile> tileGenFunc = null;

            foreach(var func in _tileGenFunctions)
            {
                if(random < func.Weight)
                {
                    tileGenFunc = func.Item;
                    break;
                }

                random -= func.Weight;
            }

            return tileGenFunc;
        }

        private ITile MakeItemPath()
        {
            ITile startOfPath = new MainPathTile();
            ITile encounterTile = new EncounterTile();
            ITile itemTile = new ItemTile();

            if (_doorFlagSet)
            {
                _doorFlagSet = false;
                // itemTile set item to the key
            }

            // left or right
            if(_rng.Next(0, 1) == 1)
            {
                encounterTile.Left = itemTile;
                itemTile.Right = encounterTile;
            }
            else
            {
                encounterTile.Right = itemTile;
                itemTile.Left = encounterTile;
            }

            // down
            if (_rng.Next(0, 1) == 1)
            {
                startOfPath.Down = encounterTile;
                encounterTile.Up = startOfPath;
            }
            else
            {
                startOfPath.Up = encounterTile;
                encounterTile.Down = startOfPath;
            }
            return startOfPath;
        }

        private ITile MakeHealPath()
        {
            ITile startOfPath = new MainPathTile();
            ITile encounterTile = new EncounterTile();
            ITile itemTile = new ItemTile();
            ITile healTile = new HealTile();

            encounterTile.Left = itemTile;
            encounterTile.Right = healTile;
            itemTile.Right = encounterTile;
            healTile.Left = encounterTile;

            if (_rng.Next(0, 1) == 1)
            {
                startOfPath.Up = encounterTile;
                encounterTile.Down = startOfPath;
            }
            else
            {
                startOfPath.Down = encounterTile;
                encounterTile.Up = startOfPath;
            }

            return startOfPath;
        }

        private ITile MakeDoorPath()
        {
            ITile startOfPath = new MainPathTile();
            ITile itemTile = new ItemTile();
            ITile doorTile = new DoorTile();

            if (_rng.Next(0, 1) == 1)
            {
                doorTile.Up = itemTile;
                itemTile.Down = doorTile;

                startOfPath.Up = doorTile;
                doorTile.Down = startOfPath;
            }
            else
            {
                doorTile.Down = itemTile;
                itemTile.Up = doorTile;

                startOfPath.Down = doorTile;
                doorTile.Up = startOfPath;
            }

            return startOfPath;
        }

        private ITile MakeShopPath()
        {
            ITile startOfPath = new MainPathTile();
            ITile shopTile = new ShopTile();
            
            if (_rng.Next(0, 1) == 1)
            {
                startOfPath.Up = shopTile;
                shopTile.Down = startOfPath;
            }
            else
            {
                startOfPath.Down = shopTile;
                shopTile.Up = startOfPath;
            }

            return startOfPath;
        }
        #endregion

    }
}
