using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;

namespace TowerDefense.Enemies {
    public class Enemy {
        public Vector2D pos;
        private float health;
        protected int size;
        private Vector2D velocity;
        public Path path;

        public Enemy(Vector2D pos, float health, int size, Vector2D velocity, Path path) {
            this.pos = pos;
            this.health = health;
            this.size = size;
            this.velocity = velocity;
            this.path = path;
           // Console.WriteLine(path.Current + " " + path.End);
        }

        public virtual void Update() {

            if(path != null && path.Current != null)
            {
                //Console.WriteLine(path.Current);
                this.pos = path.Current;
                path.GoNext();
            }
        }

        public virtual void Render(Graphics g) { }
    }
}
