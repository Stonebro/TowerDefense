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
        // Position of the Tower.
        public BaseTile pos;
        // Splash Bitmap of the Tower.
        public Bitmap splash;
        // Sprite Bitmap of the Tower.
        public Bitmap sprite;

        public Tower() {

        }

       
        /// "Builds" Tower.
        public virtual void BuildTower(BaseTile pos) {
            this.pos = pos; // Sets position of tower to position specified.
            // Adds Tower to the List of Towers.
            GameWorld.Instance.towers.Add(this);

            //for(int i=0; i<GameWorld.instance.towers.Count; i++) {
            //    Console.WriteLine(GameWorld.instance.towers[i].pos);
            //}
        }
    }
}
