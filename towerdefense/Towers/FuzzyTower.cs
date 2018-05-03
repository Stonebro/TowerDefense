using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.Util.FuzzyLogic;
using TowerDefense.Util.FuzzyLogic.FuzzyOperators;
using TowerDefense.Util.FuzzyLogic.FuzzySets;

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
            FuzzyModule towerFuzzyModule = InitFuzzyTowerBaseModule();
            FuzzyTowerAttack(towerFuzzyModule);
            
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

            FuzzyVariable distanceToEnemy = towerFuzzyModule.CreateFLV("DistanceToEnemy");
            health.AddLeftShoulderSet("Close", 0, 10, 30);
            health.AddTriangularSet("Medium", 30, 50, 70);
            health.AddRightShoulderSet("Far", 70, 85, 100);



            return towerFuzzyModule;
        }

        public void FuzzyTowerAttack(FuzzyModule towerFuzzyModule)
        {
            // Retrieves the antecedents.
            FuzzyVariable health = towerFuzzyModule.GetVar("Health");
            FzSet low = new FzSet(health.GetSet("Low"));
            FzSet middle = new FzSet(health.GetSet("Middle"));
            FzSet high = new FzSet(health.GetSet("High"));

            FuzzyVariable distanceToEnemy= towerFuzzyModule.GetVar("DistanceToEnemy");
            FzSet close = new FzSet(health.GetSet("Low"));
            FzSet medium = new FzSet(health.GetSet("Middle"));
            FzSet far = new FzSet(health.GetSet("High"));

            // Creates the consequent.
            FuzzyVariable shootDesirability = towerFuzzyModule.CreateFLV("ShootDesirability");

            FzSet undesirable = shootDesirability.AddLeftShoulderSet("Undesirable", 0, 10, 20);
            FzSet desirable = shootDesirability.AddTriangularSet("Desirable", 20, 50, 70);
            FzSet veryDesirable = shootDesirability.AddRightShoulderSet("VeryDesirable", 100, 100, 100);

            // Add rules to complete the FAM.
            towerFuzzyModule.AddRule(new FzAND(low, close), veryDesirable);
            towerFuzzyModule.AddRule(new FzAND(low, medium), veryDesirable);
            towerFuzzyModule.AddRule(new FzAND(low, far), veryDesirable);

            towerFuzzyModule.AddRule(new FzAND(middle, close), desirable);
            towerFuzzyModule.AddRule(new FzAND(middle, medium), desirable);
            towerFuzzyModule.AddRule(new FzAND(middle, far), desirable);

            towerFuzzyModule.AddRule(new FzAND(high, close), undesirable);
            towerFuzzyModule.AddRule(new FzAND(high, medium), undesirable);
            towerFuzzyModule.AddRule(new FzAND(high, far), undesirable);

            towerFuzzyModule.Fuzzify("Health", 12);
            towerFuzzyModule.Fuzzify("DistanceToEnemy", 99);
            towerFuzzyModule.PrintAllDOMS();
        }

    }
}
