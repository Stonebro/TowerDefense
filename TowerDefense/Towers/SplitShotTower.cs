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
            splash = new Bitmap(Resources.Resources.ArrowTower);
            sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }

        // Attack up to 3 enemies or Reload every tick
        public override void Update() {
            if (enemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                Attack(enemyInRange(attackTargets));
                attackIntervalCounter++;
            }
            else Reload();
        }

        // Add up to 3 shots, draw a line for each attack and reduce each enemy's health
        protected override void Attack(List<Enemy> enemies) {
            base.Attack(enemies);
            foreach(Enemy e in enemies) {
                shotsFired++;
                if (!e.dead && g != null) {
                    g.DrawLine(new Pen(Color.HotPink, 2), position, (e.pos + new Vector2D(7, 7)));
                    e.health -= attackPower;
                }
            }
        }
    }
}
