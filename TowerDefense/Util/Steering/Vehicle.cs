using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.World;
using static TowerDefense.Util.Steering.SteeringBehaviour;

namespace TowerDefense.Util.Steering
{
    /// <summary>
    /// This is a partial rewrite of the Vehicle class authored by Mat Buckland. All credit goes to him.
    /// </summary>
    public class Vehicle : FlyingEntity
    {
        public double TimeElapsed;
        public bool Tag;
        public GameWorld World = GameWorld.Instance;
        public Vehicle(Vector2D pos, Vector2D scale, Vector2D velocity, Vector2D heading, double radius, double mass, double maxSpeed, double maxForce, double maxTurnRate, BehaviourType behaviors)
            : base(pos, scale, velocity, heading, radius, mass, maxSpeed, maxForce, maxTurnRate, behaviors)
        {

        }

        public bool RotateHeadingToFacePosition(Vector2D target)
        {
            Vector2D toTarget = Vector2D.Vec2DNormalize(target - Pos);

            double angle = Math.Acos(Heading.Dot(toTarget));

            if (angle < 0.2) return true;

            if (angle > MaxTurnRate) angle = MaxTurnRate;

            C2DMatrix RotationMatrix = new C2DMatrix();

            RotationMatrix.Rotate((float)angle * Heading.Sign(toTarget));
            Vector2D heading = Vector2D.Zero;
            Vector2D velocity = Vector2D.Zero;
            RotationMatrix.TransformVector2Ds(ref heading);
            RotationMatrix.TransformVector2Ds(ref velocity);
            Heading = heading;
            Velocity = velocity;
            Side = Heading.Perp();
            return false;
        }

    }
}
