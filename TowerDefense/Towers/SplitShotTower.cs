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
            description = "This tower functions like an Arrow Tower, but can target up to 3 enemies at once! Place it on corners to maximize it's effect!";
            goldCost = 24;
            attackPower = 1;
            attackRange = 3;
            attackInterval = 15;
            attackTargets = 3;
            //sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }

        public override void Update() {
            if (nearbyEnemies != null && EnemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                AttackHighestPriority(EnemyInRange(attackTargets));
                attackIntervalCounter++;
            } else DoNothing();
        }

        protected override void AttackHighestPriority(List<Enemy> enemies) {
            base.AttackHighestPriority(enemies);
            foreach(Enemy e in enemies) {
                shotsFired++;
                if (!e.dead && b != null) {
                    b.DrawLine(new Pen(Color.LightGoldenrodYellow, 2), position, (e.pos + new Vector2D(7, 7)));
                    e.health -= attackPower;
                }
            }
        }
    }
}
