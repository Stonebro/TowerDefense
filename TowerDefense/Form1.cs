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
        //private int selectedTower;
        private Tower selectedTower;

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
            // Previews location of Tower placement if mouse is on the PictureBox.
            if (mousePos != null) {
                SolidBrush brush = new SolidBrush(Color.FromArgb(128, 200, 0, 0));
                // No tower selected: 1 square mouse cursor
                ///if (selectedTower == 0)
                if (selectedTower == null)
                    e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size, BaseTile.size)));
                // Tower selected: 4 square mouse cursor
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
                world.tower.DrawAttackRange(e.Graphics);
            }
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
            // No tower to build selected, mouse can be used to select placed Towers
            ///if(selectedTower == 0 && !GetTileAtMouse.buildable) {
            if (selectedTower == null && !GetTileAtMouse.buildable) {
                // Loop through all Towers
                for (int i = 0; i < world.towers.Count; i++) {
                    // Check if the clicked tile is occupied by a Tower
                    for(int j = 0; j < world.towers[i].pos.Count; j++) {
                        if (world.towers[i].pos[j] == GetTileAtMouse) {
                            Console.WriteLine("Tower index: " + i + " selected");
                            // Deselect the previously selected Tower (if there is one)
                            DeselectTower();
                            // Set the selected Tower, draw its Range and toggle the Details.
                            world.tower = world.towers[i];
                            SelectTower();
                        }
                    }
                }
            }


            // Gets 2x2 square of Tiles according to location of mouse.
            List<BaseTile> selectedTiles = GetSelectedTiles(e.Location);
            // If the selected tiles are buildable AND you have a tower selected AND you have enough money..
            if (world.isBuildable(selectedTiles) && selectedTower != null && world.gold + selectedTower.goldCost >= 0) {
                // ..Check the selected tower's type and create a new object of that type
                Tower addTower = null;
                if (selectedTower is ArrowTower) addTower = new ArrowTower();
                if (selectedTower is CannonTower) addTower = new CannonTower();
                // ..Deduct gold
                world.DeductGold(selectedTower.goldCost);
                // Disable each selected tile
                foreach (BaseTile bt in selectedTiles) {
                    bt.DisableTile();
                    bt.tower = addTower;
                }
                // Build the tower, update the gold and redraw the background
                addTower.BuildTower(selectedTiles);
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
            this.selectedTower = new ArrowTower();
            //towertower = new ArrowTower(); // UGLY
            GameWorldPB.Invalidate();
            DeselectTower();
        }

        /// Handles selecting CannonTower as placement.      
        private void Tower2PB_MouseDown(object sender, MouseEventArgs e)
        {
            this.selectedTower = new CannonTower();
            //towertower = new CannonTower();
            GameWorldPB.Invalidate();
            DeselectTower();
        }

        private void handSelectPB_Click(object sender, EventArgs e) {
            this.selectedTower = null;
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
            DeselectTower();
            GameWorldPB.Invalidate();
            DrawBackground();
        }

        
        /// Handles timer tick to update gameworld. 
        private void globalTimer_Tick(object sender, EventArgs e) {
            //Graphics graphics = this.CreateGraphics();
            DrawBackground();
            world.Update();
            if (world.tower == null) deleteTowerBtn.Visible = false;
        }

        private void deleteTowerBtn_Click(object sender, EventArgs e) {
            foreach(BaseTile bt in world.tower.pos)
                bt.EnableTile();
            world.towers.Remove(world.tower);
            DeselectTower();
            DrawBackground();
        }

        // Toggle relevant information for the selected Tower on or off
        private void SelectTower() {
            if (world.tower != null) {
                selectedTowerNameLabel.Visible = true;
                selectedTowerName.Visible = true;
                selectedTowerAtkDmgLabel.Visible = true;
                selectedTowerDamage.Visible = true;
                selectedTowerASLabel.Visible = true;
                selectedTowerAS.Visible = true;
                selectedTowerKillsLabel.Visible = true;
                selectedTowerKills.Visible = true;
                deleteTowerBtn.Visible = true;
                selectedTowerName.Text = world.tower.name;
                selectedTowerDamage.Text = world.tower.attackPower.ToString();
                selectedTowerAS.Text = world.tower.attackInterval.ToString();
                selectedTowerKills.Text = world.tower.kills.ToString();
                world.tower.drawTowerRange = true;
            }
        }

        private void DeselectTower() {
            selectedTowerNameLabel.Visible = false;
            selectedTowerName.Visible = false;
            selectedTowerAtkDmgLabel.Visible = false;
            selectedTowerDamage.Visible = false;
            selectedTowerASLabel.Visible = false;
            selectedTowerAS.Visible = false;
            selectedTowerKillsLabel.Visible = false;
            selectedTowerKills.Visible = false;
            deleteTowerBtn.Visible = false;
            if(world.tower != null) world.tower.drawTowerRange = false;
            world.tower = null;
        }
    }
}