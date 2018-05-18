using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Entities.Projectiles;
using TowerDefense.Tiles;
using TowerDefense.World;

namespace TowerDefense.Util {
    public class Path {
        // Waypoints along the path, these waypoints define the path.
        private List<Vector2D> waypoints;
        // Returns current waypoint if there is one, otherwise returns null.
        public Vector2D Current {
            get {
                if(waypoints != null) {
                    if (waypoints.Count > 0)
                        return waypoints[0];
                    else return null;
                }
                return null;
            }
        }
        // Returns last waypoint if there is one, otherwise returns null.
        public Vector2D End {
            get {
                if (waypoints != null)
                    if (waypoints.Count > 0)
                        return waypoints[waypoints.Count - 1];
                    else return null;
                return null;
            }
        }

        // Default/empty constructor
        public Path() : this(new List<Vector2D>()) { }

        // Creates path from list of vec2.
        public Path(List<Vector2D> path) {
            waypoints = path;
        }

        // Returns if path is finished.
        public bool IsFinished(){
            if (waypoints != null)
                return waypoints.Count == 0;
            else return false;
        }

        // Goes to next waypoint in path. Returns false if no next waypoint.
        public bool GoNext(){
            if(waypoints != null) {
                if (waypoints.Count > 0)
                    waypoints.RemoveAt(0);
                return waypoints.Count > 0;
            }
            return false;
        }
        
        // Adds (vec2) waypoint to the end of the list of waypoints.
        public void AddWaypoint(Vector2D waypoint){
            waypoints.Add(waypoint);
        }

        // Adds (vec2) position of vertex to the end of the list of waypoints.
        public void AddWaypoint(Vertex wayPointVertex){
            waypoints.Add(wayPointVertex.parentTile.pos);
        }

        // Adds (vec2) position of tile to the end of the list of waypoints.
        public void AddWaypoint(BaseTile waypointTile){
            waypoints.Add(waypointTile.pos);
        }

        // Adds (vec2) waypoint to the start of the list of waypoints.
        public void AddWaypointFront(Vector2D waypoint){
            waypoints.Insert(0, waypoint);
        }

        // Adds(vec2) position of vertex to the start of the list of waypoints.
        // // + new Vector2D(BaseTile.size / 2, BaseTile.size / 2)
        public void AddWaypointFront(Vertex waypointVertex) {
            waypoints.Insert(0, waypointVertex.parentTile.pos);
        }

        // Adds (vec2) position of tile to the start of the list of waypoints.
        public void AddWaypointFront(BaseTile waypointTile) {
            waypoints.Insert(0, waypointTile.pos);
        }

        // Draws the path and center it inside a Tile
        public void Render(Graphics g) {
            for (int i = 0; i < waypoints.Count - 1; i++) {
                g.DrawLine(new Pen(Color.FromArgb(128, 0, 255, 0), 1), waypoints[i].x+7, waypoints[i].y+7, waypoints[i + 1].x+7, waypoints[i + 1].y+7);
            }
        }

        // Returns if any of the Tiles in newTiles are placed onto the Path
        public bool IsBlocked(List<BaseTile> newTiles) {
            if (newTiles == null) return true;
            foreach(Vector2D waypoint in waypoints) {
                foreach(BaseTile tile in newTiles){   
                    if (waypoint == tile.pos) return true;
                }
            }
            return false;
        }

        // Path setter (from other Path).
        public void Set(Path path) {
            this.waypoints = path.waypoints;
        }

        // Clears Path.
        public void Clear() {
            waypoints.Clear();
        }

        // Returns a estimate of the distance in Tiles between 2 points.
        private static int Heuristics(Vector2D a, Vector2D b) 
            => (int)Math.Abs((a.x / BaseTile.size) - (b.x / BaseTile.size)) + (int)Math.Abs((a.y / BaseTile.size) - (b.y / BaseTile.size));

        // Gets a path between two tiles using A*, Manhattan Heuristics.
        public static Path GetPath(BaseTile startTile, BaseTile targetTile) {
            // Create Path to return
            Path path = new Path();

            PriorityQueue<Vertex> priorityQueue = new PriorityQueue<Vertex>();

            // Distance from startTile to startTile is always 0.
            startTile.vertex.distance = 0;
            // Makes the algoritmn know this Vertex is visited. 
            startTile.vertex.scratch = true;

            // Adds Vertex of startTile to queue (first Vertex to be visited).
            priorityQueue.Insert(startTile.vertex, Heuristics(startTile.pos, targetTile.pos));

            Vertex currentVertex;

            while (!priorityQueue.IsEmpty) {
                currentVertex = priorityQueue.GetHighestPriority();

                // Algorithm arrived at the target tile.
                if (currentVertex.parentTile.pos == targetTile.pos) {
                    Vertex tempVertex = currentVertex;
                    // While previous Vertex is not null.
                    while (tempVertex.previous != null) {
                        // Forms the shortest path by going through the previous Vertices.
                        path.AddWaypointFront(tempVertex);
                        tempVertex = tempVertex.previous;
                    }
                    // Returns the shortest path.
                    return path;
                }

                // Looks for neighbouring Vertices.
                foreach (Edge edge in currentVertex.adj) {
                    // Checks if there is not already a faster route to the destination vertex found.
                    if (edge.dest.distance >= currentVertex.distance + edge.cost) {
                        // Previous Vertex leading to the fastest route to the destination vertex is now the current vertex.
                        edge.dest.previous = currentVertex;
                        // Sets the distance of this neighbour to be the distance to the current Vertex + the cost. 
                        edge.dest.distance = currentVertex.distance + edge.cost;
                        // If the destination vertex hasn't been visited yet and it is not a disabled vertex.
                        if (!edge.dest.scratch && !edge.dest.disabled) {
                            // Set the Vertex to be visited.
                            edge.dest.scratch = true;
                            // Add the Vertex to the PriorityQueue with the priority being the (Manhattan) heuristic outcome + the distance to the vertex.
                            priorityQueue.Insert(edge.dest, (int)edge.dest.distance + Heuristics(edge.dest.parentTile.pos, targetTile.pos));
                        }
                    }
                }

            }
            // Simply return Path if it all goes wrong
            return path;
        }
    }
}
