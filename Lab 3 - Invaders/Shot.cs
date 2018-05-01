using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Lab_3___Invaders
{
    /// <summary>
    /// Shot Class
    /// </summary>
    class Shot
    {
        private const int moveInterval = 9;
        private const int width = 3;
        private const int height = 10;

        public Point Location { get; private set; }

        private Direction direction;
        private Rectangle boundaries;
        /// <summary>
        /// Constructor for Shot 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="direction"></param>
        /// <param name="boundaries"></param>
        public Shot(Point location, Direction direction,
            Rectangle boundaries)
        {
            this.Location = location;
            this.direction = direction;
            this.boundaries = boundaries;
        }
        /// <summary>
        /// The laser object draw method -- shooting
        /// </summary>
        /// <param name="graphics"></param>
        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Red,
                Location.X, Location.Y, width, height);
        }
        /// <summary>
        /// Move method 
        /// </summary>
        /// <returns></returns>
        public bool Move()
        {
            Point newLocation;
            if (direction == Direction.Down)
            {
                newLocation = new Point(Location.X, (Location.Y + moveInterval));
            }
			else if (direction == Direction.Down20L)
			{
				newLocation = new Point((Location.X - 3), (Location.Y + moveInterval));
			}
			else if (direction == Direction.Down20R)
			{
				newLocation = new Point((Location.X + 3), (Location.Y + moveInterval));
			}
			else if (direction == Direction.Down40L)
			{
				newLocation = new Point((Location.X - 6), (Location.Y + moveInterval));
			}
			else if (direction == Direction.Down40R)
			{
				newLocation = new Point((Location.X + 6), (Location.Y + moveInterval));
			}
            else //if (direction == Direction.Up)
            {
                newLocation = new Point(Location.X, (Location.Y - moveInterval));
            }

            if ((newLocation.Y < boundaries.Height) && (newLocation.Y > 0))
            {
                Location = newLocation;
                return true;
            }
            else
                return false;
        }
    }
}
