using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Enemies;
using TowerDefense.Util.FuzzyLogic;

namespace TowerDefense.Towers
{
    class FuzzyTower : Tower
    {
        /// FuzzyTower constructor.
        public FuzzyTower()
        {
            name = "Fuzzy Tower";
            goldCost = 73;
            attackPower = 1;
            attackRange = 4;
            attackInterval = 10;
            splash = new Bitmap(Resources.Resources.ArrowTower);
            sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }

        public override void Update()
        {
            if (nearbyEnemies != null && enemyInRange() != null && attackIntervalCounter % attackInterval == 0)
            {
                AttackHighestPriority(enemyInRange());
            }
            else { DoNothing(); }
        }

        protected override void AttackHighestPriority(Enemy enemy)
        {
            if (!enemy.dead)
            {
                Console.WriteLine("Attacking " + DateTime.Now + "   " + attackIntervalCounter);
                enemy.health -= attackPower;
            }
        }

        public FuzzyModule InitFuzzyTowerBaseModule()
        {
            FuzzyModule towerFuzzyModule = new FuzzyModule();

            FuzzyVariable health = towerFuzzyModule.CreateFLV("Health");
            health.AddLeftShoulderSet("Low", 0, 10, 30);
            health.AddTriangularSet("Middle", 30, 50, 70);
            health.AddRightShoulderSet("High", 70, 85, 100);

            return towerFuzzyModule;
        }

        public void FuzzyTowerAttack(FuzzyModule towerFuzzyModule)
        {

        }

    }
}
