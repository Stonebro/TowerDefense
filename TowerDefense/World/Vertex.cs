using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;

namespace TowerDefense.World {
    public class Vertex {
        public BaseTile parentTile;
        public List<Edge> adj;
        public float distance;
        public Vertex previous;
        public bool scratch;

        public Vertex(BaseTile parent) {
            this.parentTile = parent;
            adj = new List<Edge>();
        }

        public void Reset() {
            distance = Graph.INFINITY;
            previous = null;
            scratch = false; 
        }

        public static void ResetVertex(Vertex vertex) {
            if(vertex != null)
            vertex.Reset();
        }
    }
}
