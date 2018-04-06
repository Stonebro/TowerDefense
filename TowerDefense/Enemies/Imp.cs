using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Towers;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense.Enemies {
    public class Imp : Enemy {
        public Imp() {
            name = "Imp";
            health = 15;
            size = 10;
            bounty = 3;
        }

        public override void Update() {
            base.Update();
        }

        public override void Render(Graphics g) {
            g.FillRectangle(new SolidBrush(Color.Red), new RectangleF(pos.x + ((BaseTile.size-size)/2), pos.y + ((BaseTile.size - size) / 2), size, size));
        }

        public override void Die() {
            dead = true;
            path = null;
            GameWorld.Instance.AddGold(bounty);
            Console.WriteLine("IMP DIIEED");
            //GameWorld.Instance.enemies.Remove(this);
        }
    }
}
