using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;

namespace TowerDefense.Entities
{
    public class Entity
    {
        public Path path;
        public Vector2D pos;

        public void Move(float speed) {
            if (path.Current != null) {
                // Calculates x difference in next waypoint on Path and the enemy. 
                float deltaX = path.Current.x - this.pos.x;
                // Is the difference a positive number or not (so should the enemy move to the left or to the right). 
                bool deltaXPositive = deltaX >= 0;
                // Calculates y difference in next waypoint on Path and the enemy. 
                float deltaY = path.Current.y - this.pos.y;
                // Is the difference a positive number or not (so should the enemy move up or down).
                bool deltaYPositive = deltaY >= 0;


                // If the enemy needs to move horizontally to reach the next waypoint.
                if (deltaX != 0)
                {
                    // if the enemy should move to the right (delta is positive).
                    if (deltaXPositive)
                        // Move the enemy to the right.
                        this.pos = new Vector2D(this.pos.x + speed, this.pos.y);
                    else // Move the enemy to the left.
                        this.pos = new Vector2D(this.pos.x - speed, this.pos.y);
                }
                // If the enemy needs to move vertically to reach the next waypoint.
                if (deltaY != 0)
                {
                    // If the enemy should move down (delta is positive).
                    if (deltaYPositive)
                        // Move the enemy downwards.
                        this.pos = new Vector2D(this.pos.x, this.pos.y + speed);
                    else
                        // Move the enemy upwards.
                        this.pos = new Vector2D(this.pos.x, this.pos.y - speed);
                }
                // If the Enemy reached the waypoint
                if (path.Current.x == this.pos.x && path.Current.y == this.pos.y && path != null)
                    path.GoNext(); // Make the enemy go to the next waypoint (if there is one).
            }
        }
    }
}
