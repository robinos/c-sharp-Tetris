using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    interface ISquareObserver
    {
        void GameSquareFilled(SquareFilledEventArgs gameSquareFilledArgs);
        void NextSquareFilled(SquareFilledEventArgs nextSquareFilledArgs);
        void KeyDownEvent(object sender, System.Windows.Forms.KeyEventArgs e);
    }
}
