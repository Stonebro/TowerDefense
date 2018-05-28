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
            description = "The Cannon Tower is the Arrow Tower's bigger brother. With increased range and damage, it's an excellent tower to put on a corner to cover ground.";
            goldCost = 15;
            attackPower = 10;
            attackRange = 8;
            attackInterval = 30f;
            audioPlayer.URL = "C:/Dev/TowerDefense/TowerDefense/Audio/CannonShot.mp3";
            audioPlayer.settings.volume = 10;
            audioPlayer.controls.stop();
            splash = new Bitmap(Properties.Resources.CannonTowerSplash);
            sprite = new Bitmap(Properties.Resources.CannonTowerSprite);
        }

        public override void Update() {
            if (nearbyEnemies != null && EnemyInRange() != null && attackIntervalCounter % attackInterval == 0) {
                AttackHighestPriority(EnemyInRange());
                attackIntervalCounter++;
            } else { DoNothing(); }
        }

        protected override void AttackHighestPriority(Enemy enemy) {
            base.AttackHighestPriority(enemy);
            shotsFired++;
            audioPlayer.controls.play();
            if (!enemy.dead & b != null) { 
                b.DrawLine(new Pen(Color.Purple, 4), position, (enemy.pos + new Vector2D(7, 7))); 
                enemy.health -= attackPower;
            }
        }
    }
}
