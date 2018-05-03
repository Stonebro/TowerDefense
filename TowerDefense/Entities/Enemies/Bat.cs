using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense.Entities.Enemies {
    class Bat : Enemy {

        Vector2D _velocity;
        //a normalized vector pointing in the direction the entity is heading. 
        Vector2D _heading;
        //a vector perpendicular to the heading vector
        Vector2D _side;
        double _mass;
        //the maximum speed this entity may travel at.
        double _maxSpeed;
        //the maximum force this entity can produce to power itself 
        //(think rockets and thrust)
        double _maxForce;
        //the maximum rate (radians per second)this vehicle can rotate         
        double _maxTurnRate;
        public Vector2D _pos;

        Vector2D moveTarget;

        public Bat(int waveBonus) {
            name = "Bat";
            health = 10 + (waveBonus * 2);
            size = 7;
            bounty = 2 + waveBonus;
            moveTarget = new Vector2D(100, 200);
            _heading = new Vector2D(0, 0);
            _maxTurnRate = 0.2;
            _maxForce = 0.2;
            _maxSpeed = 0.3;
            _velocity = new Vector2D(0, 0);
            pos = GameWorld.Instance.startTile.pos;
        }

        public bool RotateHeadingToFacePosition(Vector2D target) {
            Vector2D toTarget = Vector2D.Vec2DNormalize(target - _pos);

            //first determine the angle between the heading vector and the target
            double angle = Math.Acos(_heading.Dot(toTarget));

            //return true if the player is facing the target
            if (angle < 0.00001) return true;

            //clamp the amount to turn to the max turn rate
            if (angle > _maxTurnRate) angle = _maxTurnRate;

            //The next few lines use a rotation matrix to rotate the player's heading
            //vector accordingly
            C2DMatrix RotationMatrix = new C2DMatrix();

            //notice how the direction of rotation has to be determined when creating
            //the rotation matrix
            RotationMatrix.Rotate((float)angle * _heading.Sign(toTarget));
            RotationMatrix.TransformVector2Ds(ref _heading);
            RotationMatrix.TransformVector2Ds(ref _velocity);

            //finally recreate m_vSide
            _side = _heading.Perp();

            return false;
        }

        public void SetHeading(Vector2D new_heading) {
            if((new_heading.LengthSq() - 1.0) < 0.00001 == false) { 
                _heading = new_heading;

                //the side vector must always be perpendicular to the heading
                _side = _heading.Perp();
            }
        }

        public override void Update() {
            RotateHeadingToFacePosition(moveTarget);
            //MoveTo(moveTarget);
        }

        public void MoveTo(Vector2D target) {
            float deltaX = target.x - this._pos.x;
            bool deltaXPositive = deltaX >= 0;
            float deltaY = target.y - this._pos.y;
            bool deltaYPositive = deltaY >= 0;


            if (deltaX != 0) {
                if (deltaXPositive)
                    this.pos += new Vector2D(this.pos.x + 0.02f, this.pos.y);
                else // Move the enemy to the left.
                    this.pos += new Vector2D(this.pos.x - 0.02f, this.pos.y);
            }
            if (deltaY != 0) {
                if (deltaYPositive)
                    this.pos = new Vector2D(this.pos.x, this.pos.y + 0.02f);
                else
                    this.pos = new Vector2D(this.pos.x, this.pos.y - 0.02f);
            }
            //if (path.Current.x == this.pos.x && path.Current.y == this.pos.y && path != null)
            //    path.GoNext(); // Make the enemy go to the next waypoint (if there is one).

        }

        public override void Render(Graphics g) {
            g.FillRectangle(new SolidBrush(Color.Black), new RectangleF(pos.x + ((BaseTile.size - size) / 2), pos.y + ((BaseTile.size - size) / 2), size, size));
        }
    }
}
