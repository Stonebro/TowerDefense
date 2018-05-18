﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.World;

namespace TowerDefense.Util.Steering
{
    class SteeringBehaviour
    {
        #region Consts
        // The radius of the constraining circle for the wander behavior.
        private const double WanderRad = 1.2;
        // Distance the wander circle is projected in front of the agent.
        private const double WanderDist = 2.0;
        // The maximum amount of displacement along the circle each frame.
        private const double WanderJitterPerSec = 80.0;
        // Used in path following.
        private const double WaypointSeekDist = 20;
        // Used for making an agent only flee if the agent to flee from is within "panic distance".
        private const bool _fleePanicDistanceOn = false;
        // Used for setting the panic distance.
        private const double _panicDistanceSQ = 100 * 100;
        // Used for making an agent only evade if the agent to evade is within "threat distance".
        private const bool _evadeThreatRangeOn = false;
        // Used for setting the maximum distance from which a evading agent will consider a pursuing agent a threat.
        private const double _evadeThreatRange = 100;
        // If cell space partioning should be used.
        private const bool _cellSpaceOn = false;
        #endregion

        #region Enums
        public enum SumMethod { WEIGHTED_AVG, PRIORITIZED, DITHERED }
        public SumMethod SummingMethod;
        [Flags]
        public enum BehaviourType
        {
            NONE = 0,
            SEEK = 1,
            FLEE = 2,
            ARRIVE = 4,
            WANDER = 8,
            COHESION = 16,
            SEPARATION = 32,
            ALIGNMENT = 64,
            OBSTACLEAVOIDANCE = 128,
            WALLAVOIDANCE = 256,
            FOLLOWPATH = 512,
            PURSUIT = 1024,
            EVADE = 2048,
            INTERPOSE = 4096,
            HIDE = 8192,
            FLOCK = 16384,
            OFFSETPURSUIT = 32768,
        }

        public BehaviourType behaviours;

        /* Arrive makes use of this enum to determine how quickly a vehicle
           should decelerate to its target*/
        public enum DecelerationRate { SLOW = 3, NORMAL = 2, FAST = 1 };
        private DecelerationRate decelerationRate;
        #endregion

        private Vehicle _vehicle;
        private Vector2D _steeringForce;
        private Vehicle _targetAgent1;
        private Vehicle _targetAgent2;
        private Vector2D _target;

        private double _dBoxLength = 40;

        private List<Vector2D> _feelers;
        private double _wallDetectionFeelerLength = 40;

        private Vector2D _wanderTarget;
        private double _wanderJitter;
        private double _wanderRadius;
        private double _wanderDistance;

        // Used for setting the weights of behaviours when Prioritization is used.
        private double _weightSeparation = 1;
        private double _weightCohesion = 2;
        private double _weightAlignment = 1;
        private double _weightWander = 1;
        private double _weightObstacleAvoidance = 10;
        private double _weightWallAvoidance = 10;
        private double _weightSeek = 1;
        private double _weightFlee = 1;
        private double _weightArrive = 1;
        private double _weightPursuit = 1;
        private double _weightOffsetPursuit = 1;
        private double _weightInterpose = 1;
        private double _weightHide = 1;
        private double _weightEvade = 0.01;
        private double _weightFollowPath = 0.05;

        // Used for setting the probability that a steering behaviour will be used when Prioritzed dithering is used.
        private float _chanceWallAvoidance;
        private float _chanceObstacleAvoidance;
        private float _chanceSeparation;
        private float _chanceAlignment;
        private float _chanceCohesion;
        private float _chanceWander;
        private float _chanceSeek;
        private float _chanceFlee;
        private float _chanceEvade;
        private float _chanceHide;
        private float _chanceArrive;

        private int _flags;
        private double _viewDistance = 50;
        private Path _path;
        private Vector2D _offset;
        private double _waypointSeekDistSq;
        private double theta = new Random().NextDouble() * (Math.PI * 2);

        public SteeringBehaviour(Vehicle agent)
        {
            _vehicle = agent;
            _targetAgent1 = null;
            _targetAgent2 = null;
            _feelers = new List<Vector2D>();
            _wanderDistance = WanderDist;
            _wanderJitter = WanderJitterPerSec;
            _wanderRadius = WanderRad;
            _wanderTarget = new Vector2D(_wanderRadius * Math.Cos(theta), _wanderRadius * Math.Sin(theta));
            _flags = 0;
            decelerationRate = DecelerationRate.NORMAL;
            SummingMethod = SumMethod.WEIGHTED_AVG;
            _path = new Path();
            _path.Looped = true;

        }

