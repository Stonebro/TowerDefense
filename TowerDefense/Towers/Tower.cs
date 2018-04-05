﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense.Towers {
    public class Tower {
        // Gold cost of placing the Tower.
        public int goldCost;
        // Attacking power of the Tower.
        public int attackPower;
        public int attackRange;
        public float attackInterval;
        // Position of the Tower.
        public BaseTile pos;
        public Vector2D position;
        // Splash Bitmap of the Tower.
        public Bitmap splash;
        // Sprite Bitmap of the Tower.
        public Bitmap sprite;

        public Tower() {

        }

       
        /// "Builds" Tower.
        public virtual void BuildTower(BaseTile pos) {
            this.pos = pos; // Sets position of tower to position specified.
            this.position = pos.pos + new Vector2D(BaseTile.size, BaseTile.size);
            Console.WriteLine("pos: " + position);
            GameWorld.instance.towers.Add(this);

        }

        public void DrawAttackRange(Graphics g) {
            int range = (attackRange * 2 + 2);
            //Console.WriteLine(position.x + "  " + position.y + "   " + range);
            Pen pen = new Pen(Color.Red);
            g.DrawEllipse(pen, position.x - (range/2)*BaseTile.size, position.y - (range/2) * BaseTile.size, range*BaseTile.size, range*BaseTile.size);
        }

        public virtual void Update() {
        }
    }
}
