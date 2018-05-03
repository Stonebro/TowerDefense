using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.Util;

namespace TowerDefense.Entities {
    class SplitShotTower : Tower {
        public SplitShotTower() {
            name = "Splitshot Tower";
            goldCost = 24;
            attackPower = 1;
            attackRange = 3;
            attackInterval = 15;
            attackTargets = 3;
            splash = new Bitmap(Resources.Resources.ArrowTower);
            sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }

        public override void Update() {
            if (nearbyEnemies != null && enemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                AttackHighestPriority(enemyInRange(attackTargets));
                attackIntervalCounter++;
            } else { DoNothing(); }
        }

        protected override void AttackHighestPriority(List<Enemy> enemies) {
            base.AttackHighestPriority(enemies);
            foreach(Enemy e in enemies) {
                shotsFired++;
                if (!e.dead) {
                    if (b != null) // TEMP. CHEAT
                        b.DrawLine(new Pen(Color.LightGoldenrodYellow, 2), position, (e.pos + new Vector2D(7, 7))); // TEMP. CHEAT
                    e.health -= attackPower;
                }
            }
        }
    }
}
