using System;
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
        public int goldCost;
        public int attackPower;
        public BaseTile pos;
        public Bitmap splash;
        public Bitmap sprite;

        public Tower() {

        }

        public virtual void BuildTower(BaseTile pos) {
            this.pos = pos;
            GameWorld.instance.towers.Add(this);

            //for(int i=0; i<GameWorld.instance.towers.Count; i++) {
            //    Console.WriteLine(GameWorld.instance.towers[i].pos);
            //}
        }
    }
}
