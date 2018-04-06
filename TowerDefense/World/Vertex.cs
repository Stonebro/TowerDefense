using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;

namespace TowerDefense.World {
    public class Vertex {
        // The Tile that corresponds to this Vertex.
        public BaseTile parentTile;
        // Edges that originate from this Vertex.
        public List<Edge> adj;
        // Distance from possible source to this Vertex.
        public float distance = Graph.INFINITY;
        // Previous Vertex in possible path.
        public Vertex previous;
        // Used for Dijkstra (if Vertex has been visited or not). 
        public bool scratch;

        /// Vertex constructor.
        public Vertex(BaseTile parent) {
            this.parentTile = parent;
            adj = new List<Edge>();
        }

        /// Resets Vertex.
        public void Reset() {
            distance = Graph.INFINITY;
            previous = null;
            scratch = false; 
        }

        /// Static function for resetting Vertex. 
        public static void ResetVertex(Vertex vertex) {
            if(vertex != null)
            vertex.Reset();
        }
    }
}
