﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense {
    public partial class Form1 : Form {
        // Singleton Gameworld.
        private GameWorld world = GameWorld.Instance;
        // Current position of mouse.
        private Vector2D mousePos;
        // Currently selected Tower.
        private Tower selectedTower;
        // Decides if Graph of GameWorld is drawn or not.
        private bool drawVerts = false; 
        // Used for counting elapsed ticks of GlobalTimer.
        public static int tickCounter { get; private set; }

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
                if (selectedTower == null)
                    e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size, BaseTile.size)));
                // Tower selected: 4 square mouse cursor
                else { 
                    e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size * 2, BaseTile.size * 2)));
                    if (selectedTower != null)
                    {
                        selectedTower.position = GetTileAtMouse.pos + new Vector2D(BaseTile.size, BaseTile.size); ;
                        selectedTower.DrawAttackRange(e.Graphics);
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

            foreach (Tower t in world.towers) {
                t.b = g; // CHEAT
                if (t.drawTowerRange) t.DrawAttackRange(g);
                g.DrawImage(t.sprite, t.position.x - BaseTile.size, t.position.y - BaseTile.size);
            }
            foreach (Enemy e in world.enemies) if(!e.dead) e.Render(g);
            GameWorldPB.Image = bm;
        }

        /// Handles placing a tower on the PictureBox.
        private void GameWorldPB_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                DeselectTower();
                handSelectPB_Click(sender, e);
            } else { 
                // No tower to build selected, mouse can be used to select placed Towers
                if (selectedTower == null && !GetTileAtMouse.buildable) {
                    // Loop through all Towers
                    for (int i = 0; i < world.towers.Count; i++) {
                        // Check if the clicked tile is occupied by a Tower
                        for(int j = 0; j < world.towers[i].pos.Count; j++) {
                            if (world.towers[i].pos[j] == GetTileAtMouse) {
                                // Deselect the previously selected Tower (if there is one)
                                DeselectTower();
                                // Set the selected Tower, draw its Range and toggle the Details.
                                world.tower = world.towers[i];
                                SelectTower();
                            } 
                        }
                    }
                } else DeselectTower();


                // Gets 2x2 square of Tiles according to location of mouse.
                List<BaseTile> selectedTiles = GetSelectedTiles(e.Location);
                // If the selected tiles are buildable AND you have a tower selected AND you have enough money..
                if (world.IsBuildable(selectedTiles) && selectedTower != null && world.gold + selectedTower.goldCost >= 0) {
                    if(world.CheckIfPathIsBlocked(selectedTiles) == false) { 
                        // ..Check the selected tower's type and create a new object of that type
                        Tower addTower = null;
                        if (selectedTower is ArrowTower) addTower = new ArrowTower();
                        if (selectedTower is CannonTower) addTower = new CannonTower();
                        if (selectedTower is SplitShotTower) addTower = new SplitShotTower();
                        if (selectedTower is DogHouseTower) addTower = new DogHouseTower();
                        // ..Deduct gold
                        world.DeductGold(selectedTower.goldCost);
                        // Disable each selected tile
                        foreach (BaseTile bt in selectedTiles) {
                            bt.DisableTile();
                            bt.tower = addTower;
                        }
                        // Build the tower, update the gold and redraw the background
                        addTower.BuildTower(selectedTiles);
                        DeselectTower();
                        world.tower = addTower;
                        SelectTower();
                        DrawBackground();
                    }
                }
            }

        }

        /// Handles change of mouse location within PictureBox, redraws.
        private void GameWorldPB_MouseMove(object sender, MouseEventArgs e) {
            mousePos = e.Location;
            if (selectedTower == null && !GetTileAtMouse.buildable) GameWorldPB.Cursor = Cursors.Hand;
            else GameWorldPB.Cursor = Cursors.Default;
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
            GameWorldPB.Invalidate();
            DeselectTower();
        }

        /// Handles selecting CannonTower as placement.      
        private void Tower2PB_MouseDown(object sender, MouseEventArgs e)
        {
            this.selectedTower = new CannonTower();
            GameWorldPB.Invalidate();
            DeselectTower();
        }

        private void Tower3PB_MouseDown(object sender, MouseEventArgs e) {
            this.selectedTower = new SplitShotTower();
            GameWorldPB.Invalidate();
            DeselectTower();
        }

        private void DogHouseTowerPB_Click(object sender, EventArgs e) {
            this.selectedTower = new DogHouseTower();
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
            playerGoldAmount.Text = world.gold.ToString();
            playerLivesAmount.Text = world.lives.ToString();
            if (tickCounter < (1000 / globalTimer.Interval * 10) && tickCounter % (1000 / globalTimer.Interval) == 0) {
                //world.SpawnEnemy();
            }
            DrawBackground();
            world.Update();
            tickCounter++;
        }

        private void deleteTowerBtn_Click(object sender, EventArgs e) {
            foreach(BaseTile bt in world.tower.pos)
                bt.EnableTile();
            world.towers.Remove(world.tower);
            world.RecalculatePaths();
            DeselectTower();
            DrawBackground();
        }

        private void nextWaveBtn_Click(object sender, EventArgs e) {
            world.waveCount++;
            tickCounter = 0;
        }

        // Toggle relevant information for the selected Tower on
        private void SelectTower() {
            if (world.tower != null) {
                selectedTowerNameLabel.Visible = true;
                selectedTowerName.Visible = true;
                selectedTowerAtkDmgLabel.Visible = true;
                selectedTowerDamage.Visible = true;
                selectedTowerASLabel.Visible = true;
                selectedTowerAS.Visible = true;
                selectedTowerShotsFiredLabel.Visible = true;
                selectedTowerShotsFired.Visible = true;
                selectedTowerTotalDamageLabel.Visible = true;
                selectedTowerTotalDamage.Visible = true;
                selectedTowerKillsLabel.Visible = true;
                selectedTowerKills.Visible = true;
                deleteTowerBtn.Visible = true;
                selectedTowerName.Text = world.tower.name;
                selectedTowerDamage.Text = world.tower.attackPower.ToString();
                selectedTowerAS.Text = "1atk/"+(world.tower.attackInterval/10).ToString()+"s";
                selectedTowerShotsFired.Text = world.tower.shotsFired.ToString();
                selectedTowerTotalDamage.Text = (world.tower.shotsFired * world.tower.attackPower).ToString();
                selectedTowerKills.Text = world.tower.kills.ToString();
                world.tower.drawTowerRange = true;
            }
        }

        // Toggle information for a selected Tower off
        private void DeselectTower() {
            selectedTowerNameLabel.Visible = false;
            selectedTowerName.Visible = false;
            selectedTowerAtkDmgLabel.Visible = false;
            selectedTowerDamage.Visible = false;
            selectedTowerASLabel.Visible = false;
            selectedTowerAS.Visible = false;
            selectedTowerShotsFiredLabel.Visible = false;
            selectedTowerShotsFired.Visible = false;
            selectedTowerTotalDamageLabel.Visible = false;
            selectedTowerTotalDamage.Visible = false;
            selectedTowerKillsLabel.Visible = false;
            selectedTowerKills.Visible = false;
            deleteTowerBtn.Visible = false;
            if(world.tower != null) world.tower.drawTowerRange = false;
            world.tower = null;
        }


    }
}