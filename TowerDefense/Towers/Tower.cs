using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Enemies;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.World;
using TowerDefense.CommandPattern;
using TowerDefense.Enemies;

namespace TowerDefense.Towers {
    public abstract class Tower : IReceiver {
        // Tower's name
        public string name;
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
        // Position of the Tower.
        public List<BaseTile> pos;
        public Vector2D position;
        // Splash Bitmap of the Tower.
        public Bitmap splash;
        // Sprite Bitmap of the Tower.
        public Bitmap sprite;
        // Enemies in range
        protected List<Enemy> enemiesInRange = new List<Enemy>();
        // Bool to see if the Tower's attackrange should be drawn
        public bool drawTowerRange;
        // Amount of kills this Tower made;
        public int kills;
        //public PriorityQueue<Enemy> nearbyEnemies = new PriorityQueue<Enemy>();
        public List<Enemy> nearbyEnemies = new List<Enemy>();

        /// "Builds" Tower.
        public virtual void BuildTower(List<BaseTile> pos) {
            this.pos = pos; // Sets position of tower to position specified.
            this.position = pos[0].pos + new Vector2D(BaseTile.size, BaseTile.size);
            GameWorld.Instance.towers.Add(this);
            GameWorld.Instance.RecalculatePaths();
        }

        // Draw a circle with a radius of 'this.attackRange' squares
        public void DrawAttackRange(Graphics g) {
            int range = (attackRange * 2 + 2);
            Pen pen = new Pen(Color.Red);
            g.DrawEllipse(pen, position.x - (range/2)*BaseTile.size, position.y - (range/2) * BaseTile.size, range*BaseTile.size, range*BaseTile.size);
        }

        public virtual void AttackNearestTarget() {

        }

        public virtual void DoNothing() {
            DoNothingCommand dnc = new DoNothingCommand(this);
            dnc.Execute();
        }

        protected virtual void AttackHighestPriority(Enemy enemy) {
            if (enemy.health - attackPower <= 0) kills++;
        }

        protected Enemy enemyInRange() {
            for(int i = 0; i < GameWorld.Instance.enemies.Count; i++)
                if (GameWorld.Instance.enemies[i].pos.Distance(position) < (attackRange + 2) * BaseTile.size && !GameWorld.Instance.enemies[i].dead)
                    return GameWorld.Instance.enemies[i];
            return null;
        }

        public virtual void Update() {
            if (enemyInRange() != null || attackIntervalCounter % 10 != 0) attackIntervalCounter++;
        }
    }
}
