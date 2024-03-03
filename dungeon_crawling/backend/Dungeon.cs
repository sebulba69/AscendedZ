using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    /// <summary>
    /// 1 dungeon floor
    /// </summary>
    public class Dungeon
    {
        private int _maxTileCount = 12;
        private int _tileCount = 1;
        public int MaxTileCount { get => _maxTileCount; set => _maxTileCount = value; }
        public int TileCount { get => _tileCount; set => _tileCount = value; }
        public Tile Root { get; set; }
        public Tile Exit { get; set; }
        public Tile CurrentTile { get; set; }
        public Dungeon()
        {
            if(Root == null)
            {
                Root = new Tile()
                {
                    Value = 0,
                    Occupied = true
                };

                CurrentTile = Root;
            }
        }

        public void MoveRight()
        {
            if(CurrentTile.Right == null)
            {
                if(TileCount < MaxTileCount)
                {
                    int value = CurrentTile.Value + 1;
                    CurrentTile.Occupied = false;

                    CurrentTile.Right = new Tile()
                    {
                        Value = value,
                        Occupied = true
                    };

                    IncrementTileCount();
                }
            }
            else
            {
                CurrentTile.Occupied = false;
                CurrentTile = CurrentTile.Right;
            }
        }

        private void IncrementTileCount()
        {
            TileCount++;
            if (TileCount == MaxTileCount)
                Exit = CurrentTile;
        }
        


    }
}
