using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Towers;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense.Tiles {
    public class BaseTile {
        public static readonly int size = 15;
        public Vector2D pos;
        public bool buildable = true;
        public Tower tower;
        public Vertex vertex;
        //public Bitmap sprite; 

        public BaseTile(Vector2D pos) {
            this.pos = pos;
        }

        public override string ToString() {
            if (tower != null)
                Console.WriteLine(tower.GetType());
            return pos.ToString();
        }

        public void DestroyVertex() {
            this.vertex = null;
        }

        public void CreateVertex() {
            if (vertex == null && buildable) vertex = new Vertex(this);
        }

        public bool IsConnected(BaseTile neighbour) {
            foreach(Edge e in vertex.adj) {
                if (e.dest.parentTile == neighbour)
                    return true;
            }
            return false;
        }

        public RectangleF vertexRectangle() {
            return new RectangleF(pos.x + (size / 3), pos.y + (size / 3), size - (size / 3 * 2), size - (size / 3 * 2));
        }

        public void DrawVertex(Graphics g) {
            if (vertex == null) return;
            g.FillEllipse(new SolidBrush(Color.DarkTurquoise), vertexRectangle());
            if (vertex.adj != null) {
                PointF a, b;
                a = new PointF(pos.x + size / 2, pos.y + size / 2);
                foreach(Edge e in vertex.adj) {
                    Vector2D ePos = e.dest.parentTile.pos;
                    b = new PointF(ePos.x + size/2, ePos.y + size/2);
                    g.DrawLine(new Pen(Color.DarkTurquoise), a, b);
                }
            }

        }
    }
}
