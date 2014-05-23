using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris
{
    public partial class SimpleGraphicsForm : Form
    {
        private Bitmap GameImage;
        private Bitmap NextImage;
        private Graphics GameGraphics;
        private Graphics NextGraphics;

        public delegate void GameFillHandler(SquareFilledEventArgs gameSquareFilled);
        public delegate void NextFillHandler(SquareFilledEventArgs nextSquareFilled);
        public delegate void OnKeyDownHandler(object sender, KeyEventArgs e);
        public event GameFillHandler GameSquareFilled = null;
        public event NextFillHandler NextSquareFilled = null;
        public event OnKeyDownHandler KeyDownEvent = null;

        /*
         * Constructor - initialises the game board 
         */
        public SimpleGraphicsForm()
        {
            InitializeComponent();

            //At this point, GameImage and NextImage are null. This will assign new values, using the size of each panel
            GameImage = new Bitmap(GamePanel.Width, GamePanel.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            NextImage = new Bitmap(NextPanel.Width, NextPanel.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            
            GamePanel.BackgroundImage = GameImage;
            NextPanel.BackgroundImage = NextImage;

            //initialise the DrawingGame graphics object
            GameGraphics = Graphics.FromImage(GameImage);
            NextGraphics = Graphics.FromImage(NextImage);

            //Fills the images we just created with white
            GameGraphics.Clear(Color.White);
            NextGraphics.Clear(Color.White);

            InitializeGraphics();

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(SimpleGraphicsForm_KeyDown);
        }

        private void InitializeGraphics()
        {
            //draw the game grid
            DrawGrid();
        }

        private void SimpleGraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            End();
        }

        private void End()
        {
            GameImage.Dispose();
            NextImage.Dispose();
            GameGraphics.Dispose();
            NextGraphics.Dispose();
        }

        /*
         * Draw the basic game grid
         */
        private void DrawGrid()
        {
            for (int x = 0; x < 200; x += 20)
            {
                for (int y = 0; y < 400; y += 20)
                {
                    GameGraphics.DrawRectangle(new Pen(Color.Black), new Rectangle(x, y, 20, 20));
                }
            }

            for (int x = 0; x < 120; x += 20)
            {
                for (int y = 0; y < 120; y += 20)
                { 
                    NextGraphics.DrawRectangle(new Pen(Color.Black), new Rectangle(x, y, 20, 20));
                }
            }                

            this.Refresh();
        }

        /*
         * 
         */
        public void FillGameSquare(int x, int y, Color color)
        {
            //displayMessage("Received for x: "+x+" y: "+y+" ", "ooh");

            //Conver from squares to x and y coordinates
            x *= 20;
            y *= 20;

            if (x >= 0 && x < 200 && y >= 0 && y < 400)
            {
                GameGraphics.FillRectangle(new SolidBrush(color), new Rectangle(x, y, 20, 20));

                SquareFilledEventArgs gameSquareFilledArgs = new SquareFilledEventArgs();
                gameSquareFilledArgs.x = x;
                gameSquareFilledArgs.y = y;

                if (color == Color.Blue)
                {
                    gameSquareFilledArgs.state = 2;
                }
                else if (color == Color.Black)
                {
                    gameSquareFilledArgs.state = 1;
                }
                else //color white
                {
                    gameSquareFilledArgs.state = 0;
                }

                if (GameSquareFilled != null)
                {
                    GameSquareFilled(gameSquareFilledArgs);
                }

                DrawGrid();

                this.Refresh();
            }
        }

        /*
         * 
         */
        public void FillNextSquare(int x, int y, Color color)
        {
            //Conver from squares to x and y coordinates
            x *= 20;
            y *= 20;

            if (x >= 0 && x < 120 && y >= 0 && y < 120)
            {
                NextGraphics.FillRectangle(new SolidBrush(color), new Rectangle(x, y, 20, 20));

                SquareFilledEventArgs nextSquareFilledArgs = new SquareFilledEventArgs();
                nextSquareFilledArgs.x = x;
                nextSquareFilledArgs.y = y;

                if (color.Equals(Color.Black))
                {
                    nextSquareFilledArgs.state = 1;
                }
                else
                {
                    nextSquareFilledArgs.state = 0;
                }

                if (NextSquareFilled != null)
                {
                    NextSquareFilled(nextSquareFilledArgs);
                }

                DrawGrid();

                this.Refresh();
            }
        }

        // Handle the KeyDown event to determine the type of character entered into the control. 
        private void SimpleGraphicsForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (KeyDownEvent != null)
            {
                KeyDownEvent(sender, e);
            }
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            /*Draw the GameImage onto the GamePanel. In this, instead
             * of using DrawingPanel.CreateGraphics(), we can use e.Graphics instead.
             * It will return the graphics used to paint DrawingPanel. This will do the 
             * same as DrawingPanel.CreateGraphics(), but i thought i should point
             * this out*/ 
            e.Graphics.DrawImageUnscaled(GameImage, new Point(0, 0));
        }

        /*
        private void ClearButton_Click(object sender, EventArgs e)
        {
            //Fills the image with white
            Graphics.FromImage(GraphicsImage).Clear(Color.White);
            //Draw the GraphicsImage onto the DrawingPanel
            DrawingPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
        }
        */

        /*
        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            //This sub fires whenever the mouse is moved over the DrawingPanel control
            if (e.Button == MouseButtons.Left) //See if the left mouse button is held down
            {
                //Draw a circle on the canvas
                System.Drawing.Graphics DrawingGraphics = Graphics.FromImage(GraphicsImage);
                if (AACheckBox.Checked)
                {
                    //AntiAliasing is to be used
                    DrawingGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                }
                switch (ShapeComboBox.SelectedIndex)
                {
                    case 0: //draw a filled circle
                        DrawingGraphics.FillEllipse(new SolidBrush(ColorButton.BackColor), new Rectangle(Convert.ToInt32(e.X - numericUpDown1.Value / 2), Convert.ToInt32(e.Y - numericUpDown1.Value / 2), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown1.Value)));
                        break;
                    case 1: //draw an open circle
                        DrawingGraphics.DrawEllipse(new Pen(ColorButton.BackColor), new Rectangle(Convert.ToInt32(e.X - numericUpDown1.Value / 2), Convert.ToInt32(e.Y - numericUpDown1.Value / 2), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown1.Value)));
                        break;
                    case 2: //draw a filled square
                        DrawingGraphics.FillRectangle(new SolidBrush(ColorButton.BackColor), new Rectangle(Convert.ToInt32(e.X - numericUpDown1.Value / 2), Convert.ToInt32(e.Y - numericUpDown1.Value / 2), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown1.Value)));
                        break;
                    case 3: //draw an open square
                        DrawingGraphics.DrawRectangle(new Pen(ColorButton.BackColor), new Rectangle(Convert.ToInt32(e.X - numericUpDown1.Value / 2), Convert.ToInt32(e.Y - numericUpDown1.Value / 2), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown1.Value)));
                        break;
                }
                //Draw the GraphicsImage onto the DrawingPanel
                DrawingPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
            }
        }
         */

        /*
        private void DrawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            //This sub fires whenever the mouse is clicked on the DrawingPanel control. This makes it so a single click will draw as well
            if (e.Button == MouseButtons.Left) //See if the left mouse button is held down
            {
                //Draw a circle on the canvas
                System.Drawing.Graphics DrawingGraphics = Graphics.FromImage(GraphicsImage);
                if (AACheckBox.Checked)
                {
                    //AntiAliasing is to be used
                    DrawingGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                }
                switch (ShapeComboBox.SelectedIndex)
                {
                    case 0: //draw a filled circle
                        DrawingGraphics.FillEllipse(new SolidBrush(ColorButton.BackColor), new Rectangle(Convert.ToInt32(e.X - numericUpDown1.Value / 2), Convert.ToInt32(e.Y - numericUpDown1.Value / 2), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown1.Value)));
                        break;
                    case 1: //draw an open circle
                        DrawingGraphics.DrawEllipse(new Pen(ColorButton.BackColor), new Rectangle(Convert.ToInt32(e.X - numericUpDown1.Value / 2), Convert.ToInt32(e.Y - numericUpDown1.Value / 2), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown1.Value)));
                        break;
                    case 2: //draw a filled square
                        DrawingGraphics.FillRectangle(new SolidBrush(ColorButton.BackColor), new Rectangle(Convert.ToInt32(e.X - numericUpDown1.Value / 2), Convert.ToInt32(e.Y - numericUpDown1.Value / 2), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown1.Value)));
                        break;
                    case 3: //draw an open square
                        DrawingGraphics.DrawRectangle(new Pen(ColorButton.BackColor), new Rectangle(Convert.ToInt32(e.X - numericUpDown1.Value / 2), Convert.ToInt32(e.Y - numericUpDown1.Value / 2), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown1.Value)));
                        break;
                }
                //Draw the GraphicsImage onto the DrawingPanel
                DrawingPanel.CreateGraphics().DrawImageUnscaled(GraphicsImage, new Point(0, 0));
            }
        }
        */
        /* 
        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            /*Draw the GraphicsImage onto the DrawingPanel. In this, instead
             * of using DrawingPanel.CreateGraphics(), we can use e.Graphics instead.
             * It will return the graphics used to paint DrawingPanel. This will do the 
             * same as DrawingPanel.CreateGraphics(), but i thought i should point
             * this out 
            e.Graphics.DrawImageUnscaled(GraphicsImage, new Point(0, 0));
        }
        */

        /*
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //this event fires when the user clicks Save on the save dialog. It will save the image
            GraphicsImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
        }
        */
 
        /*
        private void button1_Click(object sender, EventArgs e)
        {
            //Shows the save dialog
            saveFileDialog1.ShowDialog();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            //Shows the choose color dialog
            colorDialog1.ShowDialog();
            /*Set's the ColorButton's backcolor to the color chosen by
             * the user. When painting on the image, you will notice we
             * refer to this property to get the color to paint with. 
            ColorButton.BackColor = colorDialog1.Color;
        }*/

        //Displays a message.
        public void displayMessage(string message, string title)
        {
           MessageBox.Show(message, title);
        }
    }
}
