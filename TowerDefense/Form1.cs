using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.Tiles;
using TowerDefense.Towers;
using TowerDefense.Util;
using TowerDefense.World;
using WMPLib;

namespace TowerDefense
{
    public partial class Form1 : Form
    {
        // Singleton Gameworld.
        private GameWorld world = GameWorld.Instance;
        // Current position of mouse.
        private Vector2D mousePos;
        // Currently selected Tower.
        private Tower selectedTower;
        // Decides if Graph of GameWorld is drawn or not.
        private bool drawVerts = false;
        // Used for counting elapsed ticks of GlobalTimer.
        public static int TickCounter { get; private set; }
        WindowsMediaPlayer audioPlayer = new WindowsMediaPlayer();

        // Gets Tile that corresponds to the current mouse position.
        private BaseTile GetTileAtMouse
        {
            get
            {
                return world.tilesList[world.GetIndexOfTile(mousePos)];
            }
        }

        public Form1()
        {
            InitializeComponent();
            playerGoldAmount.Text = world.Gold.ToString();
            playerLivesAmount.Text = world.Lives.ToString();
            audioPlayer.settings.volume = 50;
            DrawBackground();
            globalTimer.Enabled = true;
        }

        // Paints the GameWorld. 
        private void GameWorldPB_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            // Previews location of Tower placement if mouse is on the PictureBox.
            if (mousePos != null)
            {
                SolidBrush brush = new SolidBrush(Color.FromArgb(128, 200, 0, 0));
                // No tower selected: 1 square mouse cursor
                if (selectedTower == null)
                    e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size, BaseTile.size)));
                // Tower selected: 4 square mouse cursor
                else
                {
                    e.Graphics.FillRectangle(brush, new Rectangle(GetTileAtMouse.pos, new Vector2D(BaseTile.size * 2, BaseTile.size * 2)));
                    if (selectedTower != null)
                    {
                        selectedTower.position = GetTileAtMouse.pos + new Vector2D(BaseTile.size, BaseTile.size); ;
                        selectedTower.DrawAttackRange(e.Graphics);
                    }
                }
            }
            if (world.Tower != null)
            {
                SolidBrush blue = new SolidBrush(Color.FromArgb(128, Color.Blue));
                e.Graphics.FillRectangle(blue, new Rectangle(world.Tower.pos[0].pos, new Vector2D(BaseTile.size * 2, BaseTile.size * 2)));
                world.Tower.DrawAttackRange(e.Graphics);
            }
        }

        // Draws Graph of GameWorld if user desires to see it.
        public void DrawBackground()
        {
            Bitmap bm = new Bitmap(600, 600);
            Graphics g = Graphics.FromImage(bm);
            world.RenderWorld(g);
            if (drawVerts)
            {
                foreach (BaseTile bt in world.tilesList)
                {
                    bt.DrawVertex(g);
                }
            }

            foreach (Tower t in world.towers)
            {
                t.b = g; // Steal Form's graphics.
                if (t.drawTowerRange) t.DrawAttackRange(g);
                g.DrawImage(t.sprite, t.position.x - BaseTile.size, t.position.y - BaseTile.size);
            }
            // Render each non-dead enemy
            foreach (Enemy e in world.enemies) if (!e.dead) e.Render(g);
            GameWorldPB.Image = bm;
        }

        /// Handles placing a tower on the PictureBox.
        private void GameWorldPB_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                world.Crosshair.x = e.Location.X;
                world.Crosshair.y = e.Location.Y;
            }
            if (e.Button == MouseButtons.Right)
            {
                DeselectTower();
                HandSelectPB_Click(sender, e);
            }
            else
            {
                // No tower to build selected, mouse can be used to select placed Towers
                if (selectedTower == null && !GetTileAtMouse.buildable)
                {
                    // Loop through all Towers
                    for (int i = 0; i < world.towers.Count; i++)
                    {
                        // Check if the clicked tile is occupied by a Tower
                        for (int j = 0; j < world.towers[i].pos.Count; j++)
                        {
                            if (world.towers[i].pos[j] == GetTileAtMouse)
                            {
                                // Deselect the previously selected Tower (if there is one)
                                DeselectTower();
                                // Set the selected Tower, draw its Range and toggle the Details.
                                world.Tower = world.towers[i];
                                SelectTower();
                            }
                        }
                    }
                }
                else DeselectTower();


                // Gets 2x2 square of Tiles according to location of mouse.
                List<BaseTile> selectedTiles = GetSelectedTiles(e.Location);
                // If the selected tiles are buildable AND you have a tower selected AND you have enough money AND the Path won't be blocked..
                if (world.IsBuildable(selectedTiles) && selectedTower != null && world.Gold + selectedTower.goldCost >= 0)
                {
                    if (!world.CheckIfPathIsBlocked(selectedTiles)) {
                        // ..Check the selected tower's type and create a new object of that type
                        Tower addTower = null;
                        if (selectedTower is ArrowTower) addTower = new ArrowTower();
                        if (selectedTower is CannonTower) addTower = new CannonTower();
                        if (selectedTower is SplitShotTower) addTower = new SplitShotTower();
                        if (selectedTower is DogHouseTower) addTower = new DogHouseTower();
                        if (selectedTower is FuzzyTower) addTower = new FuzzyTower();
                        // ..Deduct gold
                        world.DeductGold(selectedTower.goldCost);
                        // Disable each selected tile
                        foreach (BaseTile bt in selectedTiles) {
                            bt.DisableTile();
                            bt.tower = addTower;
                        }
                        // Build the tower, update the gold and redraw the background
                        addTower.BuildTower(selectedTiles);
                        audioPlayer.URL = "C:/Dev/TowerDefense/TowerDefense/Audio/TowerPlace.mp3";
                        DeselectTower();
                        world.Tower = addTower;
                        SelectTower();
                        DrawBackground();
                    }
                    else audioPlayer.URL = "C:/Dev/TowerDefense/TowerDefense/Audio/CannotBuildThere.mp3";
                }
            }

        }

        /// Handles change of mouse location within PictureBox, redraws.
        private void GameWorldPB_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = e.Location;
            if (selectedTower == null && !GetTileAtMouse.buildable) GameWorldPB.Cursor = Cursors.Hand;
            else GameWorldPB.Cursor = Cursors.Default;
            GameWorldPB.Invalidate();
        }

        /// Sets mousePos to null after leaving main PictureBox, redraws PictureBox.       
        private void GameWorldPB_MouseLeave(object sender, EventArgs e)
        {
            mousePos = null;
            GameWorldPB.Invalidate();
        }

        /// Handles selecting ArrowTower as placement.
        private void Tower1PB_MouseDown(object sender, MouseEventArgs e)
        {
            this.selectedTower = new ArrowTower();
            GameWorldPB.Invalidate();
            DeselectTower();
            towerDescription.Text = selectedTower.description;
        }

        /// Handles selecting CannonTower as placement.      
        private void Tower2PB_MouseDown(object sender, MouseEventArgs e)
        {
            this.selectedTower = new CannonTower();
            GameWorldPB.Invalidate();
            DeselectTower();
            towerDescription.Text = selectedTower.description;
        }

        private void Tower3PB_MouseDown(object sender, MouseEventArgs e)
        {
            this.selectedTower = new SplitShotTower();
            GameWorldPB.Invalidate();
            DeselectTower();
            towerDescription.Text = selectedTower.description;
        }

        private void DogHouseTowerPB_Click(object sender, EventArgs e)
        {
            this.selectedTower = new DogHouseTower();
            GameWorldPB.Invalidate();
            DeselectTower();
            towerDescription.Text = selectedTower.description;
        }

        private void FuzzyTowerPB_Click(object sender, EventArgs e)
        {
            this.selectedTower = new FuzzyTower();
            GameWorldPB.Invalidate();
            DeselectTower();
            towerDescription.Text = selectedTower.description;
        }

        private void HandSelectPB_Click(object sender, EventArgs e)
        {
            this.selectedTower = null;
            GameWorldPB.Invalidate();
        }

        ///  Gets currently selected Tiles (used after clicking the main picturebox).
        private List<BaseTile> GetSelectedTiles(Vector2D pos)
        {
            List<BaseTile> toReturn = new List<BaseTile>();
            /* Checks if the tile clicked has a tile to the right and a tile to the bottom of it.
             * This is needed because a tower is 2x2 tiles. 
             * If this check isn't done while the user attempts to place a tower on the right/bottom edge of the picturebox 
             * it causes unexpected behavior.
             */
            if (world.GetIndexOfTile(pos) % world.tilesH != world.tilesH - 1 && world.GetIndexOfTile(pos) <= world.tiles - world.tilesH)
            {
                toReturn.Add(world.tilesList[world.GetIndexOfTile(pos)]); // Adds actual selected tile to list.
                toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + 1]); // Adds tile to the right to list.
                toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + world.tilesH]); // Adds tile to the bottom to list. 
                toReturn.Add(world.tilesList[world.GetIndexOfTile(pos) + world.tilesH + 1]); // Adds tile to the bottom-right to list.
            }
            return toReturn;
        }

        /// Handles "Show Vertices" Button click event.
        private void ShowVerticesBtn_Click(object sender, EventArgs e)
        {
            drawVerts = !drawVerts; // Toggles bool.
            DeselectTower();
            GameWorldPB.Invalidate();
            DrawBackground();
        }

        /// Handles timer tick to update gameworld. 
        private void GlobalTimer_Tick(object sender, EventArgs e)
        {
            playerGoldAmount.Text = world.Gold.ToString();
            playerLivesAmount.Text = world.Lives.ToString();
            if (TickCounter < (1000 / globalTimer.Interval * 10) && TickCounter % (1000 / globalTimer.Interval) == 0)
            {
                world.SpawnEnemy();
            }
            DrawBackground();
            world.Update();
            TickCounter++;
        }

        private void DeleteTowerBtn_Click(object sender, EventArgs e)
        {
            foreach (BaseTile bt in world.Tower.pos)
                bt.EnableTile();
            audioPlayer.URL = "C:/Dev/TowerDefense/TowerDefense/Audio/TowerDelete.mp3";
            world.towers.Remove(world.Tower);
            world.RecalculatePaths();
            DeselectTower();
            DrawBackground();
        }

        private void NextWaveBtn_Click(object sender, EventArgs e)
        {
            world.waveCount++;
            TickCounter = 0;
        }

        // Toggle relevant information for the selected Tower on
        private void SelectTower()
        {
            if (world.Tower != null)
            {
                selectedTowerNameLabel.Visible = true;
                selectedTowerName.Visible = true;
                towerDescription.Text = world.Tower.description;
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
                selectedTowerName.Text = world.Tower.name;
                selectedTowerDamage.Text = world.Tower.attackPower.ToString();
                selectedTowerAS.Text = "1atk/" + (world.Tower.attackInterval / 10).ToString() + "s";
                selectedTowerShotsFired.Text = world.Tower.shotsFired.ToString();
                selectedTowerTotalDamage.Text = (world.Tower.shotsFired * world.Tower.attackPower).ToString();
                selectedTowerKills.Text = world.Tower.kills.ToString();
                world.Tower.drawTowerRange = true;
            }
        }

        // Toggle information for a selected Tower off
        private void DeselectTower()
        {
            selectedTowerNameLabel.Visible = false;
            selectedTowerName.Visible = false;
            towerDescription.Text = "";
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
            if (world.Tower != null) world.Tower.drawTowerRange = false;
            world.Tower = null;
        }


    }
}