using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Entities;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense.Entities.Enemies {
    public class Imp : Enemy {
        public Imp(int waveBonus) {
            name = "Imp";
            health = 20 + (waveBonus*3);
            size = 10;
            bounty = 3 + waveBonus;
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
            Console.WriteLine("IMP DIIEED");
            //GameWorld.Instance.enemies.Remove(this);
        }
    }
}
