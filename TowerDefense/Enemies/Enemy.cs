using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;
using TowerDefense.CommandPattern;
using TowerDefense.World;

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

        public Enemy(Vector2D pos, float health, int size, Vector2D velocity, Path path)
        {
            this.pos = pos;
            this.health = health;
            this.size = size;
            this.velocity = velocity;
            this.path = path;
            // Console.WriteLine(path.Current + " " + path.End);
        }

        public virtual void Update()
        {
            if(!dead) { 
                if (health <= 0) Die();

                else if (path.Current != null)
                {
                    this.pos = path.Current;
                }
                if(path != null) path.GoNext();
            }

        }

        public virtual void Render(Graphics g) { }
        public virtual void Die() { }
    }
}
