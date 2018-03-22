using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerDefense.Tiles;
using TowerDefense.Towers;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense {
    public partial class Form1 : Form {
        private GameWorld world = GameWorld.instance;
        private Vector2D mousePos;
        private int selectedTower;
        enum Towers { ArrowTower=1, CannonTower };

        bool drawVerts = false;

        private BaseTile GetTileAtMouse {
            get {
                return world.tilesList[world.GetIndexOfTile(mousePos)];
            }
        }

        public Form1() {
            InitializeComponent();
        }
        
        private void GameWorldPB_Paint(object sender, PaintEventArgs e) {
            base.OnPaint(e);
            world.RenderWorld(e.Graphics);
            if (mousePos != null) {
                SolidBrush brush = new SolidBrush(Color.FromArgb(128, 200, 0, 0));
                e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size*2, BaseTile.size*2)));
            }
            if (drawVerts) {
                foreach (BaseTile bt in world.tilesList) {
                    bt.DrawVertex(e.Graphics);
                }
            }
        }

        private void GameWorldPB_MouseDown(object sender, MouseEventArgs e) {
            List<BaseTile> selectedTiles = GetSelectedTiles(e.Location);

            Tower addTower = null; 
            if (world.isBuildable(selectedTiles) && selectedTower != 0) {
                if (selectedTower == (int)Towers.ArrowTower)
                    addTower = new ArrowTower();
                if (selectedTower == (int)Towers.CannonTower)
                    addTower = new CannonTower();
                foreach (BaseTile bt in selectedTiles) {
                    bt.buildable = false;
                    bt.tower = addTower;
                }
                addTower.BuildTower(GetTileAtMouse);
            }
        }

        private void GameWorldPB_MouseMove(object sender, MouseEventArgs e) {
            mousePos = e.Location;
            GameWorldPB.Invalidate();
        }

        private void GameWorldPB_MouseLeave(object sender, EventArgs e) {
            mousePos = null;
            GameWorldPB.Invalidate();
        }

        private void Tower1PB_MouseDown(object sender, MouseEventArgs e) {
            this.selectedTower = (int)Towers.ArrowTower;
        }

        private void Tower2PB_MouseDown(object sender, MouseEventArgs e) {
            this.selectedTower = (int)Towers.CannonTower;
        }

        private List<BaseTile> GetSelectedTiles(Vector2D pos) {
            List<BaseTile> toReturn = new List<BaseTile>();
                if (world.GetIndexOfTile(pos) % world.tilesH != world.tilesH-1 && world.GetIndexOfTile(pos) <= world.tiles-world.tilesH) {
                    toReturn.Add(world.tilesList[world.GetIndexOfTile(pos)]);
                    toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + 1]);
                    toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + world.tilesH]);
                    toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + world.tilesH + 1]);
                }
            return toReturn;
        }

        private void showVerticesBtn_Click(object sender, EventArgs e) {
            drawVerts = !drawVerts;
            //Graphics g = this.CreateGraphics();
            //foreach(BaseTile bt in world.tilesList) {
            //    bt.DrawVertex(g);
            //}
        }
    }
}