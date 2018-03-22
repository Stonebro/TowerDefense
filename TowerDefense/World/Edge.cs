using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.World {
    /// Represents Edge (between vertices).
    public class Edge {

        // Destination Vertex.
        public Vertex dest;
        // Cost for travelling this Edge.
        public float cost;

       
        /// Edge constructor.
        public Edge(Vertex dest, float cost) {
            this.dest = dest;
            this.cost = cost;
        }
    }
}