        /// <summary>
        ///  Calculates the accumulated steering force according to the method set in SummingMethod.
        /// </summary> 
        public Vector2D Calculate()
        {
            // Resets the steering force.
            _steeringForce = Vector2D.Zero;

            /* Use space partitioning to calculate the neighbours of this vehicle
               if switched on. If not, use the standard tagging system */

            // Tag neighbors if any of the following 3 group behaviors are switched on.
            if (On(BehaviourType.SEPARATION) || On(BehaviourType.ALIGNMENT) || On(BehaviourType.COHESION))
            {
                _vehicle.World.TagVehiclesWithinViewRange(_vehicle, _viewDistance);
            }



            switch (SummingMethod)
            {
                case SumMethod.WEIGHTED_AVG:

                    _steeringForce = CalculateWeightedSum();
                    break;

                case SumMethod.PRIORITIZED:

                    _steeringForce = CalculatePrioritized();
                    break;

                case SumMethod.DITHERED:

                    _steeringForce = CalculateDithered();
                    break;

                default: _steeringForce = Vector2D.Zero; break;

            }

            return _steeringForce;
        }

        public double ForwardComponent()
        {
            return _vehicle.Heading.Dot(_steeringForce);
        }

        public double SideComponent()
        {
            return _vehicle.Side.Dot(_steeringForce);
        }

        /// <summary>
        /// This function calculates how much of its max steering force the 
        /// vehicle has left to apply and then applies that amount of the
        /// force to add.
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="ForceToAdd"></param>
        /// <returns></returns>
        bool AccumulateForce(Vector2D RunningTot, Vector2D ForceToAdd)
        {

            //calculate how much steering force the vehicle has used so far
            double MagnitudeSoFar = RunningTot.Length();

            //calculate how much steering force remains to be used by this vehicle
            double MagnitudeRemaining = _vehicle.MaxForce - MagnitudeSoFar;

            //return false if there is no more force left to use
            if (MagnitudeRemaining <= 0.0) return false;

            //calculate the magnitude of the force we want to add
            double MagnitudeToAdd = ForceToAdd.Length();

            /* If the magnitude of the sum of ForceToAdd and the running total
               does not exceed the maximum force available to this vehicle, just
               add together. Otherwise add as much of the ForceToAdd vector is
               possible without going over the max. */
            if (MagnitudeToAdd < MagnitudeRemaining)
            {
                RunningTot += ForceToAdd;
            }

            else
            {
                //
                Vector2D maxForceLeft = ForceToAdd * MagnitudeRemaining;
                maxForceLeft.Normalize();
                RunningTot += maxForceLeft;
            }
            return true;
        }

        /// <summary>
        /// This method calls each active steering behavior in order of priority
        /// and acumulates their forces until the max steering force magnitude
        /// is reached, at which time the function returns the steering force 
        /// accumulated to that point.
        /// </summary>
        /// <returns>Steering Force</returns>
        public Vector2D CalculatePrioritized()
        {
            Vector2D force = Vector2D.Zero;

            //if (On(BehaviourType.OBSTACLEAVOIDANCE))
            //{
            //    force = ObstacleAvoidance(_vehicle.World->Obstacles()) *
            //            _weightObstacleAvoidance;

            //    if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            //}

            if (On(BehaviourType.EVADE))
            {
                if (_targetAgent1 != null)
                {

                    force = Evade(_targetAgent1) * _weightEvade;
                }
                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviourType.FLEE))
            {
                force = Flee(_vehicle.World.Crosshair()) * _weightFlee;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }



            //these next three can be combined for flocking behavior (wander is
            //also a good behavior to add into this mix)

