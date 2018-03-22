using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.World {
    public class Edge {

        public Vertex dest;
        public float cost;

        public Edge(Vertex dest, float cost) {
            this.dest = dest;
            this.cost = cost;
        }
    }
}
