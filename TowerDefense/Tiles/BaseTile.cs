﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense.Tiles
{
    public class BaseTile
    {
        // Size of Tile (in pixels)
        public static readonly int size = 15;
        // Position of Tile
        public Vector2D pos;
        // Specifies if User is allowed to build on this Tile.
        public bool buildable = true;
        // Possible Tower that is placed on this Tile.
        public Tower tower;
        // Vertex of this Tile.
        public Vertex vertex;
        // Sprite of this Tile.
        //public Bitmap sprite; 

        /// BaseTile constructor.
        public BaseTile(Vector2D pos)
        {
            this.pos = pos;
        }

        /// ToString of BaseTile, returns position of Tile.
        public override string ToString()
        {
            if (tower != null)
                Console.WriteLine(tower.GetType());
            return pos.ToString();
        }

        /// Destroys the Vertex that corresponds to the Tile. 
        public void DestroyVertex()
        {
            this.vertex = null;
        }

        /// Creates a vertex corresponding to this Tile if it doesn't have one yet and the Tile is buildable.
        public void CreateVertex()
        {
            if (vertex == null && buildable) vertex = new Vertex(this);
        }

        /// Returns if the vertex corresponding to this tile is connected with the vertex corresponding to the specified neighbouring Tile.
        public bool IsConnected(BaseTile neighbour)
        {
            foreach (Edge e in vertex.adj)
            {
                if (e.dest.parentTile == neighbour)
                    return true;
            }
            return false;
        }

        /// Creates a rectangle based on the position of the Tile (used for drawing).
        public RectangleF VertexRectangle()
        {
            return new RectangleF(pos.x + (size / 3), pos.y + (size / 3), size - (size / 3 * 2), size - (size / 3 * 2));
        }


        /// Draws the representation of the Vertex of the Tile
        public void DrawVertex(Graphics g)
        {
            if (vertex == null) return;
            // Disabled Vertex = red
            if (vertex.disabled) g.FillEllipse(new SolidBrush(Color.Red), VertexRectangle());
            else g.FillEllipse(new SolidBrush(Color.DarkTurquoise), VertexRectangle());
            if (vertex.adj != null)
            {
                PointF a, b;
                a = new PointF(pos.x + size / 2, pos.y + size / 2);
                foreach (Edge e in vertex.adj)
                {
                    Vector2D ePos = e.dest.parentTile.pos;
                    b = new PointF(ePos.x + size / 2, ePos.y + size / 2);
                    // Draw a red line if the Edge is disabled, draw a darkturqoise line if it is enabled.
                    if (e.disabled) g.DrawLine(new Pen(Color.Red), a, b);
                    else g.DrawLine(new Pen(Color.DarkTurquoise), a, b);
                }
            }

        }

        public void DisableTile()
        {
            buildable = false;
            vertex.disabled = true;
            foreach (Edge e in vertex.adj)
            {
                e.disabled = true;
                foreach (Edge n in e.dest.adj)
                    if (n.dest == vertex)
                        n.disabled = true;
            }
        }

        public void EnableTile()
        {
            buildable = true;
            vertex.disabled = false;
            foreach (Edge e in vertex.adj)
            {
                e.disabled = false;
                foreach (Edge n in e.dest.adj)
                    if (n.dest == vertex)
                        n.disabled = false;
            }
        }
    }
}
