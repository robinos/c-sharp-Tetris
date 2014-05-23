using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public class GameLogic : ISquareObserver
    {
        private SimpleGraphicsForm simpleGraphics;
        private int currentPiece;
        private int nextPiece;
        private int currentRotation;
        private int currentX;
        private int currentY;
        //In the context of filledGameSquares and filledNextSquares
        //0 means the square is empty, 1 means the square is filled with the active piece
        //and 2 means the square is filled with an inactive (already placed) piece
        private int[,] filledGameSquares;
        private int[,] filledNextSquares;

        /*
         * 
         */
        public GameLogic(SimpleGraphicsForm simpleGraphics)
        {
            this.simpleGraphics = simpleGraphics;

            filledGameSquares = new int[11, 21];
            filledNextSquares = new int[7, 7];
        }

        public void StartNewGame()
        {
            currentRotation = 0;
            currentX = 4;
            currentY = 0;

            //initially empty game board
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 21; y++)
                {
                    filledGameSquares[x, y] = 0;
                }
            }

            //initially empty next square
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    filledNextSquares[x, y] = 0;
                }
            }

            nextPiece = GetRandomPiece();
            DrawNewGamePiece();
        }

        /*
         * Listens for which game squares are filled
         */
        public void GameSquareFilled(SquareFilledEventArgs args)
        {
            filledGameSquares[args.x/20, args.y/20] = args.state;

            for (int y = 1; y < 20; y++)
            {
                if (LineCompleted(y))
                {
                    RemoveLine(y);
                }
            }
        }

        /*
         * Listens for which next-piece squares are filled
         */
        public void NextSquareFilled(SquareFilledEventArgs args)
        {
            filledNextSquares[args.x/20, args.y/20] = args.state;
        }

        private bool LineCompleted(int y)
        {
            bool complete = true;

            for (int x = 0; x < 10; x++)
            {
                if (!(filledGameSquares[x, y] == 2))
                {
                    complete = false;
                }
            }

            return complete;
        }

        private void RemoveLine(int y)
        {
            for (int x = 0; x < 10; x++)
            {
                simpleGraphics.FillGameSquare(x,y,Color.White);
            }

            if (y != 0)
            {
                for (int i = y - 1; i >= 0; i--)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if ((filledGameSquares[x, i] == 2))
                        {
                            simpleGraphics.FillGameSquare(x, i+1, Color.Blue);
                            simpleGraphics.FillGameSquare(x, i, Color.White);
                        }
                    }
                }
            }
        }

        /*
         * Listens for which key was pushed down
         */
        public void KeyDownEvent(object sender, KeyEventArgs e)
        {
            if (currentY >= 0 && currentY < 19 && currentX >= 0 && currentX < 9)
            {
                DrawCurrentPiece(currentPiece, currentX, currentY, Color.White, true, currentRotation);
                currentY += 1;
                if (DetectVerticalCollision(currentX, currentY))
                {
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.Blue, true, currentRotation);
                    if (currentY == 0)
                    {
                        simpleGraphics.displayMessage("Game over man...game over!", "Game Over");
                    }
                    else
                    {
                        DrawNewGamePiece();
                    }
                }
                else
                {
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.Black, true, currentRotation);
                }
            }
            else
            {
                DrawCurrentPiece(currentPiece, currentX, currentY, Color.Blue, true, currentRotation);
                if (currentY == 0)
                {
                    simpleGraphics.displayMessage("Game over man...game over!", "Game Over");
                }
                else
                {
                    DrawNewGamePiece();
                }
            }

            // Determine which key was pressed 
            if (e.KeyCode == Keys.Left)
            {
                if (currentX >= 0)
                {
                    if (!DetectLeftCollision(currentX, currentY))
                    {
                        DrawCurrentPiece(currentPiece, currentX, currentY, Color.White, true, currentRotation);
                        currentX -= 1;
                        DrawCurrentPiece(currentPiece, currentX, currentY, Color.Black, true, currentRotation);
                    }
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (currentX < 9)
                {
                    if (!DetectLeftCollision(currentX + 1, currentY))
                    {
                        DrawCurrentPiece(currentPiece, currentX, currentY, Color.White, true, currentRotation);
                        currentX += 1;
                        DrawCurrentPiece(currentPiece, currentX, currentY, Color.Black, true, currentRotation);
                    }
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                //simpleGraphics.displayMessage("Rotate left!", "Rotate counterclockwise");
                if (currentPiece == 2 || currentPiece == 3 || currentPiece == 4)
                {
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.White, true, currentRotation);
                    if (currentRotation == 1) { currentRotation = 0; }
                    else
                    {
                        if(currentY > 0) currentRotation = 1;
                    }
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.Black, true, currentRotation);
                }
                else if (currentPiece == 5 || currentPiece == 6 || currentPiece == 7)
                {
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.White, true, currentRotation);
                    if (currentRotation == 0)
                    {
                        if (currentY > 0) currentRotation = 3;
                    }
                    else if (currentRotation == 3)
                    {
                        if(currentY > 0) currentRotation = 2;
                    }
                    else if (currentRotation == 2)
                    {
                        if(currentY > 0) currentRotation = 1;
                    }
                    else //current Rotation 1
                    {
                        currentRotation = 0;
                    }
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.Black, true, currentRotation);
                }
                //else //(currentPiece == 1)
                //{
                    //only one rotation, do nothing
                //}
            }
            else if (e.KeyCode == Keys.Down)
            {
                //simpleGraphics.displayMessage("Rotate left!", "Rotate counterclockwise");
                if (currentPiece == 2 || currentPiece == 3 || currentPiece == 4)
                {
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.White, true, currentRotation);
                    if (currentRotation == 1) { currentRotation = 0; }
                    else
                    {
                        if (currentY > 0) currentRotation = 1;
                    }
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.Black, true, currentRotation);
                }
                else if (currentPiece == 5 || currentPiece == 6 || currentPiece == 7)
                {
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.White, true, currentRotation);
                    if (currentRotation == 0)
                    {
                        if (currentY > 0) currentRotation = 1;
                    }
                    else if (currentRotation == 1)
                    {
                        if (currentY > 0) currentRotation = 2;
                    }
                    else if (currentRotation == 2)
                    {
                        if (currentY > 0) currentRotation = 3;
                    }
                    else //current Rotation 3
                    {
                        currentRotation = 0;
                    }
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.Black, true, currentRotation);
                }
                //else //(currentPiece == 1)
                //{
                //only one rotation, do nothing
                //}
            }
            else if (e.KeyCode == Keys.Space)
            {
                int y = currentY;

                if (y >= 19)
                {
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.White, true, currentRotation);
                    DrawCurrentPiece(currentPiece, currentX, currentY, Color.Blue, true, currentRotation);
                }
                else
                {
                    while (y < 19)
                    {
                        if (DetectVerticalCollision(currentX, y))
                        {
                            DrawCurrentPiece(currentPiece, currentX, y, Color.White, true, currentRotation);
                            DrawCurrentPiece(currentPiece, currentX, y, Color.Blue, true, currentRotation);
                            break;
                        }
                        else
                        {
                            DrawCurrentPiece(currentPiece, currentX, y, Color.White, true, currentRotation);
                            DrawCurrentPiece(currentPiece, currentX, y+1, Color.Black, true, currentRotation);
                        }
                        y++;
                    }
                }
                if (y == 0)
                {
                    simpleGraphics.displayMessage("Game over man...game over!", "Game Over");
                }
                else
                {
                    DrawNewGamePiece();
                }
            }
        }

        private int GetRandomPiece()
        {
            return(new Random().Next(1,8));
        }

        private void DrawNewGamePiece()
        {
            currentPiece = nextPiece;
            currentX = 4;
            currentY = 0;
            currentRotation = 0;

            DrawCurrentPiece(currentPiece, 4, 0, Color.Black, true, 0);

            DrawCurrentPiece(nextPiece, 2, 1, Color.White, false, 0);
            nextPiece = GetRandomPiece();
            DrawNextPiece();
        }

        private void DrawNextPiece()
        {
            DrawCurrentPiece(nextPiece, 2, 1, Color.Black, false, 0);
        }

        private bool DetectVerticalCollision(int x, int y)
        {
            bool collision = false;

            switch (currentPiece)
            {
                case 1:
                    if(VerticalCollisionTest(x, y + 2)) collision = true;
                    if(VerticalCollisionTest(x + 1, y + 2)) collision = true;
                    break;
                case 2:
                    if (currentRotation == 1)
                    {
                        if(VerticalCollisionTest(x - 1, y + 1)) collision = true;
                        if(VerticalCollisionTest(x, y + 1)) collision = true;
                        if(VerticalCollisionTest(x + 1, y + 1)) collision = true;
                        if (VerticalCollisionTest(x + 2, y + 1)) collision = true;
                    }
                    else
                    {
                        if (VerticalCollisionTest(x, y + 1)) collision = true;
                    }
                    break;
                case 3:
                    if (currentRotation == 1)
                    {
                        if (VerticalCollisionTest(x - 1, y + 1)) collision = true;
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                    }
                    else
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 2)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    break;
                case 4:
                    if (currentRotation == 1)
                    {
                        if(VerticalCollisionTest(x, y + 2)) collision = true;
                        if(VerticalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 1)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 2)) collision = true;
                    }
                    break;
                case 5:
                    if (currentRotation == 1)
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 2)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (VerticalCollisionTest(x, y + 1)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 1)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x - 1, y)) collision = true;
                    }
                    else
                    {
                        if (VerticalCollisionTest(x, y + 1)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 2)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    break;
                case 6:
                    if (currentRotation == 1)
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x + 1, y)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (VerticalCollisionTest(x, y + 1)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 1)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 2)) collision = true;
                    }
                    else
                    {
                        if (VerticalCollisionTest(x, y + 1)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 1)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 2)) collision = true;
                    }
                    break;
                case 7:
                    if (currentRotation == 1)
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (VerticalCollisionTest(x, y + 1)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 1)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (VerticalCollisionTest(x, y + 2)) collision = true;
                        if (VerticalCollisionTest(x - 1, y + 1)) collision = true;
                        if (VerticalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    break;
                default:
                    break;
            }

            return collision;
        }

        private bool DetectLeftCollision(int x, int y)
        {
            bool collision = false;

            switch (currentPiece)
            {
                case 1:
                    if (HorizontalCollisionTest(x - 1, y)) collision = true;
                    if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                    break;
                case 2:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x - 1, y)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 2)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                    }
                    break;
                case 3:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                        if (HorizontalCollisionTest(x - 2, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x - 1, y)) collision = true;
                        if (HorizontalCollisionTest(x - 2, y + 1)) collision = true;
                    }
                    break;
                case 4:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x - 1, y)) collision = true;
                        if (HorizontalCollisionTest(x, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                    }
                    break;
                case 5:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x - 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                        if (HorizontalCollisionTest(x, y - 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (HorizontalCollisionTest(x - 1, y)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                        if (HorizontalCollisionTest(x - 2, y - 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                        if (HorizontalCollisionTest(x - 2, y + 1)) collision = true;
                    }
                    break;
                case 6:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x - 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                        if (HorizontalCollisionTest(x - 2, y - 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (HorizontalCollisionTest(x - 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y)) collision = true;
                        if (HorizontalCollisionTest(x - 2, y + 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                        if (HorizontalCollisionTest(x, y + 1)) collision = true;
                    }
                    break;
                case 7:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x - 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y - 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (HorizontalCollisionTest(x - 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x - 2, y)) collision = true;
                        if (HorizontalCollisionTest(x - 1, y + 1)) collision = true;
                    }
                    break;
                default:
                    break;
            }

            return collision;
        }

        private bool DetectRightCollision(int x, int y)
        {
            bool collision = false;

            switch (currentPiece)
            {
                case 1:
                    if (HorizontalCollisionTest(x + 2, y)) collision = true;
                    if (HorizontalCollisionTest(x + 2, y + 1)) collision = true;
                    break;
                case 2:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x + 1, y)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 2)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x + 3, y)) collision = true;
                    }
                    break;
                case 3:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x + 1, y)) collision = true;
                        if (HorizontalCollisionTest(x, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    break;
                case 4:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                        if (HorizontalCollisionTest(x + 2, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x + 1, y)) collision = true;
                        if (HorizontalCollisionTest(x + 2, y + 1)) collision = true;
                    }
                    break;
                case 5:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x + 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y)) collision = true;
                        if (HorizontalCollisionTest(x + 2, y + 1)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                        if (HorizontalCollisionTest(x + 2, y - 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (HorizontalCollisionTest(x + 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                        if (HorizontalCollisionTest(x, y + 1)) collision = true;
                    }
                    break;
                case 6:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x + 1, y)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                        if (HorizontalCollisionTest(x + 2, y - 1)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                        if (HorizontalCollisionTest(x, y - 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (HorizontalCollisionTest(x + 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                        if (HorizontalCollisionTest(x + 2, y + 1)) collision = true;
                    }
                    break;
                case 7:
                    if (currentRotation == 1)
                    {
                        if (HorizontalCollisionTest(x + 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                    }
                    else if (currentRotation == 2)
                    {
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y - 1)) collision = true;
                    }
                    else if (currentRotation == 3)
                    {
                        if (HorizontalCollisionTest(x + 1, y - 1)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    else
                    {
                        if (HorizontalCollisionTest(x + 2, y)) collision = true;
                        if (HorizontalCollisionTest(x + 1, y + 1)) collision = true;
                    }
                    break;
                default:
                    break;
            }

            return collision;
        }

        private bool VerticalCollisionTest(int x, int y)
        {
            if (y >= 0 && y < 20)
            {
                if (x < 0) x = 0;
                if (x > 9) x = 9;

                if (filledGameSquares[x, y] >= 2)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private bool HorizontalCollisionTest(int x, int y)
        {
            if (x >= 0 && x < 11)
            {
                if (filledGameSquares[x, y] >= 2)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private void DrawCurrentPiece(int piece, int x, int y, Color color, bool gamePanel, int rotation)
        {
            switch (piece)
            {
                case 1:
                    DrawO(x, y, color, gamePanel);
                    break;
                case 2:
                    DrawI(x, y, color, gamePanel, rotation);
                    break;
                case 3:
                    DrawS(x, y, color, gamePanel, rotation);
                    break;
                case 4:
                    DrawZ(x, y, color, gamePanel, rotation);
                    break;
                case 5:
                    DrawL(x, y, color, gamePanel, rotation);
                    break;
                case 6:
                    DrawJ(x, y, color, gamePanel, rotation);
                    break;
                case 7:
                    DrawT(x, y, color, gamePanel, rotation);
                    break;
                default: //0
                    break;
            }
        }

        private void DrawO(int x, int y, Color color, bool gamePanel)
        {
            if (gamePanel == true)
            {
                simpleGraphics.FillGameSquare(x, y, color);
                simpleGraphics.FillGameSquare(x, y + 1, color);
                simpleGraphics.FillGameSquare(x + 1, y, color);
                simpleGraphics.FillGameSquare(x + 1, y + 1, color);
            }
            else
            {
                simpleGraphics.FillNextSquare(x, y, color);
                simpleGraphics.FillNextSquare(x, y + 1, color);
                simpleGraphics.FillNextSquare(x + 1, y, color);
                simpleGraphics.FillNextSquare(x + 1, y + 1, color);
            }

        }

        private void DrawI(int x, int y, Color color, bool gamePanel, int rotation)
        {
            if (gamePanel == true)
            {
                if (rotation == 1)
                {
                    simpleGraphics.FillGameSquare(x, y, color);
                    simpleGraphics.FillGameSquare(x, y - 1, color);
                    simpleGraphics.FillGameSquare(x, y + 1, color);
                    simpleGraphics.FillGameSquare(x, y + 2, color);
                }
                else
                {
                    simpleGraphics.FillGameSquare(x, y, color);
                    simpleGraphics.FillGameSquare(x - 1, y, color);
                    simpleGraphics.FillGameSquare(x + 1, y, color);
                    simpleGraphics.FillGameSquare(x + 2, y, color);
                }
            }
            else
            {
                simpleGraphics.FillNextSquare(x, y, color);
                simpleGraphics.FillNextSquare(x - 1, y, color);
                simpleGraphics.FillNextSquare(x + 1, y, color);
                simpleGraphics.FillNextSquare(x + 2, y, color);
            }
        }

        private void DrawS(int x, int y, Color color, bool gamePanel, int rotation)
        {
            if (gamePanel == true)
            {
                if (rotation == 1)
                {
                    simpleGraphics.FillGameSquare(x, y, color);
                    simpleGraphics.FillGameSquare(x - 1, y, color);
                    simpleGraphics.FillGameSquare(x - 1, y - 1, color);
                    simpleGraphics.FillGameSquare(x, y + 1, color);
                }
                else
                {
                    simpleGraphics.FillGameSquare(x, y, color);
                    simpleGraphics.FillGameSquare(x + 1, y, color);
                    simpleGraphics.FillGameSquare(x, y + 1, color);
                    simpleGraphics.FillGameSquare(x - 1, y + 1, color);
                }
            }
            else
            {
                simpleGraphics.FillNextSquare(x, y, color);
                simpleGraphics.FillNextSquare(x + 1, y, color);
                simpleGraphics.FillNextSquare(x, y + 1, color);
                simpleGraphics.FillNextSquare(x - 1, y + 1, color);
            }
        }

        private void DrawZ(int x, int y, Color color, bool gamePanel, int rotation)
        {
            if (gamePanel == true)
            {
                if (rotation == 1)
                {
                    simpleGraphics.FillGameSquare(x, y, color);
                    simpleGraphics.FillGameSquare(x + 1, y, color);
                    simpleGraphics.FillGameSquare(x + 1, y - 1, color);
                    simpleGraphics.FillGameSquare(x, y + 1, color);
                }
                else
                {
                    simpleGraphics.FillGameSquare(x, y, color);
                    simpleGraphics.FillGameSquare(x - 1, y, color);
                    simpleGraphics.FillGameSquare(x, y + 1, color);
                    simpleGraphics.FillGameSquare(x + 1, y + 1, color);
                }
            }
            else
            {
                simpleGraphics.FillNextSquare(x, y, color);
                simpleGraphics.FillNextSquare(x - 1, y, color);
                simpleGraphics.FillNextSquare(x, y + 1, color);
                simpleGraphics.FillNextSquare(x + 1, y + 1, color);
            }
        }

        private void DrawL(int x, int y, Color color, bool gamePanel, int rotation)
        {
            {
                if (gamePanel == true)
                {
                    if (rotation == 1)
                    {
                        simpleGraphics.FillGameSquare(x, y - 1, color);
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x, y + 1, color);
                        simpleGraphics.FillGameSquare(x + 1, y + 1, color);
                    }
                    else if (rotation == 2)
                    {
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x + 1, y, color);
                        simpleGraphics.FillGameSquare(x - 1, y, color);
                        simpleGraphics.FillGameSquare(x + 1, y - 1, color);
                    }
                    else if (rotation == 3)
                    {
                        simpleGraphics.FillGameSquare(x, y - 1, color);
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x, y + 1, color);
                        simpleGraphics.FillGameSquare(x - 1, y - 1, color);
                    }
                    else
                    {
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x + 1, y, color);
                        simpleGraphics.FillGameSquare(x - 1, y, color);
                        simpleGraphics.FillGameSquare(x - 1, y + 1, color);
                    }
                }
                else
                {
                    simpleGraphics.FillNextSquare(x, y, color);
                    simpleGraphics.FillNextSquare(x + 1, y, color);
                    simpleGraphics.FillNextSquare(x - 1, y, color);
                    simpleGraphics.FillNextSquare(x - 1, y + 1, color);
                }
            }
        }

        private void DrawJ(int x, int y, Color color, bool gamePanel, int rotation)
        {
            {
                if (gamePanel == true)
                {
                    if (rotation == 1)
                    {
                        simpleGraphics.FillGameSquare(x, y - 1, color);
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x, y + 1, color);
                        simpleGraphics.FillGameSquare(x + 1, y - 1, color);
                    }
                    else if (rotation == 2)
                    {
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x + 1, y, color);
                        simpleGraphics.FillGameSquare(x - 1, y, color);
                        simpleGraphics.FillGameSquare(x - 1, y - 1, color);
                    }
                    else if (rotation == 3)
                    {
                        simpleGraphics.FillGameSquare(x, y - 1, color);
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x, y + 1, color);
                        simpleGraphics.FillGameSquare(x - 1, y + 1, color);
                    }
                    else
                    {
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x + 1, y, color);
                        simpleGraphics.FillGameSquare(x - 1, y, color);
                        simpleGraphics.FillGameSquare(x + 1, y + 1, color);
                    }
                }
                else
                {
                    simpleGraphics.FillNextSquare(x, y, color);
                    simpleGraphics.FillNextSquare(x + 1, y, color);
                    simpleGraphics.FillNextSquare(x - 1, y, color);
                    simpleGraphics.FillNextSquare(x + 1, y + 1, color);
                }
            }
        }

        private void DrawT(int x, int y, Color color, bool gamePanel, int rotation)
        {
            {
                if (gamePanel == true)
                {
                    if (rotation == 1)
                    {
                        simpleGraphics.FillGameSquare(x, y - 1, color);
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x, y + 1, color);
                        simpleGraphics.FillGameSquare(x + 1, y, color);
                    }
                    else if (rotation == 2)
                    {
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x + 1, y, color);
                        simpleGraphics.FillGameSquare(x - 1, y, color);
                        simpleGraphics.FillGameSquare(x, y - 1, color);
                    }
                    else if (rotation == 3)
                    {
                        simpleGraphics.FillGameSquare(x, y - 1, color);
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x, y + 1, color);
                        simpleGraphics.FillGameSquare(x - 1, y, color);
                    }
                    else
                    {
                        simpleGraphics.FillGameSquare(x, y, color);
                        simpleGraphics.FillGameSquare(x + 1, y, color);
                        simpleGraphics.FillGameSquare(x - 1, y, color);
                        simpleGraphics.FillGameSquare(x, y + 1, color);
                    }
                }
                else
                {
                    simpleGraphics.FillNextSquare(x, y, color);
                    simpleGraphics.FillNextSquare(x + 1, y, color);
                    simpleGraphics.FillNextSquare(x - 1, y, color);
                    simpleGraphics.FillNextSquare(x, y + 1, color);
                }
            }
        }

    }
}
