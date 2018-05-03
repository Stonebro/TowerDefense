using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.Tiles;
using TowerDefense.Util;

namespace TowerDefense.Entities {
    class CannonTower : Tower{
        /// CannonTower constructor.
        public CannonTower() {
            name = "Cannon Tower";
            goldCost = 15;
            attackPower = 10;
            attackRange = 8;
            attackInterval = 30f;
            splash = new Bitmap(Resources.Resources.CannonTower);
            sprite = new Bitmap(Resources.Resources.CannonTowerSprite);
        }

        public override void Update() {
            if (nearbyEnemies != null && enemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                AttackHighestPriority(enemyInRange());
                attackIntervalCounter++;
            } else { DoNothing(); }
        }

        protected override void AttackHighestPriority(Enemy enemy) {
            base.AttackHighestPriority(enemy);
            shotsFired++;
            if (!enemy.dead) {
                if(b!= null)
                    b.DrawLine(new Pen(Color.Purple, 4), position, (enemy.pos + new Vector2D(7, 7))); // TEMP. CHEAT
                enemy.health -= attackPower;
            }
        }
    }
}
