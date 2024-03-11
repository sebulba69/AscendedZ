using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    public class Tile
    {
        private bool _isExit = false;
        private bool _pathsGenerated = false;
        public bool IsExit { get => _isExit; set => _isExit = value; }
        public bool PathsGenerated { get => _pathsGenerated; set => _pathsGenerated = value; }
    }
}
