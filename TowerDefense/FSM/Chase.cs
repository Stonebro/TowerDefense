﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;
using TowerDefense.Entities.Projectiles;
using TowerDefense.World;
using TowerDefense.Tiles;

namespace TowerDefense.FSM {
    class Chase : State<AttackDog> {
        public override void Enter(AttackDog t) {
            //Console.WriteLine("Chasing a target!   counter = " + t.attackIntervalCounter);
        }

        public override void Execute(AttackDog t) {
            BaseTile[] tilesList = GameWorld.Instance.tilesList;
            GameWorld instance = GameWorld.Instance;
            if(t.pos != t.target.pos) {
                instance.ResetAllVertices();
                t.path = Path.GetPath(tilesList[instance.GetIndexOfTile(t.pos)], tilesList[instance.GetIndexOfTile(t.target.pos)]);
                if(t.path.Current != null) {
                    t.Move(8f);
                }
            }
            //if (t.attackIntervalCounter != t.attackInterval) t.attackIntervalCounter++;
        }

        public override void Exit(AttackDog t) {

        }

    }
}