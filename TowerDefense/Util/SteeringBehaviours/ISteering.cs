using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities.Enemies;

namespace TowerDefense.Util.SteeringBehaviours {
    public interface ISteering {
        Vector2D ApplySteering(Enemy enemy);
    }
}
