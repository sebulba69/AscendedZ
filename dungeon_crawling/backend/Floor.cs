using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    public class Floor
    {
        private Random _rng;
        private const int HEIGHT = 10;
        private const int WIDTH = 10;
        private const int UP = 0;
        private const int LEFT = 1;
        private const int RIGHT = 2;

        private bool _completed = false;
        private int _level = -1;
        private int _maxTileCount = 15;

        private int _tileCount = 0;

        /// <summary>
        /// -- PUBLIC FOR SERIALIZATION ONLY, DO NOT ACCESS OUTSIDE THIS CLASS -- 
        /// </summary>
        public int TileCount { get => _tileCount; set => _tileCount = value; }

        /// <summary>
        /// -- PUBLIC FOR SERIALIZATION ONLY, DO NOT ACCESS OUTSIDE THIS CLASS -- 
        /// TEMP -- Make this editable later on for memes.
        /// You should be able to edit this later.
        /// </summary>
        public int MaxTileCount { get => _maxTileCount; set => _maxTileCount = value; }


        /// <summary>
        /// -- PUBLIC FOR SERIALIZATION ONLY, DO NOT ACCESS OUTSIDE THIS CLASS -- 
        /// </summary>
        public bool Completed 
        { 
            get => _completed; 
            set => _completed = value; 
        }

        /// <summary>
        /// -- PUBLIC FOR SERIALIZATION ONLY, DO NOT ACCESS OUTSIDE THIS CLASS -- 
        /// </summary>
        public int Level 
        { 
            get => _level; 
            set => _level = value; 
        }

        public Tile[,] DungeonFloor { get; set; }

        /// <summary>
        /// Player Row Coord
        /// </summary>
        public int R { get; set; }

        /// <summary>
        /// Player Column Coord
        /// </summary>
        public int C { get; set; }

        public Floor()
        {
            _rng = new Random();
            if (DungeonFloor == null)
            {
                DungeonFloor = new Tile[HEIGHT, WIDTH + 1];
            }
        }

        public void StartDungeon(int level)
        {
            if (!_completed || _level < 0)
            {
                _level = level;

                R = HEIGHT - 1;
                C = WIDTH / 2;

                DungeonFloor[R, C] = new Tile(); // place the starting tile

                MakePaths();
            }
        }

        public void MoveUp()
        {
            int r = R - 1;
            if(r >= 0)
            {
                if (DungeonFloor[r, C] != null)
                {
                    R--;
                    Move();
                }
            }
        }

        public void MoveLeft()
        {
            int c = C - 1;
            if(c >= 0)
            {
                if (DungeonFloor[R, c] != null)
                {
                    C--;
                    Move();
                }
            }
        }

        public void MoveRight()
        {
            int c = C + 1;
            if (c <= WIDTH)
            {
                if (DungeonFloor[R, c] != null)
                {
                    C++;
                    Move();
                }
            }
        }

        private void Move()
        {
            if (!DungeonFloor[R, C].PathsGenerated)
                MakePaths();
        }

        public void MoveDown()
        {
            int r = R + 1;
            if(r < HEIGHT)
            {
                R++;
            }
        }

        public bool IsPathUp()
        {
            bool pathUp = false;
            int r = R - 1;
            if (r >= 0)
            {
                pathUp = (DungeonFloor[r, C] != null);
            }
            return pathUp;
        }

        public bool IsPathLeft()
        {
            bool pathLeft = false;
            int c = C - 1;
            if (c >= 0)
            {
                pathLeft = (DungeonFloor[R, c] != null);
            }
            return pathLeft;
        }

        public bool IsPathRight()
        {
            bool pathRight = false;
            int c = C + 1;
            if (c <= WIDTH)
            {
                pathRight = (DungeonFloor[R, c] != null);
            }
            return pathRight;
        }

        public bool IsPathDown()
        {
            bool pathDown = false;
            int r = R + 1;
            if (r < HEIGHT)
            {
                pathDown = (DungeonFloor[r, C] != null);
            }
            return pathDown;
        }

        private void MakePaths()
        {
            List<int> possiblePaths = GetPossibleDirections();

            if(possiblePaths.Count > 0 
                && TileCount < MaxTileCount)
            {
                int pPathCount = possiblePaths.Count;
                int numPaths = _rng.Next(pPathCount);
                HashSet<int> directions = new HashSet<int>();
                for(int i = 0; i < numPaths; i++)
                {
                    int dir = possiblePaths[_rng.Next(pPathCount)];

                    while(directions.Contains(dir))
                        dir = possiblePaths[_rng.Next(pPathCount)];

                    directions.Add(dir);
                    int r = R;
                    int c = C;
                    switch (dir)
                    {
                        case UP:
                            r--;
                            break;
                        case LEFT:
                            c--;
                            break;
                        case RIGHT:
                            c++;
                            break;
                    }

                    if (DungeonFloor[r, c] == null)
                    {
                        DungeonFloor[r, c] = new Tile();
                        TileCount++;
                        DungeonFloor[r, c].IsExit = (TileCount == MaxTileCount);
                    }
                }

                DungeonFloor[R, C].PathsGenerated = true;
            }
        }
        
        private List<int> GetPossibleDirections()
        {
            List<int> paths = new List<int>();

            if(R - 1 >= 0)
                paths.Add(UP);

            if (C - 1 >= 0)
                paths.Add(LEFT);

            if (C + 1 <= WIDTH)
                paths.Add(RIGHT);

            return paths;
        }
    }
}
