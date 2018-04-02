using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Lab_3___Invaders
{
    class Game
    {
        private Stars stars;

        private Rectangle formArea;
        private Random random;

        private int score = 0;
        private int livesLeft = 5;
        private int wave = 0;
        private int framesSkipped = 6;
        private int currentGameFrame = 1;

        private Direction invaderDirection;
        private List<Invader> invaders;
        private const int invaderXSpacing = 60;
        private const int invaderYSpacing = 60;

        private PlayerShip playerShip;
        private List<Shot> playerShots;
        private List<Shot> invaderShots;

        private PointF scoreLocation;
        private PointF livesLocation;
        private PointF waveLocation;

        private RectangleF uiRect;

        private Bitmap ui;
        private Bitmap gameOverGraphic;

        Font messageFont = new Font(FontFamily.GenericMonospace, 50, FontStyle.Bold);
        Font statsFont = new Font(FontFamily.GenericMonospace, 12);

        // *************************************** LOAD SOUNDS  **********************************************
        //This SoundPlayer plays a sound whenever the player fires from the playerShip
        System.Media.SoundPlayer fireSoundPlayer = new System.Media.SoundPlayer(@"C:\CodeNamePew\Lab 3 - Invaders\Resources\shoot.wav");
        // This SoundPlayer plays a sound when the game ends 
        System.Media.SoundPlayer gameOverSoundPlayer = new System.Media.SoundPlayer(@"C:\CodeNamePew\Lab 3 - Invaders\Resources\gameover.wav");
        // This SoundPlayer plays a sound when the Game is over
        System.Media.SoundPlayer playerShipDeadSoundPlayer = new System.Media.SoundPlayer(@"C:\CodeNamePew\Lab 3 - Invaders\Resources\explosion.wav");
       
        // ***************************************************************************************************

        public Game(Random random, Rectangle formArea)
        {
            this.formArea = formArea;
            this.random = random;
            stars = new Stars(random, formArea);
            scoreLocation = new PointF((formArea.Left + 240.0F), (formArea.Top + 15.0F));
            livesLocation = new PointF((formArea.Right - 410.0F), (formArea.Top + 15.0F));
            waveLocation = new PointF((formArea.Left + 145.0F), (formArea.Top + 15.0F));
            uiRect = new RectangleF(formArea.Left, formArea.Top, formArea.Width, formArea.Height);
            playerShip = new PlayerShip(formArea, 
                new Point((formArea.Width / 2), (formArea.Height - 50)));
            playerShots = new List<Shot>();
            invaderShots = new List<Shot>();
            invaders = new List<Invader>();
            ui = Properties.Resources.GameUI;
            gameOverGraphic = Properties.Resources.GameOver;


            nextWave();
        }

        // Draw is fired with each paint event of the main form
        public void Draw(Graphics graphics, int frame, bool gameOver)
        {
            graphics.FillRectangle(Brushes.Black, formArea);
            
            stars.Draw(graphics);
            foreach (Invader invader in invaders)
                invader.Draw(graphics, frame);
            playerShip.Draw(graphics);
            foreach (Shot shot in playerShots)
                shot.Draw(graphics);
            foreach (Shot shot in invaderShots)
                shot.Draw(graphics);

            // UI drawing code
            graphics.DrawImage(ui, uiRect);

            Bitmap ship = Properties.Resources.player;

            for (int l = livesLeft; l <= livesLeft && l >= 1; l--)
            {
                graphics.DrawImage(ship, formArea.Right - 330.0F + (30.0F * l), formArea.Top + 15.0F, ship.Width * 0.5F, ship.Height * 0.5F);
            }

            graphics.DrawString(("Score: " + score.ToString()),
                statsFont, Brushes.Yellow, scoreLocation);
            graphics.DrawString(("Lives: " + livesLeft.ToString()),
                statsFont, Brushes.Yellow, livesLocation);
            graphics.DrawString(("Wave: " + wave.ToString()),
                statsFont, Brushes.Yellow, waveLocation);
            if (gameOver)
            {
                graphics.DrawImage(gameOverGraphic, uiRect);
            }

        }

        // Twinkle (animates stars) is called from the form animation timer
        public void Twinkle()
        {
            stars.Twinkle(random);
        }

        public void MovePlayer(Direction direction, bool gameOver)
        {
            if (!gameOver)
            {
                playerShip.Move(direction);
            }
        }

        public void FireShot()
        {
            if (playerShots.Count < 10) //Lee: Changed from Count < 2, enables more bullets for the player to fire
            {
                Shot newShot = new Shot(
                    new Point((playerShip.Location.X + (playerShip.image.Width / 2))
                        , playerShip.Location.Y),
                    Direction.Up, formArea);
                playerShots.Add(newShot);
                // Asad: Sound Player plays the sound of shooting from the PlayerShip
                fireSoundPlayer.Play();
            }
        }

        public void Go()
        {
            if (playerShip.Alive)
            {
                // Check to see if any shots are off screen, to be removed
                List<Shot> deadPlayerShots = new List<Shot>();
                foreach (Shot shot in playerShots)
                {
                    if (!shot.Move())
                        deadPlayerShots.Add(shot);
                }
                foreach (Shot shot in deadPlayerShots)
                    playerShots.Remove(shot);

                List<Shot> deadInvaderShots = new List<Shot>();
                foreach (Shot shot in invaderShots)
                {
                    if (!shot.Move())
                        deadInvaderShots.Add(shot);
                }
                foreach (Shot shot in deadInvaderShots)
                    invaderShots.Remove(shot);

                moveInvaders();
                returnFire();
                checkForCollisions();
                if (invaders.Count < 1)
                {
                    nextWave();
                }
            }

        }

        private void moveInvaders()
        {
            // if the frame is skipped invaders do not move
            if (currentGameFrame > framesSkipped)
            {
                // Check to see if invaders are at edge of screen, 
                // if so change direction
                if (invaderDirection == Direction.Right)
                {
                    var edgeInvaders =
                        from invader in invaders
                        where invader.Location.X > (formArea.Width - 60)// Lee: Changed formArea.Width - 100, so the invaders move all the way to the right of the screen
                        select invader;
                    if (edgeInvaders.Count() > 0)
                    {
                        invaderDirection = Direction.Left;
                        foreach (Invader invader in invaders)
                            invader.Move(Direction.Down);
                    }
                    else
                    {
                        foreach (Invader invader in invaders)
                            invader.Move(Direction.Right);
                    }
                }

                if (invaderDirection == Direction.Left)
                {
                    var edgeInvaders =
                        from invader in invaders
                        where invader.Location.X < 30 //Lee: Changed Location.X < 100, so the invaders move all the way to the left of the screen
                        select invader;
                    if (edgeInvaders.Count() > 0)
                    {
                        invaderDirection = Direction.Right;
                        foreach (Invader invader in invaders)
                            invader.Move(Direction.Down);
                    }
                    else
                    {
                        foreach (Invader invader in invaders)
                            invader.Move(Direction.Left);
                    }
                }

                // Check to see if invaders have made it to the bottom
                var endInvaders =
                        from invader in invaders
                        where invader.Location.Y > playerShip.Location.Y
                        select invader;
                if (endInvaders.Count() > 0)
                {
                    GameOver(this, null);
                }

                foreach (Invader invader in invaders)
                {
                    invader.Move(invaderDirection);
                }

            }
            currentGameFrame++;
            if (currentGameFrame > 6)
                currentGameFrame = 1;
        }

        private void returnFire()
        {
            //// invaders check their location and fire at the player
            if (invaderShots.Count == wave)
                return;
            if (random.Next(10) < (10 - wave))
                return;

            var invaderColumns =
                from invader in invaders
                group invader by invader.Location.X into columns
                select columns;

            int randomColumnNumber = random.Next(invaderColumns.Count());
            var randomColumn = invaderColumns.ElementAt(randomColumnNumber);

            var invaderRow =
            from invader in randomColumn
            orderby invader.Location.Y descending
            select invader;

            Invader shooter = invaderRow.First();
            Point newShotLocation = new Point
                (shooter.Location.X + (shooter.Area.Width / 2),
            shooter.Location.Y + shooter.Area.Height);

            Shot newShot = new Shot(newShotLocation, Direction.Down,
            formArea);
            invaderShots.Add(newShot);
        }


        private void checkForCollisions()
        {
            // Created seperate lists of dead shots since items can't be
            // removed from a list while enumerating through it
            List<Shot> deadPlayerShots = new List<Shot>();
            List<Shot> deadInvaderShots = new List<Shot>();

            foreach (Shot shot in invaderShots)
            {
                if (playerShip.Area.Contains(shot.Location))
                {
                    deadInvaderShots.Add(shot);
                    livesLeft--;
                    playerShip.Alive = false;
                    if (livesLeft == 0)
                        GameOver(this, null);
                    // Asad : Sound Player plays the sound once the playerShip is killed
                    playerShipDeadSoundPlayer.Play();
                    // worth checking for gameOver state here too?
                }
            }

            foreach (Shot shot in playerShots)
            {
                List<Invader> deadInvaders = new List<Invader>();
                foreach (Invader invader in invaders)
                {
                    if (invader.Area.Contains(shot.Location))
                    {
                        deadInvaders.Add(invader);
                        deadInvaderShots.Add(shot);
                        // Score multiplier based on wave
                        score = score + (1 * wave);
                    }
                }
                foreach (Invader invader in deadInvaders) {
                    invaders.Remove(invader);
	                if (invader.Area.Contains(shot.Location)) {
	                	playerShots.Remove(shot);
	                	return;
	                }
                }
            }
            foreach (Shot shot in deadPlayerShots)
                playerShots.Remove(shot);
            foreach (Shot shot in deadInvaderShots)
                invaderShots.Remove(shot);
        }

        private void nextWave()
        {
            wave++;
            Random rnd = new Random(); //Lee: Created the random generator and the IF-ELSE statement so that when a new wave begins,
            if (rnd.Next(0,2) == 0) {  // it will start moving either to the left or the right (random).
            	invaderDirection = Direction.Left;
            } else {
            	invaderDirection = Direction.Right;
            }
            // if the wave is under 7, set frames skipped to 6 - current wave number
            if (wave < 7)
            {
                framesSkipped = 6 - wave;
            }
            else
                framesSkipped = 0;

            int currentInvaderYSpace = 0;
            for (int x = 0; x < 5; x++)
            {
                ShipType currentInvaderType = (ShipType)x;
                currentInvaderYSpace += invaderYSpacing;
                int currentInvaderXSpace = 120; //Lee: Changed this value from 0, so the invaders will start in the middle of the screen
                for (int y = 0; y < 5; y++)
                {
                    currentInvaderXSpace += invaderXSpacing;
                    Point newInvaderPoint =
                        new Point(currentInvaderXSpace, currentInvaderYSpace);
                    // Need to add more varied invader score values
                    Invader newInvader =
                        new Invader(currentInvaderType, newInvaderPoint, 10);
                    invaders.Add(newInvader);
                    playerShots.Clear(); //Lee: Added these two lines to ensure that the 
                    invaderShots.Clear();//map is cleared of bullets when a new wave begins
                }
            }
        }

        public event EventHandler GameOver;
    }
}
