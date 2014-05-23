using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    public static class Start
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SimpleGraphicsForm simpleGraphics = new SimpleGraphicsForm();
            GameLogic gameLogic = new GameLogic(simpleGraphics);
            simpleGraphics.GameSquareFilled += gameLogic.GameSquareFilled;
            simpleGraphics.NextSquareFilled += gameLogic.NextSquareFilled;
            simpleGraphics.KeyDownEvent += gameLogic.KeyDownEvent;
            gameLogic.StartNewGame();
            Application.Run(simpleGraphics);
        }
    }
}
