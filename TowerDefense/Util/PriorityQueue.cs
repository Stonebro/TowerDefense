using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util
{
    public class PriorityQueue<T>
    {
        PriorityNode<T>[] heap;
        int size;
       
        /// Default constructor, initializes heap with size 1.
        public PriorityQueue() : this(1) { }
    
        /// Constuctor that initializes heap with specified size.
        public PriorityQueue(int heapSize)
        {
            heap = new PriorityNode<T>[heapSize];
            size = 0;
        }

        /// Percolates down from the start by calling recursive function below.
        public void PercolateDown()
        {
            PercolateDown(1);
        }

        /// Moves a PriorityNode down until its parent is smaller and both of its children are larger.
        private void PercolateDown(int i)
        {
            if (i * 2 > size)
                return;
            else if (i * 2 == size)
            {
                PercolateDownLeft(i);
                return;
            }

            int left = i * 2;
            int right = i * 2 + 1;
            int smallest = heap[left].priority > heap[right].priority ? right : left;

            if (heap[i].priority > heap[smallest].priority)
            {
                heap[0] = heap[i];
                heap[i] = heap[smallest];
                heap[smallest] = heap[0];

                if (smallest * 2 <= size)
                {
                    if (smallest * 2 + 1 <= size)
                        PercolateDown(smallest);
                    else
                        PercolateDownLeft(smallest);
                }
            }
        }

        /// Moves a PriorityNode without a "right" child down. 
        /// If this function is called this PriorityNode only has a left child.
        /// Therefore there is no need to go down further.
        private void PercolateDownLeft(int i)
        {
            int left = i * 2;
            if (heap[i].priority > heap[left].priority)
            {
                heap[0] = heap[i];
                heap[i] = heap[left];
                heap[left] = heap[0];
            }
        }

        /// Doubles the current heap size and copies its contents.
        private void DoubleHeapSize()
        {
            PriorityNode<T>[] temp = new PriorityNode<T>[heap.Length * 2];
            for (int i = 1; i < heap.Length; i++)
                temp[i] = heap[i];

            heap = temp;
        }

        /// Checks if a PriorityNode is currently present in the heap.
        private bool HeapContainsNode(T n)
        {
            foreach (PriorityNode<T> node in heap)
            {
                if (node == null)
                    continue;
                if (node.node.Equals(n))
                    return true;
            }
            return false;
        }
     
        /// Checks if the heap is empty.
        public bool IsEmpty
        {
            get
            {
                return size == 0;
            }
        }

        /// Inserts a PriorityNode into the heap.
        public void Insert(T node, float priority)
        {
            if (node == null)
                return;

            if (HeapContainsNode(node))
                return;

            if (size + 1 == heap.Length)
                DoubleHeapSize();

            PriorityNode<T> n = new PriorityNode<T>(node, priority);

            int hole = ++size;
            heap[0] = n;

            for (; n.priority < heap[hole / 2].priority && hole > 1; hole /= 2)
                heap[hole] = heap[hole / 2];

            heap[hole] = n;
        }

        /// Returns the highest priority PriorityNode.
        public T GetHighestPriority()
        {
            if (size == 0)
                return default(T);

            if (size == 1)
            {
                return heap[size--].node;
            }

            T n = heap[1].node;

            heap[1] = heap[size--];

            PercolateDown();

            return n;
        }

        /// Calls recursive function below to print the heap to the console.
        public void PrintHeap()
        {
            PrintHeap(1, 0);
        }

        /// Prints the heap to the console.
        private void PrintHeap(int i, int t)
        {
            if (i > size)
                return;

            PrintHeap(i * 2 + 1, t + 1);

            for (int j = 0; j < t; j++)
                Console.Write('\t');
            Console.WriteLine("P: {1}", heap[i].node, heap[i].priority);

            PrintHeap(i * 2, t + 1);

        }

        /// Percolates a PriorityNode up until its parent is smaller and its children are higher.
        public void PercolateUp(int i)
        {
            if (i == 1)
                return;

            if (heap[i / 2].priority < heap[i].priority)
            {
                heap[0] = heap[i];
                heap[i] = heap[i / 2];
                heap[i / 2] = heap[0];

                PercolateUp(i / 2);
            }
        }
    }
}
