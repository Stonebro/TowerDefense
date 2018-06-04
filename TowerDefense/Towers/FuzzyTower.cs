using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.Entities.Weapons;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.Util.FuzzyLogic;
using TowerDefense.Util.FuzzyLogic.FuzzyOperators;
using TowerDefense.Util.FuzzyLogic.FuzzySets;
using TowerDefense.World;
using static TowerDefense.Util.FuzzyLogic.FuzzyModule;

/// <summary>
///  This Class took a lot of inspiration from Buckland and aspirins
/// </summary>
namespace TowerDefense.Towers
{
    class FuzzyTower : Tower
    {
        private Shotgun shotgun = new Shotgun();
        private Sniper sniper = new Sniper();
        private Weapon weapon;
        private FuzzyModule towerShotgunFuzzyModule;
        private FuzzyModule towerSniperFuzzyModule;
        /// FuzzyTower constructor.
        public FuzzyTower()
        {
            name = "Fuzzy Tower";
            description = "The Fuzzy Tower carries 2 weapons: A long ranged sniper and a short ranged shotgun. It uses Fuzzy Logic to decide which weapon to use.";
            goldCost = 35;
            sprite = new Bitmap(Properties.Resources.FuzzyTowerSprite);
            towerShotgunFuzzyModule = InitFuzzyTowerBaseModule();
            towerSniperFuzzyModule = InitFuzzyTowerBaseModule();
            weapon = sniper;
            FuzzyTowerCalcShotgun(towerShotgunFuzzyModule);
            FuzzyTowerCalcSniper(towerSniperFuzzyModule);
        }

        public override void Update()
        {
            if (attackIntervalCounter % weapon.attackInterval == 0)
            {
                Enemy target = GetDesiredTarget();
                if (target != null)
                {
                    AttackHighestPriority(target);
                    attackIntervalCounter++;
                }
            }
            else DoNothing();
        }

        protected override void AttackHighestPriority(Enemy enemy)
        {
            if (!enemy.dead && b != null)
            {
                if (weapon is Sniper)
                {
                    b.DrawLine(new Pen(Color.Red, 2), position, (enemy.pos + new Vector2D(7, 7)));
                    enemy.health -= weapon.attackPower;
                }
                if (weapon is Shotgun)
                {
                    b.DrawLine(new Pen(Color.DarkTurquoise, 4), position, (enemy.pos + new Vector2D(7, 7)));
                    enemy.health = (float)Math.Floor(enemy.health * (1 - (weapon.attackPower / 100)));
                }
            }
        }

        public override void DrawAttackRange(Graphics g)
        {
            float sniperRange = (sniper.attackRange * 2 + 2);
            float shotgunRange = (shotgun.attackRange * 2 + 2);
            Pen pen = new Pen(Color.Red);
            g.DrawEllipse(pen, position.x - (sniperRange / 2) * BaseTile.size, position.y - (sniperRange / 2) * BaseTile.size, sniperRange * BaseTile.size, sniperRange * BaseTile.size);
            g.DrawEllipse(pen, position.x - (shotgunRange / 2) * BaseTile.size, position.y - (shotgunRange / 2) * BaseTile.size, shotgunRange * BaseTile.size, shotgunRange * BaseTile.size);
        }

        /// <summary>
        ///  Retrieves target with the highest desirability and uses the weapon with the highest desirability to attack the Enemy.
        /// </summary>
        /// <returns>Enemy to attack</returns>
        public Enemy GetDesiredTarget()
        {
            Enemy toReturn = null;
            double highestOverall = -1;
            foreach (Enemy e in GameWorld.Instance.enemies)
            {
                double highestForThisLoop;
                if (position.Distance(e.pos) < (sniper.attackRange + 1) * BaseTile.size)
                {
                    double shotgunDesirability;
                    towerShotgunFuzzyModule.Fuzzify("Health", e.health / e.maxHealth * 100);
                    towerShotgunFuzzyModule.Fuzzify("DistanceToEnemy", position.Distance(e.pos) / ((sniper.attackRange + 1) * BaseTile.size) * 100);
                    shotgunDesirability = towerShotgunFuzzyModule.DeFuzzify("ShootDesirability", DefuzzifyMethod.MAX_AV);

                    double sniperDesirability;
                    towerSniperFuzzyModule.Fuzzify("Health", e.health / e.maxHealth * 100);
                    towerSniperFuzzyModule.Fuzzify("DistanceToEnemy", position.Distance(e.pos) / ((sniper.attackRange + 1) * BaseTile.size) * 100);
                    sniperDesirability = towerSniperFuzzyModule.DeFuzzify("ShootDesirability", DefuzzifyMethod.MAX_AV);

                    if (shotgunDesirability > sniperDesirability)
                    {
                        highestForThisLoop = shotgunDesirability;
                        if (highestForThisLoop > highestOverall) weapon = shotgun;
                    }
                    else
                    {
                        highestForThisLoop = sniperDesirability;
                        if (highestForThisLoop > highestOverall) weapon = sniper;
                    }
                    if (highestForThisLoop > highestOverall)
                    {
                        highestOverall = highestForThisLoop;
                        toReturn = e;
                    }
                }
            }
            return toReturn;
        }

