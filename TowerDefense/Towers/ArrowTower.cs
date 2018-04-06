using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.World;
using TowerDefense.Util;
using TowerDefense.Enemies;
using TowerDefense.CommandPattern;
using System.Threading;

namespace TowerDefense.Towers {
    class ArrowTower : Tower {
        /// ArrowTower constructor.
        public ArrowTower() {
            name = "Arrow Tower";
            goldCost = 5;
            attackPower = 1;
            attackRange = 4;
            attackInterval = 10;
            splash = new Bitmap(Resources.Resources.ArrowTower);
            sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }

        public override void Update() {
            Console.WriteLine((nearbyEnemies != null) + "    " + (enemyInRange() != null) + "    " + (attackIntervalCounter % attackInterval == 0));
            if (nearbyEnemies != null && enemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                AttackHighestPriority(enemyInRange());
            }
            else { DoNothing(); }
        }

        protected override void AttackHighestPriority(Enemy enemy) {
            if(!enemy.dead) { 
                Console.WriteLine("Attacking " + DateTime.Now + "   " + attackIntervalCounter);
                enemy.health -= attackPower;
            }
        }
    }
}
