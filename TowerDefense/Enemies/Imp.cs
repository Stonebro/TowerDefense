using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;

namespace TowerDefense.Enemies {
    public class Imp : Enemy {
        
        public Imp(Vector2D pos, float health, int size, Vector2D velocity) : base(pos, health, size, velocity) {
            
        }

        public override void Update() {
            base.Update();
        }

        public override void Render(Graphics g) {
            g.FillRectangle(new SolidBrush(Color.Red), new RectangleF(pos.x, pos.y, size, size));
        }
    }
}
