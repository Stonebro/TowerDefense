using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities.Enemies;

namespace TowerDefense.Util.Steering {
    public class Seek : ISteering {
        public Vector2D ApplySteering(Enemy enemy) {
            Vector2D dir = enemy.path.Current - enemy.pos;
            dir.Normalize();
            return dir;
        }
    }
}
