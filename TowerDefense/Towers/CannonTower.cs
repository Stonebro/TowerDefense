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
        public CannonTower() { 
            goldCost = 10;
            attackPower = 10;
            splash = new Bitmap("D:/Windesheim GP/AAI/TowerDefense/TowerDefense/Resources/CannonTower.png");
            sprite = new Bitmap("D:/Windesheim GP/AAI/TowerDefense/TowerDefense/Resources/CannonTowerSprite.png");
        }
    }
}
