using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Lab_3___Invaders
{
    public partial class Form1 : Form
    {
        public int Frame = 0;
        // The form keeps a reference to a single Game object
		private Game game;
		private UserInterface uInterface_main = new UserInterface();
		private UserInterface uInterface_scoreBoard = new UserInterface();
		private UserInterface uInterface_gameOver_score = new UserInterface();
		private UserInterface uInterface_gameOver_buttons = new UserInterface();
		private Panel mainMenu = new Panel();
		private Panel scoreBoardMenu = new Panel();
		private Panel gameOver_score = new Panel();
		private Panel gameOver_buttons = new Panel();
        public Rectangle FormArea { get { return this.ClientRectangle; } }
        Random random = new Random();

        List<Keys> keysPressed = new List<Keys>();

		// backgroud music
		// This SoundPlayer plays a sound when the Game starts at the main menu
		System.Media.SoundPlayer mainMenuMusic = new System.Media.SoundPlayer(@"C:\CodeNamePew\Lab 3 - Invaders\Resources\Mainmenu.wav");

		//font for display score
		Font statsFont = new Font(FontFamily.GenericMonospace, 40);

		//for score
		List<string> scoreboard = new List<string>();
		private string path = Path.Combine(Environment.CurrentDirectory, "scoreboard.txt");

        private bool gameOver;

        public Form1()
        {
            InitializeComponent();
            Frame = 0;
            game = new Game(random, FormArea);
            gameOver = false;
            game.GameOver += new EventHandler(game_GameOver);
            animationTimer.Start();

			//Panels
			mainMenu = uInterface_main.CreatePanel(1);
			scoreBoardMenu = uInterface_scoreBoard.CreatePanel(2);
			gameOver_score = uInterface_gameOver_score.CreatePanel(3);
			gameOver_buttons = uInterface_gameOver_buttons.CreatePanel(4);

			Controls.Add(mainMenu);
			Controls.Add(scoreBoardMenu);
			Controls.Add(gameOver_score);
			Controls.Add(gameOver_buttons);

			uInterface_main.menuImg.MouseClick += new MouseEventHandler(menuImage_MouseClick);
			uInterface_main.btnUnmuteImg.MouseClick += new MouseEventHandler(button_MouseClick);
			uInterface_main.btnMuteImg.MouseClick += new MouseEventHandler(button_MouseClick);

			uInterface_scoreBoard.menuImg.MouseClick += new MouseEventHandler(menuImage_MouseClick);
			uInterface_scoreBoard.menuImg.Paint += new PaintEventHandler(scoreBoardMenu_Paint);

			uInterface_gameOver_score.menuImg.MouseClick += new MouseEventHandler(menuImage_MouseClick);
			uInterface_gameOver_buttons.menuImg.MouseClick += new MouseEventHandler(menuImage_MouseClick);

			if (File.Exists(path))
			{
				scoreboard = game.StoreScores();
			}
			
        }

		private void menuImage_MouseClick(object sender, MouseEventArgs e)
		{
			if (sender == uInterface_main.menuImg)
			{
				Rectangle btnStart = new Rectangle(343, 367, 108, 44);
				Rectangle btnScoreBoard = new Rectangle(265, 448, 263, 42);
				Rectangle btnExit = new Rectangle(357, 535, 83, 45);

				if (btnStart.Contains(e.Location))
				{
					mainMenu.Visible = false;
					scoreBoardMenu.Visible = false;
					gameOver_score.Visible = false;
					gameOver_buttons.Visible = false;

					// code to reset the game
					gameOver = false;
					game = new Game(random, FormArea);
					game.GameOver += new EventHandler(game_GameOver);
					gameTimer.Start();
				}
				else if (btnScoreBoard.Contains(e.Location))
				{
					mainMenu.Visible = false;
					scoreBoardMenu.Visible = true;
				}
				else if (btnExit.Contains(e.Location))
				{
					Application.Exit();
				}
			}
			else if (sender == uInterface_scoreBoard.menuImg)
			{
				Rectangle btnBack = new Rectangle(40, 592, 103, 43);

				if (btnBack.Contains(e.Location))
				{
					scoreBoardMenu.Visible = false;
					mainMenu.Visible = true;
				}
			}
			else if (sender == uInterface_gameOver_score.menuImg)
			{
				Rectangle btnClick = new Rectangle(0, 0, 794, 672);

				if (btnClick.Contains(e.Location))
				{
					gameOver_score.Visible = false;
					gameOver_buttons.Visible = true;
				}
			}
			else if (sender == uInterface_gameOver_buttons.menuImg)
			{
				Rectangle btnPlayAgain = new Rectangle(324, 347, 145, 36);
				Rectangle btnScoreBoard = new Rectangle(318, 410, 156, 30);
				Rectangle btnMainMenu = new Rectangle(320, 477, 154, 28);

				if (btnPlayAgain.Contains(e.Location))
				{
					gameOver_buttons.Visible = false;

                    // code to reset the game
	                    gameOver = false;
	                    game = new Game(random, FormArea);
						game.GameOver += new EventHandler(game_GameOver);
						gameTimer.Start();
				}
				else if (btnScoreBoard.Contains(e.Location))
				{
					gameOver_buttons.Visible = false;
					scoreBoardMenu.Visible = true;
				}
				else if (btnMainMenu.Contains(e.Location))
				{
					gameOver_buttons.Visible = false;
					mainMenu.Visible = true;
					mainMenuMusic.PlayLooping();
				}
			}
		}

		//display score
		private void scoreBoardMenu_Paint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;

			int hx = 220;
			int hy = 140;
			int hxy = 80;
			int c;

			for (int i = 0; i < scoreboard.Count(); i++)
			{
				c = i + 1;

				graphics.DrawString("Score " + c.ToString() + ": " + scoreboard[i], statsFont, Brushes.White, hx, hy);
				hy += hxy;

				if (i == 4)
				{
					break;
				}
			}
		}


		private void button_MouseClick(object sender, MouseEventArgs e)
		{
			if (sender == uInterface_main.btnUnmuteImg)
			{
				mainMenuMusic.Stop();
				uInterface_main.btnUnmuteImg.Visible = false;
				uInterface_main.btnMuteImg.Visible = true;
			}
			else if (sender == uInterface_main.btnMuteImg)
			{
				mainMenuMusic.PlayLooping();
				uInterface_main.btnUnmuteImg.Visible = true;
				uInterface_main.btnMuteImg.Visible = false;
			}
		}

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            if (Frame < 3)
                Frame++;
            else
                Frame = 0;
            game.Twinkle();
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            game.Draw(graphics, Frame, gameOver);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
                Application.Exit();
            if (e.KeyCode == Keys.S)
                {
                    // code to reset the game
                    gameOver = false;
                    game = new Game(random, FormArea);
                    game.GameOver += new EventHandler(game_GameOver);
                    gameTimer.Start();
                    return;
                }
            if (e.KeyCode == Keys.Space)
                game.FireShot();
            if (keysPressed.Contains(e.KeyCode))
                keysPressed.Remove(e.KeyCode);
            keysPressed.Add(e.KeyCode);
        
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(e.KeyCode))
                keysPressed.Remove(e.KeyCode);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            game.Go();
            foreach (Keys key in keysPressed)
            {
                if (key == Keys.Left)
                {
                    game.MovePlayer(Direction.Left, gameOver);
                    return;
                }
                else if (key == Keys.Right)
                {
                    game.MovePlayer(Direction.Right, gameOver);
                    return;
                }
            }
        }

        private void game_GameOver(object sender, EventArgs e)
        {
            gameTimer.Stop();
            gameOver = true;
            Invalidate();

			//record the score 
			if (gameOver)
			{
				game.Record();
			}

			gameOver_score.Visible = true;
        }


    }
}
