﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Enemies;
using TowerDefense.Tiles;
using TowerDefense.Util;

namespace TowerDefense.Towers {
    class CannonTower : Tower{
        /// CannonTower constructor.
        public CannonTower() {
            name = "Cannon Tower";
            goldCost = 10;
            attackPower = 10;
            attackRange = 8;
            attackInterval = 30f;
            splash = new Bitmap(Resources.Resources.CannonTower);
            sprite = new Bitmap(Resources.Resources.CannonTowerSprite);
        }

        public override void Update() {
            if (nearbyEnemies != null && enemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                AttackHighestPriority(enemyInRange());
            } else { DoNothing(); }
        }

        protected override void AttackHighestPriority(Enemy enemy) {
            base.AttackHighestPriority(enemy);
            if (!enemy.dead) {
                Console.WriteLine("Attacking " + DateTime.Now + "   " + attackIntervalCounter);
                enemy.health -= attackPower;
            }
        }
    }
}
