using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    /// <summary>
    /// 1 dungeon floor
    /// </summary>
    public class Dungeon
    {
        private DungeonGenerator _dungeonGenerator;
        public Tile Root { get; set; }
        public Tile CurrentTile { get; set; }
        public Dungeon()
        {
            _dungeonGenerator = new DungeonGenerator();

            if (Root != null && CurrentTile == null)
                CurrentTile = Root;
        }

        public void Generate()
        {
            if (Root != null)
                return;

            Root = new Tile() { Layer = 0, Value = 0 };
            
            CurrentTile = Root;

            _dungeonGenerator.Generate(Root);
        }

        public void MoveUpLeft()
        {
            CurrentTile = CurrentTile.Left;
        }

        public void MoveUpRight()
        {
            CurrentTile = CurrentTile.Right;
        }

        public void MoveDownRight()
        {
            CurrentTile = CurrentTile.BottomRight;
        }

        public void MoveDownLeft()
        {
            CurrentTile = CurrentTile.BottomLeft;
        }

        public bool CanMoveLeft()
        {
            return CurrentTile.Left != null;
        }

        public bool CanMoveRight()
        {
            return CurrentTile.Right != null;
        }

        public bool CanMoveDownRight()
        {
            return CurrentTile.BottomRight != null;
        }

        public bool CanMoveDownLeft()
        {
            return CurrentTile.BottomLeft != null;
        }
    }
}
