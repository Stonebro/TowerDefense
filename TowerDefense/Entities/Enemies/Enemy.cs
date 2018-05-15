using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;
using TowerDefense.World;
using TowerDefense.Tiles;

namespace TowerDefense.Entities.Enemies
{
    public class Enemy : Entity
    {
        public string name;
        public float maxHealth;
        public float health;
        protected int size;
        public Vector2D velocity;
        public bool dead;
        public int bounty;

        // Checks if the enemy made it to the endTile or died.
        public virtual void Update()
        {
            BaseTile enemyTile = GameWorld.Instance.tilesList[GameWorld.Instance.GetIndexOfTile(this.pos)];
            if (enemyTile == GameWorld.Instance.endTile) {
                GameWorld.Instance.lives--;
                Die();
            }

            if (!dead) {
                if (path != null) {
                    if (health <= 0) Die();
                }
            }
        }

        // Flips a bool and nulls the path.
        public virtual void Die() {
            dead = true;
            path = null;
        }

        public virtual void Render(Graphics g) { }


    }
}