        public FuzzyModule InitFuzzyTowerBaseModule()
        {
            FuzzyModule towerFuzzyModule = new FuzzyModule();

            FuzzyVariable health = towerFuzzyModule.CreateFLV("Health");
            health.AddLeftShoulderSet("Low", 0, 12.5, 25);
            health.AddTriangularSet("Middle", 20, 40, 65);
            health.AddRightShoulderSet("High", 60, 100, 100);

            FuzzyVariable distanceToEnemy = towerFuzzyModule.CreateFLV("DistanceToEnemy");
            distanceToEnemy.AddLeftShoulderSet("Close", 0, 33, 33);
            distanceToEnemy.AddTriangularSet("Medium", 33, 50, 66);
            distanceToEnemy.AddRightShoulderSet("Far", 66, 70, 100);

            return towerFuzzyModule;
        }

        public void FuzzyTowerCalcShotgun(FuzzyModule towerFuzzyModule)
        {
            // Retrieves the antecedents.
            FuzzyVariable health = towerFuzzyModule.GetVar("Health");
            FzSet low = new FzSet(health.GetSet("Low"));
            FzSet middle = new FzSet(health.GetSet("Middle"));
            FzSet high = new FzSet(health.GetSet("High"));

            FuzzyVariable distanceToEnemy = towerFuzzyModule.GetVar("DistanceToEnemy");
            FzSet close = new FzSet(distanceToEnemy.GetSet("Close"));
            FzSet medium = new FzSet(distanceToEnemy.GetSet("Medium"));
            FzSet far = new FzSet(distanceToEnemy.GetSet("Far"));

            // Creates the consequent.
            FuzzyVariable shootDesirability = towerFuzzyModule.CreateFLV("ShootDesirability");

            FzSet undesirable = shootDesirability.AddLeftShoulderSet("Undesirable", 0, 20, 40);
            FzSet desirable = shootDesirability.AddTriangularSet("Desirable", 30, 40, 60);
            FzSet veryDesirable = shootDesirability.AddRightShoulderSet("VeryDesirable", 60, 90, 100);

            // Add rules to complete the FAM.
            towerFuzzyModule.AddRule(new FzAND(low, close), undesirable);
            towerFuzzyModule.AddRule(new FzAND(low, medium), undesirable);
            towerFuzzyModule.AddRule(new FzAND(low, far), undesirable);

            towerFuzzyModule.AddRule(new FzAND(middle, close), veryDesirable);
            towerFuzzyModule.AddRule(new FzAND(middle, medium), undesirable);
            towerFuzzyModule.AddRule(new FzAND(middle, far), undesirable);

            towerFuzzyModule.AddRule(new FzAND(high, close), veryDesirable);
            towerFuzzyModule.AddRule(new FzAND(high, medium), undesirable);
            towerFuzzyModule.AddRule(new FzAND(high, far), undesirable);
        }

        public void FuzzyTowerCalcSniper(FuzzyModule towerFuzzyModule)
        {
            // Retrieves the antecedents.
            FuzzyVariable health = towerFuzzyModule.GetVar("Health");
            FzSet low = new FzSet(health.GetSet("Low"));
            FzSet middle = new FzSet(health.GetSet("Middle"));
            FzSet high = new FzSet(health.GetSet("High"));


            FuzzyVariable distanceToEnemy = towerFuzzyModule.GetVar("DistanceToEnemy");
            FzSet close = new FzSet(distanceToEnemy.GetSet("Close"));
            FzSet medium = new FzSet(distanceToEnemy.GetSet("Medium"));
            FzSet far = new FzSet(distanceToEnemy.GetSet("Far"));

            // Creates the consequent.
            FuzzyVariable shootDesirability = towerFuzzyModule.CreateFLV("ShootDesirability");

            FzSet undesirable = shootDesirability.AddLeftShoulderSet("Undesirable", 0, 15, 30);
            FzSet desirable = shootDesirability.AddTriangularSet("Desirable", 15, 50, 75);
            FzSet veryDesirable = shootDesirability.AddRightShoulderSet("VeryDesirable", 70, 85, 100);

            // Add rules to complete the FAM.
            towerFuzzyModule.AddRule(new FzAND(low, close), desirable);
            towerFuzzyModule.AddRule(new FzAND(low, medium), desirable);
            towerFuzzyModule.AddRule(new FzAND(low, far), veryDesirable);

            towerFuzzyModule.AddRule(new FzAND(middle, close), undesirable);
            towerFuzzyModule.AddRule(new FzAND(middle, medium), undesirable);
            towerFuzzyModule.AddRule(new FzAND(middle, far), desirable);

            towerFuzzyModule.AddRule(new FzAND(high, close), undesirable);
            towerFuzzyModule.AddRule(new FzAND(high, medium), desirable);
            towerFuzzyModule.AddRule(new FzAND(high, far), desirable);
        }

    }
}
