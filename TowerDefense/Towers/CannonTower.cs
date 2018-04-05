using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Util;

namespace TowerDefense.Towers {
    class CannonTower : Tower{
        /// CannonTower constructor.
        public CannonTower() { 
            goldCost = -10;
            attackPower = 10;
            attackRange = 8;
            attackInterval = 0.2f;
            splash = new Bitmap(Resources.Resources.CannonTower);
            sprite = new Bitmap(Resources.Resources.CannonTowerSprite);
        }
    }
}
