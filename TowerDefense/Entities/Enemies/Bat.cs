using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.Util.SteeringBehaviours;
using TowerDefense.World;

namespace TowerDefense.Entities.Enemies {
    class Bat : Enemy {

        private List<ISteering> steeringForces;
        public ISteering addForce {
            set {
                if (!steeringForces.Contains(value))
                    steeringForces.Add(value);
            }
        }
        //public Vector2D velocity;
        //a normalized vector pointing in the direction the entity is heading. 
        public Vector2D heading;
        //a vector perpendicular to the heading vector
        public Vector2D side;
        
        public float mass;
        //the maximum speed this entity may travel at.
        public float maxSpeed;
        //the maximum force this entity can produce to power itself 
        //(think rockets and thrust)
        public float maxForce;
        //the maximum rate (radians per second)this vehicle can rotate         
        public float maxTurnRate;
        public Vector2D oldPos;

        Vector2D moveTarget;

        public Bat(int waveBonus) {
            name = "Bat";
            health = 10 + (waveBonus * 2);
            size = 7;
            bounty = 2 + waveBonus;
            moveTarget = new Vector2D(100, 200);
            heading = Vector2D.Up;
            maxTurnRate = 0.05f;
            maxForce = 10f;
            maxSpeed = 80f;
            velocity = new Vector2D(0, 0);
            pos = GameWorld.Instance.startTile.pos;
            steeringForces = new List<ISteering>();
        }

        public bool IsSpeedMaxedOut() {
            return maxSpeed * maxSpeed > velocity.LengthSq();
        }

        public float Speed() {
            return velocity.Length();
        }

        public float SpeedSq() {
            return velocity.LengthSq();
        }


        public void SetHeading(Vector2D newheading) {
            if (newheading.LengthSq() - 1 >= 0.00001) {
                heading = newheading;
                side = heading.Perp();
            }
        }

        public bool RotateHeadingToFacePosition(Vector2D target) {
            Vector2D toTarget = Vector2D.Vec2DNormalize(target - pos);

            double angle = Math.Acos(heading.Dot(toTarget));
            Console.WriteLine(heading + "   " + side);

            if (angle < 0.1) return true;

            if (angle > maxTurnRate) angle = maxTurnRate;

            C2DMatrix RotationMatrix = new C2DMatrix();

            RotationMatrix.Rotate((float)angle * heading.Sign(toTarget));
            RotationMatrix.TransformVector2Ds(ref heading);
            RotationMatrix.TransformVector2Ds(ref velocity);

            side = heading.Perp();
            Console.WriteLine(heading + "   " + side);
            return false;
        }

        public override void Update() {
        }
        public override void Update(float time_elapsed) {
            if(path.Count > 0) {
                if (pos.Distance(path.Current) < 1)
                    if (!path.GoNext()) 
                        return;


                Vector2D target = Vector2D.Zero;
                foreach(ISteering force in steeringForces) {
                    target += force.ApplySteering(this);
                    if (target.Length() >= maxForce) break;
                }

                target += pos;

                if(RotateHeadingToFacePosition(target)) {
                    oldPos = pos;
                    pos += heading * maxSpeed * time_elapsed;
                }
            }
        }

        public override void Render(Graphics g) {
            g.FillRectangle(new SolidBrush(Color.Black), new RectangleF(pos.x + ((BaseTile.size - size) / 2), pos.y + ((BaseTile.size - size) / 2), size, size));
        }
    }
}
