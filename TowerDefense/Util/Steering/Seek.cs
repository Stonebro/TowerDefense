using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities.Enemies;

namespace TowerDefense.Util.Steering
{
    // Used in earlier implementation of Steering behaviors (Bat). We figured we might aswell keep it in.
    public class Seek : ISteering
    {
        public Vector2D ApplySteering(Enemy enemy)
        {
            Vector2D dir = enemy.path.Current - enemy.pos;
            dir.Normalize();
            return dir;
        }
    }
}
