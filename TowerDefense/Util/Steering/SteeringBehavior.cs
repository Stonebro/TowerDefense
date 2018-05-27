using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Entities.Powerups;
using TowerDefense.World;

namespace TowerDefense.Util.Steering
{
    /// <summary>
    /// This is a partial rewrite of the Steeringbehaviours class by Mat Buckland (with a few extra functions added). All credit goes to him.
    /// </summary>
    public class SteeringBehavior
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
        public enum BehaviorType
        {
            NONE = 0,
            SEEK = 1,
            FLEE = 2,
            ARRIVE = 4,
            FOLLOWPATH = 8,
            PURSUIT = 16,
            EVADE = 32,
            OFFSETPURSUIT = 64,
            EXPLORE = 128
        }

        public BehaviorType behaviours;

        /* Arrive makes use of this enum to determine how quickly a FlyingEntity
           should decelerate to its target*/
        public enum DecelerationRate { SLOW = 3, NORMAL = 2, FAST = 1 };
        private DecelerationRate decelerationRate;
        #endregion

        #region Attributes
        public FlyingEntity TargetAgent1;
        public FlyingEntity TargetAgent2;
        private FlyingEntity _flyingEntity;
        private Vector2D _steeringForce = Vector2D.Zero;

        // Used for setting the weights of behaviours when Prioritization is used.

        private double _weightSeek = 1;
        private double _weightFlee = 1;
        private double _weightArrive = 1;
        private double _weightPursuit = 1;
        private double _weightOffsetPursuit = 1;
        private double _weightExplore = 1;
        private double _weightEvade = 0.01;
        private double _weightFollowPath = 0.05;

        // Used for setting the probability that a steering behaviour will be used when Prioritzed dithering is used.
        private float _chanceSeek = 0.8f;
        private float _chanceFlee = 0.6f;
        private float _chanceEvade = 1f;
        private float _chanceArrive = 0.5f;
        private float _chanceExplore = 0.3f;
        private float _chanceOffsetPursuit = 0.9f;
        private float _chancePursuit = 1;
        private float _chanceFollowPath = 0.5f;

        private Path _path;
        private int _offset = 20;

        private double theta = new Random().NextDouble() * (Math.PI * 2);
        #endregion

        public SteeringBehavior(FlyingEntity agent)
        {
            _flyingEntity = agent;
            TargetAgent1 = null;
            TargetAgent2 = null;
            decelerationRate = DecelerationRate.NORMAL;
            SummingMethod = SumMethod.WEIGHTED_AVG;
            _path = new Path
            {
                Looped = true
            };

        }
        #region Calculation
        /// <summary>
        ///  Calculates the accumulated steering force according to the method set in SummingMethod.
        /// </summary> 
        public Vector2D Calculate()
        {
            // Resets the steering force.
            _steeringForce = Vector2D.Zero;
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
            return _flyingEntity.Heading.Dot(_steeringForce);
        }

        public double SideComponent()
        {
            return _flyingEntity.Side.Dot(_steeringForce);
        }

        /// <summary>
        /// This function calculates how much of its max steering force the 
        /// vehicle has left to apply and then applies that amount of the
        /// force to add.
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="ForceToAdd"></param>
        /// <returns>If theres force left to use</returns>
        bool AccumulateForce(Vector2D RunningTot, Vector2D ForceToAdd)
        {

            //calculate how much steering force the vehicle has used so far
            double MagnitudeSoFar = RunningTot.Length();

            //calculate how much steering force remains to be used by this vehicle
            double MagnitudeRemaining = _flyingEntity.MaxForce - MagnitudeSoFar;

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

            if (On(BehaviorType.EVADE))
            {
                if (TargetAgent1 != null)
                {

                    force = Evade(TargetAgent1) * _weightEvade;
                }
                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviorType.FLEE))
            {
                force = Flee(_flyingEntity.World.Crosshair) * _weightFlee;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }


            if (On(BehaviorType.SEEK))
            {
                force = Seek(_flyingEntity.World.Crosshair) * _weightSeek;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }


