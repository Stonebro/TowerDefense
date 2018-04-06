using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;
using TowerDefense.CommandPattern;
using TowerDefense.World;
using TowerDefense.Tiles;

namespace TowerDefense.Enemies
{
    public class Enemy : IReceiver
    {
        public Vector2D pos;
        public float health;
        protected int size;
        private Vector2D velocity;
        public Path path;
        public bool dead;
        public int bounty;

        /// Enemy constructor.
        public Enemy(Vector2D pos, float health, int size, Vector2D velocity, Path path)
        {
            this.pos = pos;
            this.health = health;
            this.size = size;
            this.velocity = velocity;
            this.path = path;
        }

        /// Handles moving the enemy.
        public virtual void Update()
        {
            BaseTile enemyTile = GameWorld.Instance.tilesList[GameWorld.Instance.GetIndexOfTile(this.pos)];
            if (enemyTile == GameWorld.Instance.endTile)
            {
                GameWorld.Instance.lives--;
                Die();
            }

            if (!dead)
            {
                if (path != null)
                {
                    if (health <= 0) Die();

                    else if (path.Current != null)
                    {
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
                                this.pos = new Vector2D(this.pos.x + 3.75f, this.pos.y);
                            else // Move the enemy to the left.
                                this.pos = new Vector2D(this.pos.x - 3.75f, this.pos.y);
                        }
                        // If the enemy needs to move vertically to reach the next waypoint.
                        if (deltaY != 0)
                        {
                            // If the enemy should move down (delta is positive).
                            if (deltaYPositive)
                                // Move the enemy downwards.
                                this.pos = new Vector2D(this.pos.x, this.pos.y + 3.75f);
                            else
                                // Move the enemy upwards.
                                this.pos = new Vector2D(this.pos.x, this.pos.y - 3.75f);
                        }
                        // If the Enemy reached the waypoint
                        if (path.Current.x == this.pos.x && path.Current.y == this.pos.y && path != null)
                            path.GoNext(); // Make the enemy go to the next waypoint (if there is one).
                    }

                }
            }
        }

        public virtual void Render(Graphics g) { }
        public virtual void Die() { }
    }
}
