using AscendedZ.dungeon_crawling.backend.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling
{
    public enum PathType
    {
        Heal, Item, Shop, Blacksmith
    }

    public class PathFactory
    {
        private Random _rng;
        public PathFactory(Random rng)
        {
            _rng = rng;
        }

        public ITile MakePath(Direction direction, PathType path)
        {
            ITile startOfTile = new MainPathTile(direction);
            ITile specialTile;

            switch (path)
            {
                case PathType.Heal:
                    specialTile = new HealTile();
                    break;
                case PathType.Blacksmith:
                    specialTile = new BlacksmithTile();
                    break;
                case PathType.Item:
                    specialTile = new ItemTile();
                    break;
                case PathType.Shop:
                    specialTile = new ShopTile();
                    break;
                default:
                    throw new NotImplementedException();
            }

            MakePathFromDirection(startOfTile, specialTile, direction);

            return startOfTile;
        }

        private void MakePathFromDirection(ITile startOfPath, ITile specialTile, Direction direction)
        {
            if (direction == Direction.Left || direction == Direction.Right)
            {
                MakeUpDownPath(startOfPath, specialTile);
            }
            else
            {
                MakeLeftRightPath(startOfPath, specialTile);
            }
        }

        /// <summary>
        /// Where the primary direction is left or right
        /// </summary>
        /// <param name="startOfPath"></param>
        /// <param name="encounterTile"></param>
        /// <param name="specialTile"></param>
        /// <param name="healTile"></param>
        /// <returns></returns>
        private void MakeLeftRightPath(ITile startOfPath, ITile specialTile)
        {
            // left or right
            if (_rng.Next(0, 1) == 1)
            {
                startOfPath.Left = specialTile;
                specialTile.Right = startOfPath;
            }
            else
            {
                startOfPath.Right = specialTile;
                specialTile.Left = startOfPath;
            }
        }

        /// <summary>
        /// Where the primary direction is up or down
        /// </summary>
        /// <param name="startOfPath"></param>
        /// <param name="encounterTile"></param>
        /// <param name="specialTile"></param>
        /// <param name="healTile"></param>
        /// <returns></returns>
        private void MakeUpDownPath(ITile startOfPath, ITile specialTile)
        {
            // up or down
            if (_rng.Next(0, 1) == 1)
            {
                startOfPath.Up = specialTile;
                specialTile.Down = startOfPath;
            }
            else
            {
                startOfPath.Down = specialTile;
                specialTile.Up = startOfPath;
            }
        }
    }
}
