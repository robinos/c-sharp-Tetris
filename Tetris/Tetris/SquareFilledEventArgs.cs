using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public class SquareFilledEventArgs : EventArgs
    {
        public int x { get; set; }
        public int y { get; set; }
        public int state { get; set; }
    }
}
