using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util;

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

        public FlyingEntity(Vector2D pos, Vector2D scale, Vector2D velocity, Vector2D heading, double radius, double mass, double maxSpeed, double maxForce, double maxTurnRate)
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
    }
}
