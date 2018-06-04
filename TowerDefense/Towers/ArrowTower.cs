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
        /// ArrowTower constructor.
        public ArrowTower() {
            name = "Arrow Tower";
            description = "This is the most basic tower. It's deals low damage, but doesn't cost much. It's great for creating a large maze!";
            goldCost = 6;
            attackPower = 1;
            attackRange = 4;
            attackInterval = 10;
            sprite = new Bitmap(Properties.Resources.ArrowTowerSprite);
        }

        public override void Update() {
            if (nearbyEnemies != null && EnemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                AttackHighestPriority(EnemyInRange());
                attackIntervalCounter++;
            }
            else { DoNothing(); }
        }

        protected override void AttackHighestPriority(Enemy enemy) {
            base.AttackHighestPriority(enemy);
            shotsFired++;
            if(!enemy.dead && b != null) {
                b.DrawLine(new Pen(Color.Teal, 2), position, (enemy.pos + new Vector2D(7, 7)));
                enemy.health -= attackPower;
            }
        }
    }
}
