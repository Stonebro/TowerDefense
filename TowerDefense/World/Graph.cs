using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Tiles;

namespace TowerDefense.World {
    public class Graph {
        public static readonly float INFINITY = float.MaxValue;
        private int cost = 1;

        public void initGraph() {
            for (int i = 0; i < GameWorld.instance.tiles; i++) {
                GameWorld.instance.tilesList[i].DestroyVertex();
            }

            for (int i = 0; i < GameWorld.instance.tiles; i++) {
                BaseTile tile = GameWorld.instance.tilesList[i];
                if (tile.buildable) {
                    CreateGraph(tile);
                }
            }
        }

        public void AddEdge(BaseTile a, BaseTile b, float cost) {
            if (a.vertex == null || b.vertex == null) return;

            a.vertex.adj.Add(new Edge(b.vertex, cost));
            b.vertex.adj.Add(new Edge(a.vertex, cost));
        }

        public void CreateGraph(BaseTile tile) {
            if (tile == null) return;
            tile.CreateVertex();

            List<BaseTile> neighbours = GameWorld.instance.GetAvailableNeighbours(tile);
            foreach(BaseTile neighbour in neighbours) {
                if (tile.IsConnected(neighbour)) continue;
                neighbour.CreateVertex();
                AddEdge(tile, neighbour, cost);
            }
        }
    }
}
