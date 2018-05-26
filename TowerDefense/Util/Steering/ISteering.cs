using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities.Enemies;

namespace TowerDefense.Util.Steering
{
    // Used in earlier implementation of Steering behaviors (Bat). We figured we might aswell keep it in.
    public interface ISteering
    {
        Vector2D ApplySteering(Enemy enemy);
    }
}
