using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Entities.Projectiles;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense.FSM {
    class Return : State<AttackDog> {
        public override void Enter(AttackDog t) {
            Console.WriteLine("Returning");
        }

        // Finds a path home and move there.
        public override void Execute(AttackDog t) {
            BaseTile[] tilesList = GameWorld.Instance.tilesList;
            GameWorld instance = GameWorld.Instance;
            if (t.pos != t.homePos.pos) {
                instance.ResetAllVertices();
                t.path = Path.GetPath(tilesList[instance.GetIndexOfTile(t.pos)], tilesList[instance.GetIndexOfTile(t.homePos.pos)]);
                if (t.path.Current != null) {
                    t.Move(4f);
                }
            }
        }

        public override void Exit(AttackDog t) {

        }
    }
}
