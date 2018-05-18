using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.World;

namespace TowerDefense.Util.Steering
{
    public class Vehicle : FlyingEntity 
    {
        public double TimeElapsed;
        public bool Tag;
        public GameWorld World = GameWorld.Instance; 
        public Vehicle(Vector2D pos, Vector2D scale, Vector2D velocity, Vector2D heading, double radius, double mass, double maxSpeed, double maxForce, double maxTurnRate) 
            : base(pos,scale,velocity,heading,radius,mass,maxSpeed,maxForce, maxTurnRate)
        {

        }
    }
}
