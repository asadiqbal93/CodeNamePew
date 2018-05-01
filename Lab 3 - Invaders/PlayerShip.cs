using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab_3___Invaders
{
    /// <summary>
    /// Player ship class
    /// </summary>
    class PlayerShip
    {
        private const int horizontalInterval = 10;
        public Point Location { get; private set; }
        /// <summary>
        /// read only property for Area
        /// </summary>
        public Rectangle Area
        {
            get
            {
                return new Rectangle(Location, image.Size);
            }
        }
		public Bitmap image = new Bitmap(Properties.Resources.player);

        private DateTime deathWait;

        private bool alive;
        /// <summary>
        /// Alive property
        /// </summary>
        public bool Alive
        {
            get { return alive; }
            set
            {
                alive = value;
                if (!value)
                    deathWait = DateTime.Now;
            }
        }
        
        private Rectangle boundaries;

        private float deadShipHeight;
        /// <summary>
        /// PlayerShip constructor
        /// </summary>
        /// <param name="boundaries"></param>
        /// <param name="location"></param>
        public PlayerShip(Rectangle boundaries, Point location)
        {
            this.boundaries = boundaries;
            this.Location = location;
            Alive = true;
            deadShipHeight = 1.0F;
        }
        /// <summary>
        /// Move method for the player ship - Select from moving left or right
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Direction direction)
        {
            if (Alive)
            {
                if (direction == Direction.Left)
                {
                    Point newLocation = new Point((Location.X - horizontalInterval), Location.Y);
                    if ((newLocation.X < (boundaries.Width - 100)) && (newLocation.X > 50))
                        Location = newLocation;
                }
                else if (direction == Direction.Right)
                {
                    Point newLocation = new Point((Location.X + horizontalInterval), Location.Y);
                    if ((newLocation.X < (boundaries.Width - 100)) && (newLocation.X > 50))
                        Location = newLocation;
                }
            }
        }
        /// <summary>
        /// Draw graphics method for the player ship
        /// </summary>
        /// <param name="graphics"></param>
        public void Draw(Graphics graphics)
        {
            if (!Alive)
            {
                if ((DateTime.Now - deathWait) > TimeSpan.FromSeconds(1.5))
                {
                    deadShipHeight = 0.0F;
                    Alive = true;
                }
                else if ((DateTime.Now - deathWait) > TimeSpan.FromSeconds(1))
                {
                    deadShipHeight = 0.25F;
                }
                else if ((DateTime.Now - deathWait) > TimeSpan.FromSeconds(0.5))
                {
                    deadShipHeight = 0.75F;
                }
                else if ((DateTime.Now - deathWait) > TimeSpan.FromSeconds(0))
                {
                    deadShipHeight = 0.9F;
                }

                graphics.DrawImage(image, (float)Location.X, (float)Location.Y,
                        (float)image.Width, (image.Height * deadShipHeight));

            }
            else
            {
                graphics.DrawImage(image, Location);
            }
        }
    }
}