            if (On(BehaviorType.ARRIVE))
            {
                force = Arrive(_flyingEntity.World.Crosshair, decelerationRate) * _weightArrive;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviorType.PURSUIT))
            {
                if (TargetAgent1 != null)
                {
                    force = Pursuit(TargetAgent1) * _weightPursuit;
                }

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviorType.FOLLOWPATH))
            {
                force = FollowPath() * _weightFollowPath;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviorType.EXPLORE))
            {
                if (_flyingEntity.goals.Count != 0)
                    force = Explore(_flyingEntity.goals) * _weightExplore;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(BehaviorType.OFFSETPURSUIT))
            {
                if (TargetAgent1 != null)
                {
                    force += OffsetPursuit(TargetAgent1, _offset) * _weightOffsetPursuit;
                }
                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }
            return force;
        }

        /// <summary>
        /// This simply sums up all the active behaviors X their weights and 
        /// truncates the result to the max available steering force before 
        /// returning.
        /// </summary>
        /// <returns>Steering Force</returns>
        public Vector2D CalculateWeightedSum()
        {
            if (On(BehaviorType.EVADE))
            {
                if (TargetAgent1 != null)
                {
                    _steeringForce += Evade(TargetAgent1) * _weightEvade;
                }
            }


            if (On(BehaviorType.SEEK))
            {
                _steeringForce += Seek(_flyingEntity.World.Crosshair) * _weightSeek;
            }

            if (On(BehaviorType.FLEE))
            {
                _steeringForce += Flee(_flyingEntity.World.Crosshair) * _weightFlee;
            }

            if (On(BehaviorType.ARRIVE))
            {
                _steeringForce += Arrive(_flyingEntity.World.Crosshair, decelerationRate) * _weightArrive;
            }

            if (On(BehaviorType.PURSUIT))
            {
                if (TargetAgent1 != null)
                {
                    _steeringForce += Pursuit(TargetAgent1) * _weightPursuit;
                }
            }

            if (On(BehaviorType.OFFSETPURSUIT))
            {
                if (TargetAgent1 != null)
                {
                    _steeringForce += OffsetPursuit(TargetAgent1, _offset) * _weightOffsetPursuit;
                }
            }
            if (On(BehaviorType.FOLLOWPATH))
            {
                _steeringForce += FollowPath() * _weightFollowPath;
            }

            if (On(BehaviorType.EXPLORE))
            {
                if (_flyingEntity.goals.Count != 0)
                    _steeringForce += Explore(_flyingEntity.goals) * _weightExplore;
            }


            _steeringForce.Truncate((float)_flyingEntity.MaxForce);

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

            if (On(BehaviorType.FLEE) && new Random().NextDouble() < _chanceFlee)
            {
                _steeringForce += Flee(_flyingEntity.World.Crosshair) * _weightFlee / _chanceFlee;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate((float)_flyingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviorType.EVADE) && new Random().NextDouble() < _chanceEvade)
            {
                if (TargetAgent1 != null) _steeringForce += Evade(TargetAgent1) * _weightEvade / _chanceEvade;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate((float)_flyingEntity.MaxForce);

                    return _steeringForce;
                }
            }


            if (On(BehaviorType.SEEK) && new Random().NextDouble() < _chanceSeek)
            {
                _steeringForce += Seek(_flyingEntity.World.Crosshair) * _weightSeek / _chanceSeek;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate((float)_flyingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviorType.ARRIVE) && new Random().NextDouble() < _chanceArrive)
            {
                _steeringForce += Arrive(_flyingEntity.World.Crosshair, decelerationRate) *
                                    _weightArrive / _chanceArrive;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate((float)_flyingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviorType.EXPLORE))
            {
                if (_flyingEntity.goals.Count != 0)
                    _steeringForce += Explore(_flyingEntity.goals) * _weightExplore / _chanceExplore;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate((float)_flyingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviorType.OFFSETPURSUIT) && new Random().NextDouble() < _chanceOffsetPursuit)
            {
                _steeringForce += OffsetPursuit(TargetAgent1, _offset) * _weightOffsetPursuit / _chanceOffsetPursuit;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate((float)_flyingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviorType.PURSUIT) && new Random().NextDouble() < _chancePursuit)
            {
                if (TargetAgent1 != null)
                {
                    _steeringForce += Pursuit(TargetAgent1) * _weightPursuit / _chancePursuit;
                }

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate((float)_flyingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(BehaviorType.FOLLOWPATH) && new Random().NextDouble() < _chanceFollowPath)
            {
                if (TargetAgent1 != null)
                {
                    _steeringForce += FollowPath() * _weightFollowPath / _chanceFollowPath;
                }

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate((float)_flyingEntity.MaxForce);

                    return _steeringForce;
                }
            }



            return _steeringForce;
        }

        /// <summary>
        /// Makes position of Vehicle and targetPosition equal if the difference between them is very small.
        /// </summary>
        /// <param name="flyingEntity"></param>
        /// <param name="targetPos"></param>
        public bool ClampPositions(ref FlyingEntity flyingEntity, Vector2D targetPos)
        {
            float deltaX = flyingEntity.Pos.x - targetPos.x;
            float deltaY = flyingEntity.Pos.y - targetPos.y;
            if (-10 < deltaX && deltaX < 10)
            {
                flyingEntity.Pos.x = targetPos.x;

            }
            if (-10 < deltaY && deltaY < 10)
            {
                flyingEntity.Pos.y = targetPos.y;
            };
            if ((-10 < deltaX && deltaX < 10) && (-10 < deltaY && deltaY < 10))
            {
                flyingEntity.Pos = targetPos;
                return true;
            };
            return false;
        }

        /// <summary>
        /// Returns if specified BehaviorType is set or not.
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public bool On(BehaviorType bt)
        {
            return (behaviours & bt) == bt;
        }

        #endregion
        #region Behaviours

        /// <summary>
        /// Explores queue of Powerups.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goals"></param>
        /// <returns>Steering force.</returns>
        public Vector2D Explore(Queue<Powerup> goals)
        {
            Vector2D goalPos;
            Vector2D steeringForce = Vector2D.Zero;

            // Check to see if there is a goal left.
            if (goals.Peek() != null)
            {
                // Draw the goal
                Powerup goal = goals.Peek();
                // Convert Rect pos to Vector.
                goalPos = new Vector2D(goal.Pos.X, goal.Pos.Y);
                // Calculate steering force to Arrive to goal.
                steeringForce = Arrive(goalPos, DecelerationRate.FAST);
                // If Flying Entity reached the goal. Go for the next goal.
                if (goalPos == this._flyingEntity.Pos)
                {
                    // Bounty isn't included in Powerup so if the goal is a Coin we need to cast to Coin to add the bounty.
                    if (goal is Coin)
                    {
                        Coin coin = goal as Coin;
                        GameWorld.Instance.AddGold(coin.bounty);
                    }
                    goals.Dequeue();
                }
            }

            return steeringForce;
        }

        /// <summary>
        /// Given a target, this behavior returns a steering force which will
        /// direct the agent towards the target
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <returns>Steering Force</returns>
        public Vector2D Seek(Vector2D TargetPos)
        {
            if (_flyingEntity.Pos.x == TargetPos.x && _flyingEntity.Pos.y == TargetPos.y) return Vector2D.Zero;

            if (ClampPositions(ref _flyingEntity, TargetPos))
            {
                return Vector2D.Zero;
            }

            Vector2D DesiredVelocity = Vector2D.Normalize(TargetPos - _flyingEntity.Pos)
                                      * _flyingEntity.MaxSpeed;
            // Console.WriteLine(DesiredVelocity - _flyingEntity.Velocity);
            return (DesiredVelocity - _flyingEntity.Velocity);
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
                float distanceSq = Vector2D.Vec2DDistanceSq(_flyingEntity.Pos, TargetPos);

                if (distanceSq > _panicDistanceSQ) return Vector2D.Zero;
            }

            Vector2D DesiredVelocity = Vector2D.Normalize(_flyingEntity.Pos - TargetPos)
                                      * _flyingEntity.MaxSpeed;

            return (DesiredVelocity - _flyingEntity.Velocity);
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
            if (_flyingEntity.Pos.x == TargetPos.x && _flyingEntity.Pos.y == TargetPos.y) return Vector2D.Zero;
            ClampPositions(ref _flyingEntity, TargetPos);

            Vector2D ToTarget = TargetPos - _flyingEntity.Pos;

            // Calculate the distance to the target.
            double dist = ToTarget.Length();

            if (dist > 0)
            {
                /* Because Deceleration is enumerated as an int, this value is required
                to provide fine tweaking of the deceleration. */
                const double DecelerationTweaker = 2.0;

                /* Calculate the speed required to reach the target given the desired
                deceleration. */
                double speed = dist / ((double)deceleration * DecelerationTweaker);
                // Make sure the velocity does not exceed the max.
                speed = Math.Min(speed, _flyingEntity.MaxSpeed);

                /* From here proceed just like Seek except we don't need to normalize 
                   the ToTarget vector because we have already gone to the trouble
                   of calculating its length: dist. */
                Vector2D DesiredVelocity = ToTarget * speed / dist;

                return (DesiredVelocity - _flyingEntity.Velocity);
            }

            return new Vector2D(0, 0);
        }

        /// <summary>
        /// This behaviour returns a force that will steer the agent towards to agent thats trying to evade the pursuing agent.
        /// </summary>
        /// <returns>Steering Force</returns>
        public Vector2D Pursuit(FlyingEntity evader)
        {
            /* If the evader is ahead and facing the agent then we can just seek
               for the evader's current position. */
            Vector2D ToEvader = evader.Pos - _flyingEntity.Pos;

            double RelativeHeading = _flyingEntity.Heading.Dot(evader.Heading);

            if ((ToEvader.Dot(_flyingEntity.Heading) > 0) &&
                 (RelativeHeading < -0.95))  // Acos(0.95)= 18 degrees
            {
                return Seek(evader.Pos);
            }

            // If this agent isn't ahead the position of the evader will be predicted.

            /* The lookahead time is propotional to the distance between the evader
               and the pursuer; and is inversely proportional to the sum of the
               agent's velocities */
            double LookAheadTime = ToEvader.Length() /
                                  (_flyingEntity.MaxSpeed + evader.Speed());

            // Now seek to the predicted future position of the evader.
            return Seek(evader.Pos + evader.Velocity * LookAheadTime);
        }

        /// <summary>
        /// Returns a steering force which directs the agent away from the pursuer.
        /// </summary>
        /// <param name="pursuer"></param>
        /// <returns>Steering Force</returns>
        public Vector2D Evade(FlyingEntity pursuer)
        {
            Vector2D ToPursuer = pursuer.Pos - _flyingEntity.Pos;

            if (_evadeThreatRangeOn)
            {
                if (ToPursuer.LengthSq() > _evadeThreatRange * _evadeThreatRange) return Vector2D.Zero;
            }

            /* The lookahead time is propotional to the distance between the pursuer
             * and the pursuer; and is inversely proportional to the sum of the
             * agents' velocities. */
            double LookAheadTime = ToPursuer.Length() /
                                   (_flyingEntity.MaxSpeed + pursuer.Speed());

            // Now flee away from predicted future position of the pursuer.
            return Flee(pursuer.Pos + pursuer.Velocity * LookAheadTime);
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
            ClampPositions(ref _flyingEntity, _path.Current);
            if (_path.Current == null) return Vector2D.Zero;
            _path.GoNext();
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
        /// Produces a steering force that keeps a FlyingEntity at a specified offset
        /// from a leader FlyingEntity.
        /// </summary>
        /// <param name="leader"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Vector2D OffsetPursuit(FlyingEntity leader, int offset)
        {
            return Arrive(leader.Pos - leader.Heading * offset, DecelerationRate.FAST);
        }
    }
}
#endregion