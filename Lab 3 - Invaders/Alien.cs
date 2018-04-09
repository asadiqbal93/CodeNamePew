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
        private const int horizontalInterval = 10;
        private const int verticalInterval = 30;
        private double _healthpoint;

        private Bitmap image;
        public Point Location {
            get;
            private set;
        }
        public int Score {
            get;
            private set;
        }
        public Alien(Point location, int score){
            this.Location = location;
            this.Score = score;
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
        // Method closed for Graphics section
        /*
          public Graphics Draw(Graphics graphics, int animationCell) {

          }
        */
        // Method closed for Alien Health logic
        public double AlienHealth() {
            return 0;
        }
        public void createAlien() {
            //Method to create the Alien
        }
    }
}
