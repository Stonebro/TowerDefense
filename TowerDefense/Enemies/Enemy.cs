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
        public float health;
        public int size;
        public Vector2D velocity;

        public Enemy(Vector2D pos, float health, int size, Vector2D velocity) {
            this.pos = pos;
            this.health = health;
            this.size = size;
            this.velocity = velocity;
        }

        public virtual void Update() {

        }

        public virtual void Render(Graphics g) { }
    }
}
