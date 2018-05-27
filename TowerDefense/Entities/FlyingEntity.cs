using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities.Powerups;
using TowerDefense.Util;
using TowerDefense.Util.Steering;
using TowerDefense.World;
using static TowerDefense.Util.Steering.SteeringBehavior;

namespace TowerDefense.Entities
{
    /// <summary>
    ///  This is a rewrite of the MovingEntity class authored by Mat Buckland and all credit goes to him.
    /// </summary>
    public class FlyingEntity
    {
        // The position of the Entity. 
        public Vector2D Pos { get; set; }
        // The scale of the Entity. 
        public Vector2D Scale { get; set; }
        // The velocity of the Entity.
        public Vector2D Velocity { get; set; }
        // A normalized vector pointing in the direction the entity is heading. 
        public Vector2D Heading { get; set; }
        // A vector perpendicular to the heading vector.
        public Vector2D Side { get; set; } = Vector2D.Zero;
        // Instance of the GameWorld singleton.
        public GameWorld World = GameWorld.Instance;
        // Queue of Powerups(goals) for the Explore behaviour.
        public Queue<Powerup> goals;
        // Copy of original goals to keep looping through the goals
        public Queue<Powerup> originalGoals;

        // The radius of the bounding box of the Entity.
        public double Radius { get; set; }
        // The mass of the Entity.
        public double Mass { get; }
        // The maximum speed which the entity may travel at.
        public double MaxSpeed { get; set; }
        // The maximum force that can be applied to the Entity.
        public double MaxForce { get; set; }
        // The maximum rate (in radians per second) at which this Entity can rotate.
        public double MaxTurnRate { get; set; }
        public double BoundingRadius { get; set; }
        public bool Tag;

        private SteeringBehavior _behaviour;

        public FlyingEntity(Vector2D pos, Vector2D scale, Vector2D velocity, Vector2D heading, double radius, double mass, double maxSpeed, double maxForce, double maxTurnRate, BehaviorType behaviors)
        {
            Pos = pos;
            Scale = scale;
            Velocity = velocity;
            Heading = heading;
            Radius = radius;
            Mass = mass;
            MaxSpeed = maxSpeed;
            MaxForce = maxForce;
            MaxTurnRate = maxTurnRate;
            _behaviour = new SteeringBehavior(this)
            {
                behaviours = behaviors
            };
            originalGoals = goals;
        }

        public bool IsSpeedMaxedOut()
        {
            return MaxSpeed * MaxSpeed > Velocity.LengthSq();
        }

        public float Speed()
        {
            return Velocity.Length();
        }

        public float SpeedSq()
        {
            return Velocity.LengthSq();
        }

        public void SetHeading(Vector2D newheading)
        {
            if ((newheading.LengthSq() - 1) < 0.00001)
            {
                Heading = newheading;
                Side = Heading.Perp();
            }
        }

        public void Update(double time_elapsed)
        {
            Vector2D oldPos = Pos;
            Vector2D steeringForce = _behaviour.Calculate();
            Vector2D acceleration = steeringForce / Mass;
            Velocity = acceleration * time_elapsed / 40;
            Velocity.Truncate((float)MaxSpeed);
            Pos += Velocity * time_elapsed / 40;


            if (Velocity.LengthSq() > float.Epsilon)
            {
                Heading = Vector2D.Normalize(Velocity);
                Side = Heading.Perp();
            }
        }

        public void SetTargetAgent1(FlyingEntity target)
        {
            _behaviour.TargetAgent1 = target;
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
