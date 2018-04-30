using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Lab_3___Invaders
{
    /// <summary>
    /// Boss Level Class - Alien
    /// </summary>
    class Alien
    {
        private const int horizontalInterval = 5;
        private const int verticalInterval = 30;

        //private double _healthpoint;

        private Bitmap image;
		private Bitmap[] imageArray;

        public Point Location {
            get;
            private set;
        }

		public Rectangle Area
		{
			get
			{
				return new Rectangle(Location, imageArray[0].Size);
			}	
		}

        public int Score {
            get;
            private set;
        }

        public Alien(Point location, int score){
            this.Location = location;
            this.Score = score;
            createInvaderBitmapArray();
			image = imageArray[0];
        }

        public void Move(Direction direction) {

            switch (direction)
            {
                case Direction.Right:
                    Location = new Point((Location.X + horizontalInterval), Location.Y);
                    break;
                case Direction.Left:
                    Location = new Point((Location.X - horizontalInterval), Location.Y);
                    break;
                case Direction.Down:
                    Location = new Point(Location.X, (Location.Y + verticalInterval));
                    break;
            }
        }

		public Graphics Draw(Graphics graphics, int animationCell)
		{
			Graphics bossGraphics = graphics;
			image = imageArray[animationCell];
			try
			{
				graphics.DrawImage(image, Location);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			//DEBUG red square invaders
			//graphics.FillRectangle(Brushes.Red,
			//    Location.X, Location.Y, 20, 20);
			return bossGraphics;
		}

		private void createInvaderBitmapArray()
		{
			imageArray = new Bitmap[4];

			imageArray[0] = Properties.Resources.boss1;
			imageArray[1] = Properties.Resources.boss2;
			imageArray[2] = Properties.Resources.boss3;
			imageArray[3] = Properties.Resources.boss4;
		}

        // Method closed for Alien Health logic
        public double AlienHealth() {
            return 0;
        }
        public void createAlien() {
            //Method to create the Alien
        }
    }
}
