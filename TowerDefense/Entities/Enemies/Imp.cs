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
            maxHealth = 20 + (waveBonus * 3) ;
            health = maxHealth;
            size = 10;
            bounty = 3 + waveBonus;
        }

        // Move towards the endTile if it's not dead.
        public override void Update() {
            base.Update();
            if (!dead) 
                if (path.Current != null) 
                    Move(3.75f);
        }

        // Fill a little red square to represent the Imp.
        public override void Render(Graphics g) {
            g.FillRectangle(new SolidBrush(Color.Red), new RectangleF(pos.x + ((BaseTile.size-size)/2), pos.y + ((BaseTile.size - size) / 2), size, size));
        }
    }
}
