using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;

namespace TowerDefense.World
{
    public class Graph
    {
        // Defines Infinity (used by Dijkstra's algoritmn).
        public static readonly float INFINITY = float.MaxValue;
        // Cost of going between Vertices, default 1.
        private int cost = 1;

        /// Initialzes Graph of Gameworld.
        public void InitializeGraph()
        {
            // Destroys all Vertices.
            for (int i = 0; i < GameWorld.Instance.tiles; i++)
            {
                GameWorld.Instance.tilesList[i].DestroyVertex();
            }

            /* 
             * Creates Vertex on every buildable Tile. 
             * Connects this Vertex with all neighbouring Vertices (corresponding to other buildable Tiles) 
             * by adding Edges between them.
             */
            for (int i = 0; i < GameWorld.Instance.tiles; i++)
            {
                BaseTile tile = GameWorld.Instance.tilesList[i];
                if (tile.buildable)
                {
                    CreateGraph(tile);
                }
            }
        }

        /// Adds Edges connecting (the vertices of) two Tiles along with a cost for travelling between these two Vertices (default 1). 
        public void AddEdge(BaseTile a, BaseTile b, float cost)
        {
            if (a.vertex == null || b.vertex == null) return;

            a.vertex.adj.Add(new Edge(b.vertex, cost));
            b.vertex.adj.Add(new Edge(a.vertex, cost));
        }

        /// Creates vertex that corresponds to Tile if Tile is not null.
        /// Gets all availabile neighbouring tiles of this Tile.
        /// Connects the Vertex of the current Tile with the vertex of each available (for building,moving) neighbouring Tile
        /// if they aren't already connected. This is done by adding an Edge between them.
        public void CreateGraph(BaseTile tile)
        {
            if (tile == null) return;
            tile.CreateVertex();

            List<BaseTile> neighbours = GameWorld.Instance.GetAvailableNeighbours(tile);
            foreach (BaseTile neighbour in neighbours)
            {
                if (tile.IsConnected(neighbour)) continue;
                neighbour.CreateVertex();
                AddEdge(tile, neighbour, cost);
            }
        }
    }
}
