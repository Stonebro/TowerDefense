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

namespace TowerDefense.Entities.Enemies
{
    public class Enemy : Entity
    {
        public string name;
        public float health;
        protected int size;
        public Vector2D velocity;
        public bool dead;
        public int bounty;

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

                    else if (path.Current != null) {
                        Move(3.75f);
                    }

                }
            }
        }

        public virtual void Render(Graphics g) { }
        public virtual void Die() { }
    }
}
