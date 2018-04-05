using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.World;
using TowerDefense.Util;
using TowerDefense.Enemies;
using TowerDefense.CommandPattern;
using System.Threading;

namespace TowerDefense.Towers {
    class ArrowTower : Tower {
        /// ArrowTower constructor.
        public ArrowTower() { 
            goldCost = -5;
            attackPower = 8;
            attackRange = 4;
            attackInterval = 0.5f;
            splash = new Bitmap(Resources.Resources.ArrowTower);
            sprite = new Bitmap(Resources.Resources.ArrowTowerSprite);
        }

        public override void BuildTower(List<BaseTile> pos) {
            base.BuildTower(pos);
            if (GameWorld.Instance.enemies[0].pos.Distance(position) < (attackRange + 2) * BaseTile.size) {
                enemiesInRange.Add(GameWorld.Instance.enemies[0]);
            }

        }

        public override void Update() {
            //base.Update();
            if (enemiesInRange.Count != 0) {
                AttackNearestTarget();
            }
            else { DoNothing(); }
        }

        public override void AttackNearestTarget() {
            if (enemiesInRange != null) {
                //SetAction(ACTION_LIST.Attack);
                AttackCommand atkCmd = new AttackCommand(this);
                atkCmd.Execute();
                //Thread.Sleep(1000);
            }
        }

        //public List<Enemy> CalculateNeighbours(Vector2D targetPos, float queryRad) {
        //    Rectangle targetRect = new Rectangle(targetPos - new Vector2D(queryRad, queryRad), new Vector2D(queryRad * 2, queryRad * 2));

        //    List<Enemy> foundNeighbours = new List<Enemy>();

        //    foreach (BaseTile grid in GameWorld.instance.tilesList) {
        //        Rectangle gridRectangle = new Rectangle(grid.pos, new Vector2D(BaseTile.size, BaseTile.size));
        //        if (targetRect.IntersectsWith(gridRectangle)) {
        //            //if (grid.entityCount > 0) {
        //                foreach (Enemy entity in GameWorld.instance.enemies) {
        //                    if (Vector2D.Vec2DDistanceSq(entity.pos, targetPos) < queryRad * queryRad)
        //                        foundNeighbours.Add(entity);
        //                }
        //            //}
        //        }
        //    }
        //    Console.WriteLine(foundNeighbours[0]);
        //    return foundNeighbours;
        //}

    }
}
