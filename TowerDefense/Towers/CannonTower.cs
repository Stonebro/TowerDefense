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
        public CannonTower() {
            name = "Cannon Tower";
            description = "The Cannon Tower is the Arrow Tower's bigger brother. With increased range and damage, it's an excellent tower to put on a corner to cover ground.";
            goldCost = 15;
            attackPower = 10;
            attackRange = 8;
            attackInterval = 30f;
            splash = new Bitmap(Resources.Resources.CannonTower);
            sprite = new Bitmap(Resources.Resources.CannonTowerSprite);
        }

        // Attack or Reload every frame.
        public override void Update() {
            if (enemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                Attack(enemyInRange());
                attackIntervalCounter++;
            }
            else Reload();
        }

        // Add a shot, draw a line to display the attack and reduce the Enemy's health.
        protected override void Attack(Enemy enemy) {
            base.Attack(enemy);
            shotsFired++;
            if (!enemy.dead && g != null) {
                g.DrawLine(new Pen(Color.Purple, 4), position, (enemy.pos + new Vector2D(7, 7)));
                enemy.health -= attackPower;
            }
        }
    }
}
