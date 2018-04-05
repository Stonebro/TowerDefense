﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.World;
using TowerDefense.CommandPattern;
using TowerDefense.Enemies;

namespace TowerDefense.Towers {
    public abstract class Tower : IReceiver {
        // Gold cost of placing the Tower.
        public int goldCost;
        // Attacking power of the Tower.
        public int attackPower;
        public int attackRange;
        public float attackInterval;
        // Position of the Tower.
        public List<BaseTile> pos;
        public Vector2D position;
        // Splash Bitmap of the Tower.
        public Bitmap splash;
        // Sprite Bitmap of the Tower.
        public Bitmap sprite;
        // Enemies in range
        protected List<Enemy> enemiesInRange = new List<Enemy>();
        public bool drawTowerRange;

        /// "Builds" Tower.
        public virtual void BuildTower(List<BaseTile> pos) {
            this.pos = pos; // Sets position of tower to position specified.
            this.position = pos[0].pos + new Vector2D(BaseTile.size, BaseTile.size);
            Console.WriteLine("pos: " + position);
            //GameWorld.Instance.AddOrDeductGold(-goldCost);
            GameWorld.Instance.towers.Add(this);

        }

        public void DrawAttackRange(Graphics g) {
            int range = (attackRange * 2 + 2);
            //Console.WriteLine(position.x + "  " + position.y + "   " + range);
            Pen pen = new Pen(Color.Red);
            g.DrawEllipse(pen, position.x - (range/2)*BaseTile.size, position.y - (range/2) * BaseTile.size, range*BaseTile.size, range*BaseTile.size);
        }

        public virtual void AttackNearestTarget() {

        }

        public virtual void DoNothing() {
            DoNothingCommand dnc = new DoNothingCommand(this);
            dnc.Execute();
        }

        public virtual void Update() {
            if (GameWorld.Instance.enemies[0].pos.Distance(position) < (attackRange + 2) * BaseTile.size) {
                enemiesInRange.Add(GameWorld.Instance.enemies[0]);
            }
        }
    }
}
