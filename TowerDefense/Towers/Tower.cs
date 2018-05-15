using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.World;
using TowerDefense.Entities.Enemies;

namespace TowerDefense.Entities {
    public abstract class Tower {
        // Tower's name
        public string name;
        // Tower's description
        public string description = "";
        // Gold cost of placing the Tower.
        public int goldCost;
        // Attacking power of the Tower.
        public int attackPower;
        // Range of the Tower's attacks
        public int attackRange;
        // Interval between Tower's attacks
        public float attackInterval;
        // Timer for interval
        protected int attackIntervalCounter;
        // Amounts of targets the Tower can hit at once
        public int attackTargets;
        // A List of 4 BaseTiles on which this Tower stands.
        public List<BaseTile> pos;
        // Position of the Tower. (The center)
        public Vector2D position;
        // Splash Bitmap of the Tower.
        public Bitmap splash;
        // Sprite Bitmap of the Tower.
        public Bitmap sprite;
        // Bool to see if the Tower's attack range should be drawn
        public bool drawTowerRange;
        // Amount of kills this Tower made;
        public int kills;
        public int shotsFired;

        public Graphics g; // We gave Tower a Graphics because it made it that much easier to draw stuff.

        // "Builds" Tower.
        public virtual void BuildTower(List<BaseTile> pos) {
            this.pos = pos; 
            this.position = pos[0].pos + new Vector2D(BaseTile.size, BaseTile.size); // Sets position of tower to the center of the 4 spaces.
            GameWorld.Instance.towers.Add(this);           
            GameWorld.Instance.RecalculatePaths(pos);
        }

        // Draw a circle around the Tower, displaying it's attack range.
        public virtual void DrawAttackRange(Graphics g) {
            // If a tower's attackrange is 4 spaces. A circle of 10 spaces should be drawn. (4 on either side, plus 2 of the Tower itself)
            int range = (attackRange * 2 + 2);
            Pen pen = new Pen(Color.Red);
            g.DrawEllipse(pen, position.x - (range/2)*BaseTile.size, position.y - (range/2) * BaseTile.size, range*BaseTile.size, range*BaseTile.size);
        }

        // Gets a Tower ready to fire again
        public void Reload() {
            if (attackIntervalCounter % attackInterval != 0) attackIntervalCounter++;
            if (attackIntervalCounter == attackInterval) attackIntervalCounter = 0;
        }

        // This base void only checks if the Tower's attack would kill the enemy. If it does, add a kill and cash in a bounty.
        protected virtual void Attack(Enemy enemy) {
            if (enemy.health - attackPower <= 0) {
                kills++;
                GameWorld.Instance.AddGold(enemy.bounty);
            }
        }

        // Same as the above, except this overload works for multiple targets. (i.e it's for the SplitShotTower)
        protected virtual void Attack(List<Enemy> enemies) {
            foreach (Enemy e in enemies) { 
                if (e.health - attackPower <= 0) {
                    kills++;
                    GameWorld.Instance.AddGold(e.bounty);
                }
            }
        }

        // Loops through each enemy and returns the first Enemy it encounters that's within this Tower's range
        protected Enemy enemyInRange() {
            for(int i = 0; i < GameWorld.Instance.enemies.Count; i++)
                if (GameWorld.Instance.enemies[i].pos.Distance(position) < (attackRange + 1) * BaseTile.size && !GameWorld.Instance.enemies[i].dead)
                    return GameWorld.Instance.enemies[i];
            return null;
        }

        // Same as the above, except it's returns a List of up to 3 targets. (for the SplitShotTower)
        protected List<Enemy> enemyInRange(int amountTargets) {
            List<Enemy> targets = new List<Enemy>();
            for (int i = 0; i < GameWorld.Instance.enemies.Count; i++) { 
                if (GameWorld.Instance.enemies[i].pos.Distance(position) < (attackRange + 1) * BaseTile.size && !GameWorld.Instance.enemies[i].dead)
                    targets.Add(GameWorld.Instance.enemies[i]);
                if (targets.Count == amountTargets) return targets;
            }
            return targets;
        }

        // Purely to override
        public virtual void Update() { }
    }
}
