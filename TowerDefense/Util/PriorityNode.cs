using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util
{
    class PriorityNode<T>
    { 
        /// The node.
        public T node;
        /// The priority of the node.
        public float priority;
   
        /// PriorityNode constructor.
        public PriorityNode(T node, float priority)
        {
            this.node = node;
            this.priority = priority;
        }
    }
}
