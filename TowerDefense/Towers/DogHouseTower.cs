using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.FSM;
using TowerDefense.Tiles;
using TowerDefense.Entities.Projectiles;
using TowerDefense.Entities.Enemies;

namespace TowerDefense.Entities {
    class DogHouseTower : Tower {
        AttackDog attackDog;

        public DogHouseTower() {            
            name = "Dog House Tower";
            description = "This tower has a pet! It's an attackdog that relentlessly chases and attacks the first enemy it sees! When there's no enemies in the tower's range, doggy will return home. (This tower uses a Finite State Machine";
            goldCost = 15;
            attackPower = 5;
            attackRange = 8;
            attackInterval = 7;
            splash = new Bitmap(Resources.Resources.ArrowTower);
            sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }

        public override void BuildTower(List<BaseTile> pos) {
            base.BuildTower(pos);
            attackDog = new AttackDog(this);
        }

        protected override void AttackHighestPriority(Enemy enemy) {
            base.AttackHighestPriority(enemy);
            attackDog.AttackEnemy(enemy);
        }

        public override void Update() {
            attackDog.Render(b);
            attackDog.Update();
            if (nearbyEnemies != null && EnemyInRange() != null) {
                attackDog.ChaseEnemy(EnemyInRange());
            } else {
                attackDog.Return();
            }
        }
    }
}
