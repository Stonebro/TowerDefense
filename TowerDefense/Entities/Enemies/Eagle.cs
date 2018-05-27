using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;
using TowerDefense.Util.Steering;
using TowerDefense.World;
using static TowerDefense.Util.Steering.SteeringBehavior;

namespace TowerDefense.Entities.Enemies
{
    /// <summary>
    /// This is a class representing an Eagle which is a Flying Entity (It can fly over towers).
    /// </summary>
    public class Eagle : FlyingEntity
    {
        public Eagle(Vector2D pos, Vector2D scale, Vector2D velocity, Vector2D heading, double radius, double mass, double maxSpeed, double maxForce, double maxTurnRate, BehaviorType behaviors)
            : base(pos, scale, velocity, heading, radius, mass, maxSpeed, maxForce, maxTurnRate, behaviors)
        {
        }
    }
}
