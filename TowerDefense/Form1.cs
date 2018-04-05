using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerDefense.Enemies;
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

        private Tower towertower; // UGLY
        // enum that contains Available Towers.
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
            playerGoldAmount.Text = world.gold.ToString();
            playerLivesAmount.Text = world.lives.ToString();
            DrawBackground();
            globalTimer.Enabled = true;
        }

        /// Paints the GameWorld. 
        private void GameWorldPB_Paint(object sender, PaintEventArgs e) {
            base.OnPaint(e);
            //world.RenderWorld(e.Graphics); // Renders the world, draws each Tile.

            // Previews location of Tower placement if mouse is on the PictureBox.
            if (mousePos != null) {
                SolidBrush brush = new SolidBrush(Color.FromArgb(128, 200, 0, 0));
                if (selectedTower == 0)
                    e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size, BaseTile.size)));
                else { 
                    e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size * 2, BaseTile.size * 2)));
                    if(towertower != null) { // UGLY
                        //towertower.drawTowerRange = true; // UGLY
                        //towertower.position = GetTileAtMouse.pos + new Vector2D(BaseTile.size, BaseTile.size); // UGLY
                        //towertower.DrawAttackRange(e.Graphics); // UGLY
                    }
                }
            }
            if (world.tower != null) {
                SolidBrush blue = new SolidBrush(Color.FromArgb(128, Color.Blue));
                e.Graphics.FillRectangle(blue, new Rectangle(world.tower.pos[0].pos, new Vector2D(BaseTile.size * 2, BaseTile.size * 2)));
                
            }
            DrawBackground();
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

            foreach (Tower t in world.towers)
                if (t.drawTowerRange)
                    t.DrawAttackRange(g);

            GameWorldPB.Image = bm;
        }

        /// Handles placing a tower on the PictureBox.
        private void GameWorldPB_MouseDown(object sender, MouseEventArgs e) {
            // No tower to build selected.
            if(selectedTower == 0 && !GetTileAtMouse.buildable) {
                for (int i = 0; i < world.towers.Count; i++) {
                    for(int j = 0; j < world.towers[i].pos.Count; j++) {
                        if (world.towers[i].pos[j] == GetTileAtMouse) {
                            Console.WriteLine(i);
                            if (world.tower != null) world.tower.drawTowerRange = false;
                            world.tower = world.towers[i];
                            world.tower.drawTowerRange = true;
                        }
                    }
                }
            }


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

                if (world.gold + addTower.goldCost >= 0) {
                    world.AddOrDeductGold(addTower.goldCost);
                    Console.WriteLine(world.gold + "  " + addTower.goldCost);
                    // Sets all of the selected Tiles to not buildable (you cant build multiple Towers on the same Tiles).
                    foreach (BaseTile bt in selectedTiles) {
                        bt.buildable = false;
                        bt.DestroyVertex();
                        bt.tower = addTower;
                    }
                    // Adds the Tower to the list of Towers, with the position being the Tile where the mouse was clicked.
                    addTower.BuildTower(selectedTiles);
                    //towertower.drawTowerRange = false; // UGLY
                    //towertower = null; // UGLY
                    //towertower = addTower; // UGLY
                }

                playerGoldAmount.Text = world.gold.ToString();
                DrawBackground();
            }
            // Enemy placement testcode
            if(e.Button == MouseButtons.Right) {
                Enemy newEnemy = new Imp(e.Location, 5, 10, null);
                world.enemies.Add(newEnemy);
            }
        }

        /// Handles change of mouse location within PictureBox, redraws.
        private void GameWorldPB_MouseMove(object sender, MouseEventArgs e) {
            //if (selectedTower == 0 && !GetTileAtMouse.buildable) GameWorldPB.Cursor = Cursors.Hand;
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
            //towertower = new ArrowTower(); // UGLY
            if (world.tower != null) world.tower.drawTowerRange = false;
            GameWorldPB.Invalidate();
            world.tower = null;
        }

        /// Handles selecting CannonTower as placement.      
        private void Tower2PB_MouseDown(object sender, MouseEventArgs e)
        {
            this.selectedTower = (int)Towers.CannonTower;
            //towertower = new CannonTower();
            if (world.tower != null) world.tower.drawTowerRange = false;
            GameWorldPB.Invalidate();
            world.tower = null;
        }

        private void handSelectPB_Click(object sender, EventArgs e) {
            this.selectedTower = 0;
            GameWorldPB.Invalidate();
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