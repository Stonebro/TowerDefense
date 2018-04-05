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
        // Singleton Gameworld.
        private GameWorld world = GameWorld.Instance;
        // Current position of mouse.
        private Vector2D mousePos;
        // Currently selected Tower.
        private int selectedTower;
        // enum that contains Availbable Towers.
        private enum Towers { ArrowTower=1, CannonTower=2 };
        // Decides if Graph of GameWorld is drawn or not.
        private bool drawVerts = false;

        /// Gets Tile that corresponds to the current mouse position.
        private BaseTile GetTileAtMouse {
            get {
                return world.tilesList[world.GetIndexOfTile(mousePos)];
            }
        }

        public Form1() {
            InitializeComponent();
            instance = this;
            DrawBackground();
            globalTimer.Enabled = true;
        }
        
     
        /// Paints the GameWorld. 
        private void GameWorldPB_Paint(object sender, PaintEventArgs e) {
            base.OnPaint(e);
            world.RenderWorld(e.Graphics); // Renders the world, draws each Tile.

            // Previews location of Tower placement if mouse is on the PictureBox.
            if (mousePos != null) {
                SolidBrush brush = new SolidBrush(Color.FromArgb(128, 200, 0, 0));
                e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size*2, BaseTile.size*2)));
            }
            // Draws Graph of GameWorld if user desires to see it.

        public void DrawBackground() {
            Bitmap bm = new Bitmap(600, 600);
            Graphics g = Graphics.FromImage(bm);
            world.RenderWorld(g);
            if (drawVerts) {
                foreach (BaseTile bt in world.tilesList) {
                    bt.DrawVertex(g);
                }
            }
            bool drawTowerRange = true;
            if (drawTowerRange) {
                foreach (Tower t in world.towers)
                    t.DrawAttackRange(g);
                //world.DrawTowerRanges();
            }
            GameWorldPB.Image = bm;
        }

        /// Handles placing a tower on the PictureBox.
        private void GameWorldPB_MouseDown(object sender, MouseEventArgs e) {
            // Gets 2x2 square of Tiles according to location of mouse.
            List<BaseTile> selectedTiles = GetSelectedTiles(e.Location); 
            // Tower to add.
            Tower addTower = null;
            
            // Checks if the tiles selected are buildable and a tower is selected. 
            if (world.isBuildable(selectedTiles) && selectedTower != 0) {
                // Initializes new ArrowTower if ArrowTower is currently selected.
                if (selectedTower == (int)Towers.ArrowTower)
                    addTower = new ArrowTower();
                // Initializes new CannonTower if CannonTower is currently selected.
                if (selectedTower == (int)Towers.CannonTower)
                    addTower = new CannonTower();

                // Sets all of the selected Tiles to not buildable (you cant build multiple Towers on the same Tiles).
                foreach (BaseTile bt in selectedTiles) {
                    bt.buildable = false;
                    bt.DestroyVertex();
                    bt.tower = addTower;
                }
                
                // Adds the Tower to the list of Towers, with the position being the Tile where the mouse was clicked.
                addTower.BuildTower(GetTileAtMouse);
                DrawBackground();
            }
        }

        /// Handles change of mouse location within PictureBox, redraws.
        private void GameWorldPB_MouseMove(object sender, MouseEventArgs e) {
            mousePos = e.Location;
            GameWorldPB.Invalidate();
        }

        /// Sets mousePos to null after leaving main PictureBox, redraws PictureBox.       
        private void GameWorldPB_MouseLeave(object sender, EventArgs e) {
            mousePos = null;
            GameWorldPB.Invalidate();
        }

        /// Handles selecting ArrowTower as placement.
        private void Tower1PB_MouseDown(object sender, MouseEventArgs e) {
            this.selectedTower = (int)Towers.ArrowTower;
        }

        /// Handles selecting CannonTower as placement.      
        private void Tower2PB_MouseDown(object sender, MouseEventArgs e)
        {
            this.selectedTower = (int)Towers.CannonTower;
        }
      
        ///  Gets currently selected Tiles (used after clicking the main picturebox).
        private List<BaseTile> GetSelectedTiles(Vector2D pos) {
            List<BaseTile> toReturn = new List<BaseTile>();
            /* Checks if the tile clicked has a tile to the right and a tile to the bottom of it.
             * This is needed because a tower is 2x2 tiles. 
             * If this check isn't done while the user attempts to place a tower on the right/bottom edge of the picturebox 
             * it causes unexpected behavior.
             */
                if (world.GetIndexOfTile(pos) % world.tilesH != world.tilesH-1 && world.GetIndexOfTile(pos) <= world.tiles-world.tilesH) {
                    toReturn.Add(world.tilesList[world.GetIndexOfTile(pos)]); // Adds actual selected tile to list.
                    toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + 1]); // Adds tile to the right to list.
                    toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + world.tilesH]); // Adds tile to the bottom to list. 
                    toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + world.tilesH + 1]); // Adds tile to the bottom-right to list.
                }
            return toReturn;
        }

        /// Handles "Show Vertices" Button click event.
        private void showVerticesBtn_Click(object sender, EventArgs e) {
            drawVerts = !drawVerts; // Toggles bool.
            GameWorldPB.Invalidate();
            DrawBackground();
        }

        
        /// Handles timer tick to update gameworld. 
        private void globalTimer_Tick(object sender, EventArgs e) {
            world.Update();
        }
    }
}