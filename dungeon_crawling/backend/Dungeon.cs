using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.dungeon_crawling.backend.Tiles;

namespace AscendedZ.dungeon_crawling.backend
{
    public class Dungeon
    {
        private Random _rng;
        private bool _doorFlagSet = false;

        private List<Func<ITile, ITile>> _tileGenFunctions;

        private ITile _current;
        private ITile _root;
        private int _level;

        public ITile Root { get => _root; }

        public Dungeon(int level)
        {
            _rng = new Random();
            _level = level;

            _tileGenFunctions = new List<Func<ITile, ITile>>() 
            {
                MakeItemPath,
                MakeHealPath,
                MakeShopPath,
                MakeDoorPath
            };
        }

        public void Start()
        {
            _root = new NormalTile();

            ITile right = new NormalTile();
            
            Root.Right = right;
            
            right.Left = Root;
            
            _current = Root;
        }

        public void GenerateTile()
        {
            var tileGenFunction = _tileGenFunctions[_rng.Next(_tileGenFunctions.Count)];
            ITile eventPath = tileGenFunction.Invoke(_current);
        }

        private ITile MakeItemPath(ITile startOfPath)
        {
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
            if(_rng.Next(0,1) == 1)
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

        private ITile MakeHealPath(ITile startOfPath)
        {
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

        private ITile MakeDoorPath(ITile startOfPath)
        {
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

        private ITile MakeShopPath(ITile startOfPath)
        {
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
    }
}
