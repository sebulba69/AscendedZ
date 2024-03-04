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
    public class Floor
    {
        public Tile Root { get; set; }
        public Tile CurrentTile { get; set; }
        public Floor()
        {
            if (Root != null && CurrentTile == null)
                CurrentTile = Root;
        }

        public void Generate()
        {
            if (Root != null)
                return;

            Root = new Tile() { X = 0, Y = 0};
            
            CurrentTile = Root;
            FloorGenerator floorGenerator = new FloorGenerator(Root);
            floorGenerator.Generate(Root);
            Godot.GD.Print($"Count: {floorGenerator.TileCount}");
        }

        public void ClearNodes()
        {
            if(Root != null)
                Root = null;
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
