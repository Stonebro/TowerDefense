using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.World;
using TowerDefense.Util;
using TowerDefense.Entities;
using System.Threading;
using TowerDefense.Entities.Enemies;

namespace TowerDefense.Entities {
    class ArrowTower : Tower {
        public ArrowTower() {
            name = "Arrow Tower";
            description = "This is the most basic tower. It's deals low damage, but doesn't cost much. It's great for creating a large maze!";
            goldCost = 6;
            attackPower = 1;
            attackRange = 4;
            attackInterval = 10;
            splash = new Bitmap(Resources.Resources.ArrowTower);
            sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }

        // Attack or Reload every tick.
        public override void Update() {
            if (enemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                Attack(enemyInRange());
                attackIntervalCounter++;
            }
            else Reload();
        }

        // Add a shot, draw a little line to show the attack and reduce the enemy's health
        protected override void Attack(Enemy enemy) {
            base.Attack(enemy);
            shotsFired++;
            if (!enemy.dead && g != null) {
                g.DrawLine(new Pen(Color.Teal, 2), position, (enemy.pos + new Vector2D(7, 7)));
                enemy.health -= attackPower;
            }
        }
    }
}