            if (On(BehaviourType.SEPARATION))
            {
                force = Separation(_vehicle.World.Vehicles) * _weightSeparation;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviourType.ALIGNMENT))
            {
                force = Alignment(_vehicle.World.Vehicles) * _weightAlignment;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviourType.COHESION))
            {
                force = Cohesion(_vehicle.World.Vehicles) * _weightCohesion;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }




            if (On(BehaviourType.SEEK))
            {
                force = Seek(_vehicle.World.Crosshair()) * _weightSeek;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }


            if (On(BehaviourType.ARRIVE))
            {
                force = Arrive(_vehicle.World.Crosshair(), decelerationRate) * _weightArrive;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviourType.WANDER))
            {
                force = Wander() * _weightWander;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviourType.PURSUIT))
            {
                if (_targetAgent1 != null)
                {
                    force = Pursuit(_targetAgent1) * _weightPursuit;
                }

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviourType.OFFSETPURSUIT))
            {
                if (_targetAgent1 != null && _offset != null)
                {
                    force = OffsetPursuit(_targetAgent1, _offset);
                }

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviourType.INTERPOSE))
            {
                if (_targetAgent1 != null && _targetAgent2 != null)
                {

                    force = Interpose(_targetAgent1, _targetAgent2) * _weightInterpose;
                }

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }


            if (On(BehaviourType.FOLLOWPATH))
            {
                force = FollowPath() * _weightFollowPath;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            return _steeringForce;
        }

        /// <summary>
        /// This simply sums up all the active behaviors X their weights and 
        /// truncates the result to the max available steering force before 
        /// returning.
        /// </summary>
        /// <returns>Steering Force</returns>
        public Vector2D CalculateWeightedSum()
        {

            //if (On(BehaviourType.OBSTACLEAVOIDANCE))
            //{
            //    _steeringForce += ObstacleAvoidance(_vehicle.World->Obstacles()) *
            //            _weightObstacleAvoidance;
            //}

            if (On(BehaviourType.EVADE))
            {
                if (_targetAgent1 != null)
                {
                    _steeringForce += Evade(_targetAgent1) * _weightEvade;
                }
            }


            if (On(BehaviourType.SEPARATION))
            {
                _steeringForce += Separation(_vehicle.World.Vehicles) * _weightSeparation;
            }

            if (On(BehaviourType.ALIGNMENT))
            {
                _steeringForce += Alignment(_vehicle.World.Vehicles) * _weightAlignment;
            }

            if (On(BehaviourType.COHESION))
            {
                _steeringForce += Cohesion(_vehicle.World.Vehicles) * _weightCohesion;
            }




            if (On(BehaviourType.WANDER))
            {
                _steeringForce += Wander() * _weightWander;
            }

            if (On(BehaviourType.SEEK))
            {
                _steeringForce += Seek(_vehicle.World.Crosshair()) * _weightSeek;
            }

            if (On(BehaviourType.FLEE))
            {
                _steeringForce += Flee(_vehicle.World.Crosshair()) * _weightFlee;
            }

            if (On(BehaviourType.ARRIVE))
            {
                _steeringForce += Arrive(_vehicle.World.Crosshair(), decelerationRate) * _weightArrive;
            }

            if (On(BehaviourType.PURSUIT))
            {
                if (_targetAgent1 != null)
                {
                    _steeringForce += Pursuit(_targetAgent1) * _weightPursuit;
                }
            }

            if (On(BehaviourType.OFFSETPURSUIT))
            {
                if (_targetAgent1 != null && _offset != null)
                {
                    _steeringForce += OffsetPursuit(_targetAgent1, _offset) * _weightOffsetPursuit;
                }
            }

            if (On(BehaviourType.INTERPOSE))
            {
                if (_targetAgent1 != null && _targetAgent2 != null) _steeringForce += Interpose(_targetAgent1, _targetAgent2) * _weightInterpose;
            }


            if (On(BehaviourType.FOLLOWPATH))
            {
                _steeringForce += FollowPath() * _weightFollowPath;
            }

            _steeringForce.Truncate((float)_vehicle.MaxForce);

            return _steeringForce;
        }

        /// <summary>
        /// This method sums up the active behaviors by assigning a probabilty
        /// of being calculated to each behavior. It then tests the first priority
        /// to see if it should be calcukated this simulation-step. If so, it
        /// calculates the steering force resulting from this behavior. If it is
        /// more than zero it returns the force. If zero, or if the behavior is
        /// skipped it continues onto the next priority, and so on.
        /// </summary>
        /// <returns>Steering Force</returns>
        public Vector2D CalculateDithered()
        {
            //reset the steering force
            _steeringForce = Vector2D.Zero;

            //if (On(BehaviourType.OBSTACLEAVOIDANCE) && new Random().NextDouble() < _chanceObstacleAvoidance)
            //{
            //    _steeringForce += ObstacleAvoidance(_vehicle.World->Obstacles()) *
            //            _weightObstacleAvoidance / _chanceObstacleAvoidance;

            //    if (!_steeringForce.isZero())
            //    {
            //        _steeringForce.Truncate((float)_vehicle.MaxForce);

            //        return _steeringForce;
            //    }
            //}


            if (On(BehaviourType.SEPARATION) && new Random().NextDouble() < _chanceSeparation)
            {
                _steeringForce += Separation(_vehicle.World.Vehicles) *
                                    _weightSeparation / _chanceSeparation;

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate((float)_vehicle.MaxForce);

                    return _steeringForce;
                }
            }




            if (On(BehaviourType.FLEE) && new Random().NextDouble() < _chanceFlee)
            {
                _steeringForce += Flee(_vehicle.World.Crosshair()) * _weightFlee / _chanceFlee;

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate((float)_vehicle.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviourType.EVADE) && new Random().NextDouble() < _chanceEvade)
            {
                if (_targetAgent1 != null) _steeringForce += Evade(_targetAgent1) * _weightEvade / _chanceEvade;

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate((float)_vehicle.MaxForce);

                    return _steeringForce;
                }
            }



            if (On(BehaviourType.ALIGNMENT) && new Random().NextDouble() < _chanceAlignment)
            {
                _steeringForce += Alignment(_vehicle.World.Vehicles) *
                                    _weightAlignment / _chanceAlignment;

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate((float)_vehicle.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviourType.COHESION) && new Random().NextDouble() < _chanceCohesion)
            {
                _steeringForce += Cohesion(_vehicle.World.Vehicles) *
                                    _weightCohesion / _chanceCohesion;

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate((float)_vehicle.MaxForce);

                    return _steeringForce;
                }
            }



            if (On(BehaviourType.WANDER) && new Random().NextDouble() < _chanceWander)
            {
                _steeringForce += Wander() * _weightWander / _chanceWander;

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate((float)_vehicle.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviourType.SEEK) && new Random().NextDouble() < _chanceSeek)
            {
                _steeringForce += Seek(_vehicle.World.Crosshair()) * _weightSeek / _chanceSeek;

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate((float)_vehicle.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviourType.ARRIVE) && new Random().NextDouble() < _chanceArrive)
            {
                _steeringForce += Arrive(_vehicle.World.Crosshair(), decelerationRate) *
                                    _weightArrive / _chanceArrive;

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate((float)_vehicle.MaxForce);

                    return _steeringForce;
                }
            }

            return _steeringForce;
        }

        #region Behaviours

        /// <summary>
        /// Given a target, this behavior returns a steering force which will
        /// direct the agent towards the target
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <returns>Steering Force</returns>
        public Vector2D Seek(Vector2D TargetPos)
        {
            Vector2D DesiredVelocity = Vector2D.Normalize(TargetPos - _vehicle.Pos)
                                      * _vehicle.MaxSpeed;

            return (DesiredVelocity - _vehicle.Velocity);
        }

        /// <summary>
        /// Given a target, this behavior returns a steering force which will
        /// direct the agent away from the target.
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <returns>Steering Force</returns>
        public Vector2D Flee(Vector2D TargetPos)
        {
            /* Only flee if agent to flee from if within specified distance. This block will be seen as unreachable code by the IDE unless the FleePanicDistanceOn bool is set to true.
             * Which makes sense. */
            if (_fleePanicDistanceOn)
            {
                float distanceSq = Vector2D.Vec2DDistanceSq(_vehicle.Pos, TargetPos);

                if (distanceSq > _panicDistanceSQ) return Vector2D.Zero;
            }

            Vector2D DesiredVelocity = Vector2D.Normalize(_vehicle.Pos - TargetPos)
                                      * _vehicle.MaxSpeed;

            return (DesiredVelocity - _vehicle.Velocity);
        }

        /// <summary>
        /// This behaviour will give return a steering force that will which will direct the agent to the TargetPosition.
        /// Unlike seek, this behaviour will decelerate when the the agent is close to the targetPosition. 
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <param name="deceleration"></param>
        /// <returns>Steering Force</returns>
        public Vector2D Arrive(Vector2D TargetPos,
                                          DecelerationRate deceleration)
        {
            Vector2D ToTarget = TargetPos - _vehicle.Pos;

            // Calculate the distance to the target.
            double dist = ToTarget.Length();

            if (dist > 0)
            {
                /* Because Deceleration is enumerated as an int, this value is required
                to provide fine tweaking of the deceleration. */
                const double DecelerationTweaker = 0.3;

                /* Calculate the speed required to reach the target given the desired
                deceleration. */
                double speed = dist / ((double)deceleration * DecelerationTweaker);

                // Make sure the velocity does not exceed the max.
                speed = Math.Min(speed, _vehicle.MaxSpeed);

                /* From here proceed just like Seek except we don't need to normalize 
                   the ToTarget vector because we have already gone to the trouble
                   of calculating its length: dist. */
                Vector2D DesiredVelocity = ToTarget * speed / dist;

                return (DesiredVelocity - _vehicle.Velocity);
            }

            return new Vector2D(0, 0);
        }

        /// <summary>
        /// This behaviour returns a force that will steer the agent towards to agent thats trying to evade the pursuing agent.
        /// </summary>
        /// <returns>Steering Force</returns>
        public Vector2D Pursuit(Vehicle evader)
        {
            /* If the evader is ahead and facing the agent then we can just seek
               for the evader's current position. */
            Vector2D ToEvader = evader.Pos - _vehicle.Pos;

            double RelativeHeading = _vehicle.Heading.Dot(evader.Heading);

            if ((ToEvader.Dot(_vehicle.Heading) > 0) &&
                 (RelativeHeading < -0.95))  // Acos(0.95)= 18 degrees
            {
                return Seek(evader.Pos);
            }

            // If this agent isn't ahead the position of the evader will be predicted.

            /* The lookahead time is propotional to the distance between the evader
               and the pursuer; and is inversely proportional to the sum of the
               agent's velocities */
            double LookAheadTime = ToEvader.Length() /
                                  (_vehicle.MaxSpeed + evader.Speed());

            // Now seek to the predicted future position of the evader.
            return Seek(evader.Pos + evader.Velocity * LookAheadTime);
        }

        /// <summary>
        /// Returns a steering force which directs the agent away from the pursuer.
        /// </summary>
        /// <param name="pursuer"></param>
        /// <returns>Steering Force</returns>
        public Vector2D Evade(Vehicle pursuer)
        {
            Vector2D ToPursuer = pursuer.Pos - _vehicle.Pos;

            if (_evadeThreatRangeOn)
            {
                if (ToPursuer.LengthSq() > _evadeThreatRange * _evadeThreatRange) return Vector2D.Zero;
            }

            /* The lookahead time is propotional to the distance between the pursuer
             * and the pursuer; and is inversely proportional to the sum of the
             * agents' velocities. */
            double LookAheadTime = ToPursuer.Length() /
                                   (_vehicle.MaxSpeed + pursuer.Speed());

            // Now flee away from predicted future position of the pursuer.
            return Flee(pursuer.Pos + pursuer.Velocity * LookAheadTime);
        }

        /// <summary>
        /// This behavior makes the agent wander about randomly.
        /// </summary>
        /// <returns>Steering Force</returns>
        public Vector2D Wander()
        {
            /* This behavior is dependent on the update rate, so this line must
            be included when using time independent framerate. */
            double JitterThisTimeSlice = _wanderJitter * _vehicle.TimeElapsed;
            double randomClamped = new Random().NextDouble() - new Random().NextDouble();

            // First, add a small random vector to the target's position.
            _wanderTarget += new Vector2D(randomClamped * JitterThisTimeSlice,
                                        randomClamped * JitterThisTimeSlice);

            // Reproject this new vector back on to a unit circle.
            _wanderTarget.Normalize();

            // Increase the length of the vector to the same as the radius
            // of the wander circle.
            _wanderTarget *= _wanderRadius;

            // Move the target into a position WanderDist in front of the agent.
            Vector2D target = _wanderTarget + new Vector2D(_wanderDistance, 0);

            // Project the target into world space.
            Vector2D Target = GameWorld.PointToWorldSpace(target,
                                                 _vehicle.Heading,
                                                 _vehicle.Side,
                                                 _vehicle.Pos);

            // And steer towards it.
            return Target - _vehicle.Pos;
        }

        ///// <summary>
        ///// Given a list of Obstacles, this method returns a steering force
        ///// that will prevent the agent colliding with the closest obstacle
        ///// </summary>
        ///// <returns>Steering Force</returns>
        //public Vector2D ObstacleAvoidance(List<Entity> obstacles)
        //{
        //    // The detection box length is proportional to the agent's velocity.
        //    _dBoxLength = _dBoxLength +
        //                     (_vehicle.Speed() / _vehicle.MaxSpeed) *
        //                     _dBoxLength;

        //    // Tag all obstacles within range of the box for processing.
        //    _vehicle.World->TagObstaclesWithinViewRange(_vehicle, _dBoxLength);

        //    // This will keep track of the closest intersecting obstacle (CIB).
        //    Entity ClosestIntersectingObstacle = null;

        //    // This will be used to track the distance to the CIB.
        //    double DistToClosestIP = Double.MaxValue;

        //    // This will record the transformed local coordinates of the CIB.
        //    Vector2D LocalPosOfClosestObstacle = null;

        //    foreach (Entity obstacle in obstacles)
        //    {
        //        // If the obstacle has been tagged within range proceed.
        //        if (obstacle.IsTagged())
        //        {
        //            // Calculate this obstacle's position in local space.
        //            Vector2D LocalPos = PointToLocalSpace(obstacle.Pos,
        //                                                   _vehicle.Heading,
        //                                                   _vehicle.Side,
        //                                                   _vehicle.Pos);

        //            /* If the local position has a negative x value then it must lay
        //               behind the agent. (in which case it can be ignored) */
        //            if (LocalPos.x >= 0)
        //            {
        //                /* If the distance from the x axis to the object's position is less
        //                 * than its radius + half the width of the detection box then there
        //                 * is a potential intersection. */
        //                double ExpandedRadius = obstacle.BRadius() + _vehicle.BRadius();

        //                if (Math.Abs(LocalPos.y) < ExpandedRadius)
        //                {
        //                    /* Now to do a line/circle intersection test. The center of the 
        //                     * circle is represented by (cX, cY). The intersection points are 
        //                     * given by the formula x = cX +/-sqrt(r^2-cY^2) for y=0. 
        //                     * Only the smallest possible value of x needs to be used because that is
        //                     * the smallest point of intersection. */
        //                    double cX = LocalPos.x;
        //                    double cY = LocalPos.y;

        //                    // The sqrt part of the above equation needs to be calculated only once.
        //                    double SqrtPart = Math.Sqrt(ExpandedRadius * ExpandedRadius - cY * cY);

        //                    double ip = cX - SqrtPart;

        //                    if (ip <= 0.0)
        //                    {
        //                        ip = cX + SqrtPart;
        //                    }

        //                    /* Test to see if this is the closest so far. If it is keep a
        //                     * record of the obstacle and its local coordinates. */
        //                    if (ip < DistToClosestIP)
        //                    {
        //                        DistToClosestIP = ip;

        //                        ClosestIntersectingObstacle = obstacle;

        //                        LocalPosOfClosestObstacle = LocalPos;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    /* If we have found an intersecting obstacle, calculate a steering 
        //     * force away from it */
        //    Vector2D SteeringForce = Vector2D.Zero;

        //    if (ClosestIntersectingObstacle != null)
        //    {
        //        /* The closer the agent is to an object, the stronger the 
        //           steering force should be. */
        //        double multiplier = 1.0 + (_dBoxLength - LocalPosOfClosestObstacle.x) /
        //                           _dBoxLength;

        //        // Calculate the lateral force.
        //        SteeringForce.y = (ClosestIntersectingObstacle.BRadius() -
        //                                       LocalPosOfClosestObstacle.y) * multiplier;

        //        /* Apply a braking force proportional to the obstacles distance from
        //         * the vehicle. */
        //        const double BrakingWeight = 0.2;

        //        SteeringForce.x = (ClosestIntersectingObstacle.BRadius() -
        //                                       LocalPosOfClosestObstacle.x) *
        //                                       BrakingWeight;
        //    }

        //    // Finally, convert the steering vector from local to world space.
        //    return VectorToWorldSpace(SteeringForce,
        //                              _vehicle.Heading,
        //                              _vehicle.Side);
        //}


        //        /// <summary>
        //        /// Returns a steering force that will direct the agent away from walls.
        //        /// </summary>
        //        /// <param name="walls"></param>
        //        /// <returns>Steering Force</returns>
        //        public Vector2D WallAvoidance(List<Wall> walls)
        //{
        //        //the feelers are contained in a std::vector, _feelers
        //        CreateFeelers();

        //        double DistToThisIP = 0.0;
        //        double DistToClosestIP = Double.MaxValue;

        //        //this will hold an index into the vector of walls
        //        int ClosestWall = -1;

        //        Vector2D SteeringForce,
        //                  point,         //used for storing temporary info
        //                  ClosestPoint;  //holds the closest intersection point

        //    //examine each feeler in turn
        //    for (int i = 0; i<_feelers.Count; ++i)
        //    {
        //        //run through each wall checking for any intersection points
        //        for (int j = 0; j <walls.Count; ++j)
        //        {
        //            if (LineIntersection2D(_vehicle.Pos,
        //                                   _feelers[i],
        //                                   walls[j].From(),
        //                                   walls[j].To(),
        //                                   DistToThisIP,
        //                                   point))
        //            {
        //                //is this the closest found so far? If so keep a record
        //                if (DistToThisIP<DistToClosestIP)
        //                {
        //                    DistToClosestIP = DistToThisIP;

        //                    ClosestWall = w;

        //                    ClosestPoint = point;
        //                }
        //}
        //        }//next wall


        //        //if an intersection point has been detected, calculate a force  
        //        //that will direct the agent away
        //        if (ClosestWall >= 0)
        //        {
        //            //calculate by what distance the projected position of the agent
        //            //will overshoot the wall
        //            Vector2D OverShoot = _feelers[i] - ClosestPoint;

        ////create a force in the direction of the wall normal, with a 
        ////magnitude of the overshoot
        //SteeringForce = walls[ClosestWall].Normal() * OverShoot.Length();
        //        }

        //    }//next feeler

        //    return SteeringForce;
        //}

        ////------------------------------- CreateFeelers --------------------------
        ////
        ////  Creates the antenna utilized by WallAvoidance
        ////------------------------------------------------------------------------
        //void CreateFeelers()
        //{
        //    //feeler pointing straight in front
        //    _feelers[0] = _vehicle.Pos + _wallDetectionFeelerLength * _vehicle.Heading;

        //    //feeler to left
        //    Vector2D temp = _vehicle.Heading;
        //    Vec2DRotateAroundOrigin(temp, HalfPi * 3.5f);
        //    _feelers[1] = _vehicle.Pos + _wallDetectionFeelerLength / 2.0f * temp;

        //    //feeler to right
        //    temp = _vehicle.Heading;
        //    Vec2DRotateAroundOrigin(temp, HalfPi * 0.5f);
        //    _feelers[2] = _vehicle.Pos + _wallDetectionFeelerLength / 2.0f * temp;
        //}

        /// <summary>
        /// Returns a force that repels the agent from other agents.
        /// </summary>
        /// <param name="neighbors"></param>
        /// <returns>Steering Force</returns>
        public Vector2D Separation(List<Vehicle> neighbors)
        {
            Vector2D SteeringForce = Vector2D.Zero;

            for (int i = 0; i < neighbors.Count; ++i)
            {
                /* Make sure this agent isn't included in the calculations and that
                 * the agent being examined is close enough. 
                 * Also make sure it doesn't include the evade target */
                if ((neighbors[i] != _vehicle) && neighbors[i].Tag &&
                  (neighbors[i] != _targetAgent1))
                {
                    Vector2D ToAgent = _vehicle.Pos - neighbors[i].Pos;

                    /* Scale the force inversely proportional to the agents distance  
                       from its neighbor. */
                    SteeringForce += Vector2D.Normalize(ToAgent) / ToAgent.Length();
                }
            }

            return SteeringForce;
        }

        /// <summary>
        /// Returns a force that attempts to align agents heading with the heading of its neighbours.
        /// </summary>
        /// <param name="neighbors"></param>
        /// <returns></returns>
        Vector2D Alignment(List<Vehicle> neighbors)
        {
            // Used to record the average heading of the neighbors.
            Vector2D AverageHeading = Vector2D.Zero;

            // Used to count the number of vehicles in the neighborhood.
            int NeighborCount = 0;

            // Iterate through all the tagged vehicles and sum their heading vectors.
            for (int i = 0; i < neighbors.Count; ++i)
            {
                /* Make sure *this* agent isn't included in the calculations and that
                 * the agent being examined is close enough. 
                 * Also make sure it doesn't include any evade target */
                if ((neighbors[i] != _vehicle) && neighbors[i].Tag &&
                  (neighbors[i] != _targetAgent1))
                {
                    AverageHeading += neighbors[i].Heading;

                    ++NeighborCount;
                }
            }

            /* If the neighborhood contained one or more vehicles, average their
             * heading vectors. */
            if (NeighborCount > 0)
            {
                AverageHeading /= (double)NeighborCount;

                AverageHeading -= _vehicle.Heading;
            }

            return AverageHeading;
        }

        /// <summary>
        /// Returns a steering force that attempts to move the agent towards the
        /// center of mass of the agents in its immediate area.
        /// </summary>
        /// <param name="neighbors"></param>
        /// <returns>Steering Force</returns>
        Vector2D Cohesion(List<Vehicle> neighbors)
        {
            // First find the center of mass of all the agents.
            Vector2D CenterOfMass = Vector2D.Zero;
            Vector2D SteeringForce = Vector2D.Zero;

            int NeighborCount = 0;

            // Iterate through the neighbors and sum up all the position vectors.
            for (int i = 0; i < neighbors.Count; ++i)
            {
                /* Make sure this agent isn't included in the calculations and that
                 * the agent being examined is close enough. 
                 * Also make sure it doesn't include the evade target, */
                if ((neighbors[i] != _vehicle) && neighbors[i].Tag &&
                  (neighbors[i] != _targetAgent1))
                {
                    CenterOfMass += neighbors[i].Pos;

                    ++NeighborCount;
                }
            }

            if (NeighborCount > 0)
            {
                //the center of mass is the average of the sum of positions
                CenterOfMass /= (double)NeighborCount;

                //now seek towards that position
                SteeringForce = Seek(CenterOfMass);
            }

            //the magnitude of cohesion is usually much larger than separation or
            //allignment so it usually helps to normalize it.
            SteeringForce.Normalize();
            return SteeringForce;
        }

        /// <summary>
        /// Returns a steering force that makes the agent attempt to position itself between the two specified Vehicles.
        /// </summary>
        /// <param name="agentA"></param>
        /// <param name="agentB"></param>
        /// <returns>Steering Force</returns>
        public Vector2D Interpose(Vehicle agentA, Vehicle agentB)
        {
            /* First we need to figure out where the two agents are going to be at 
             * time T in the future. This is approximated by determining the time
             * taken to reach the mid way point at the current time at at max speed. */
            Vector2D MidPoint = (agentA.Pos + agentB.Pos) / 2.0;
            double TimeToReachMidPoint = Vector2D.Vec2DDistance(_vehicle.Pos, MidPoint) /
                                         _vehicle.MaxSpeed;

            /* Now we have T, we assume that agent A and agent B will continue on a
             * straight trajectory and extrapolate to get their future positions */
            Vector2D APos = agentA.Pos + agentA.Velocity * TimeToReachMidPoint;
            Vector2D BPos = agentB.Pos + agentB.Velocity * TimeToReachMidPoint;

            // Calculate the mid point of these predicted positions.
            MidPoint = (APos + BPos) / 2.0;

            // Then steer to Arrive at it.
            return Arrive(MidPoint, DecelerationRate.FAST);
        }



        /// <summary>
        /// Given a series of Vector2Ds, this method produces a force that will
        /// move the agent along the waypoints in order. The agent uses the
        /// 'Seek' behavior to move to the next waypoint - unless it is the last
        /// waypoint, in which case it 'Arrives'.
        /// </summary>
        /// <returns>Steering Force</returns>
        public Vector2D FollowPath()
        {
            /* Move to next target if close enough to current target (working in
             * distance squared space). */
            if (Vector2D.Vec2DDistanceSq(_path.Current, _vehicle.Pos) <
               _waypointSeekDistSq)
            {
                _path.GoNext();
            }

            if (!_path.IsFinished())
            {
                return Seek(_path.Current);
            }

            else
            {
                return Arrive(_path.Current, DecelerationRate.NORMAL);
            }
        }
        /// <summary>
        /// Produces a steering force that keeps a vehicle at a specified offset
        /// from a leader vehicle.
        /// </summary>
        /// <param name="leader"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Vector2D OffsetPursuit(Vehicle leader, Vector2D offset)
        {
            //calculate the offset's position in world space
            Vector2D WorldOffsetPos = GameWorld.PointToWorldSpace(offset,
                                                            leader.Heading,
                                                            leader.Side,
                                                            leader.Pos);

            Vector2D ToOffset = WorldOffsetPos - _vehicle.Pos;

            //the lookahead time is propotional to the distance between the leader
            //and the pursuer; and is inversely proportional to the sum of both
            //agent's velocities
            double LookAheadTime = ToOffset.Length() /
                                  (_vehicle.MaxSpeed + leader.Speed());

            //now Arrive at the predicted future position of the offset
            return Arrive(WorldOffsetPos + leader.Velocity * LookAheadTime, DecelerationRate.FAST);
        }



        public bool On(BehaviourType bt)
        {
            return (behaviours & bt) == bt;
        }
    }
}
#endregion