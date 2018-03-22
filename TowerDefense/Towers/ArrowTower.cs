using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Util;


namespace TowerDefense.Towers {
    class ArrowTower : Tower {
        /// ArrowTower constructor.
        public ArrowTower() { 
            goldCost = 5;
            attackPower = 5;
            splash = new Bitmap(Resources.Resources.ArrowTower);
            sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }
    }
}
