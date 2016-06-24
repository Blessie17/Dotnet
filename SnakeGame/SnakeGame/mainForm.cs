using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class mainForm : Form
    {
        List<SnakeBody> createSnake = new List<SnakeBody>();
        int gameScore = 0;
        int movementDirection = 0; // Down = 0, Left = 1, Right = 2, Up = 3
        const int blockWidth = 12;
        const int blockHeight = 12;
        bool gameOverflag = false;
        SnakeBody snakeFood = new SnakeBody();
        public mainForm()
        {
            InitializeComponent();
            snakeTimer.Interval = 1000 / 6; //speed of the snake
            snakeTimer.Tick += new EventHandler(UpdateOnTick);
            snakeTimer.Start();
            StartNewGame();
        }
        private void StartNewGame()
        {
            gameOverflag = false;
            gameScore = 0;
            movementDirection = 0;
            createSnake.Clear();
            SnakeBody snakeHead = new SnakeBody();
            snakeHead.XcooR = 10;
            snakeHead.YCooR = 5;
            createSnake.Add(snakeHead);
            createFood();
        }
        private void createFood()
        {
            int maxBlockWidth = gameBoard.Size.Width / blockWidth;
            int maxBlockHeight = gameBoard.Size.Height / blockHeight;
            Random random = new Random();
            snakeFood = new SnakeBody();
            snakeFood.XcooR = random.Next(0, maxBlockWidth);
            snakeFood.YCooR = random.Next(0, maxBlockHeight);
        }
        
        private void UpdateOnTick(object sender, EventArgs e)
        {
            if (gameOverflag)
            {
                if (keyControls.Pressed(Keys.Enter))
                    StartNewGame();
            }
            else
            {
                if (keyControls.Pressed(Keys.Right))
                {
                    if (createSnake.Count < 2 || createSnake[0].XcooR == createSnake[1].XcooR)
                        movementDirection = 2;
                }
                else if (keyControls.Pressed(Keys.Left))
                {
                    if (createSnake.Count < 2 || createSnake[0].XcooR == createSnake[1].XcooR)
                        movementDirection = 1;
                }
                else if (keyControls.Pressed(Keys.Up))
                {
                    if (createSnake.Count < 2 || createSnake[0].YCooR == createSnake[1].YCooR)
                        movementDirection = 3;
                }
                else if (keyControls.Pressed(Keys.Down))
                {
                    if (createSnake.Count < 2 || createSnake[0].YCooR == createSnake[1].YCooR)
                        movementDirection = 0;
                }
                DrawUpdatedSnake();
            }
            gameBoard.Invalidate();
        }
        private void DrawUpdatedSnake()
        {
            for (int i = createSnake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (movementDirection)
                    {
                        case 0: // Move down
                            createSnake[i].YCooR++;
                            break;
                        case 1: // Move left
                            createSnake[i].XcooR--;
                            break;
                        case 2: // Move Right
                            createSnake[i].XcooR++;
                            break;
                        case 3: // Move Up
                            createSnake[i].YCooR--;
                            break;
                    }
                    int maxBlockWidth = gameBoard.Width / blockWidth;
                    int maxBlockHeight = gameBoard.Height / blockHeight;
                    if (createSnake[i].XcooR < 0 || createSnake[i].XcooR >= maxBlockWidth || createSnake[i].YCooR < 0 || createSnake[i].YCooR >= maxBlockHeight)
                        gameOverflag = true;
                    for (int j = 1; j < createSnake.Count; j++)
                        if (createSnake[i].XcooR == createSnake[j].XcooR && createSnake[i].YCooR == createSnake[j].YCooR)
                            gameOverflag = true;
                    if (createSnake[i].XcooR == snakeFood.XcooR && createSnake[i].YCooR == snakeFood.YCooR)
                    {
                        SnakeBody part = new SnakeBody();
                        part.XcooR = createSnake[createSnake.Count - 1].XcooR;
                        part.YCooR = createSnake[createSnake.Count - 1].YCooR;
                        createSnake.Add(part);
                        createFood();
                        gameScore = gameScore + 10;
                    }
                }
                else
                {
                    createSnake[i].XcooR = createSnake[i - 1].XcooR;
                    createSnake[i].YCooR = createSnake[i - 1].YCooR;
                }
            }
        } 
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            keyControls.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            keyControls.ChangeState(e.KeyCode, false);
        }
        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics gameLayout = e.Graphics;
            SolidBrush drawBrush = new SolidBrush(Color.Maroon);
            if (gameOverflag)
            {
                Font font = this.Font;
                string strGameover = "Game Over !";
                string strScore = "Your Score: " + gameScore.ToString();
                string strMessage = "Press Enter to Start a new game";
                int center_width = gameBoard.Width / 2;
                SizeF textSize = gameLayout.MeasureString(strGameover, font);
                PointF textCooR = new PointF(center_width - textSize.Width / 2, 16);
                gameLayout.DrawString(strGameover, font, drawBrush, textCooR);
                textSize = gameLayout.MeasureString(strScore, font);
                textCooR = new PointF(center_width - textSize.Width / 2, 32);
                gameLayout.DrawString(strScore, font, drawBrush, textCooR);
                textSize = gameLayout.MeasureString(strMessage, font);
                textCooR = new PointF(center_width - textSize.Width / 2, 48);
                gameLayout.DrawString(strMessage, font, drawBrush, textCooR);
            }
            else
            {
                for (int i = 0; i < createSnake.Count; i++)
                {
                    Brush snakeColor = i == 0 ? Brushes.DarkGreen : Brushes.DarkTurquoise;
                    gameLayout.FillRectangle(snakeColor, new Rectangle(createSnake[i].XcooR * blockWidth, createSnake[i].YCooR * blockHeight, blockWidth, blockHeight));
                }
                gameLayout.FillRectangle(Brushes.Black, new Rectangle(snakeFood.XcooR * blockWidth, snakeFood.YCooR * blockHeight, blockWidth, blockHeight));
                gameLayout.DrawString("Score: " + gameScore.ToString(), this.Font, drawBrush, new PointF(4, 4));
            }
        }
    }
}