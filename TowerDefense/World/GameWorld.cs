using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.Util.SteeringBehaviours;

namespace TowerDefense.World
{
    // Represents the GameWorld.
    public class GameWorld
    {
        // Singleton instance of GameWorld.
        private static GameWorld _instance;

        // Getter that initializes Gameworld if it doesn't already exist and returns the GameWorld instance.
        // This makes the GameWorld and the state of the game available to any class that needs it.
        public static GameWorld Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }
        // Amount of horizontal & vertical Tiles.
        public int tilesH = 40;
        public int tilesV = 40;
        // Total amount of Tiles.
        public int tiles;
        // List containing all Tiles.
        public BaseTile[] tilesList;
        // List of Towers.
        public List<Tower> towers;
        // List of Enemies
        public List<Enemy> enemies;
        // Graph with all vertices and Edges
        public Graph graph;
        // StartTile and EndTile.
        public BaseTile startTile, endTile;
        // Number of waves, this affects enemy strength
        public int waveCount;

        // UI Elements
        public int gold { get; private set; }
        public int lives { get; set; }
        public Tower tower { get; set; }

        // GameWorld constructor. 
        public GameWorld()
        {
            gold = 500;
            lives = 50;
            // Initializes GameWorld.
            _instance = this;
            // Sets total amount of Tiles.
            tiles = tilesH * tilesV;
            tilesList = new BaseTile[tiles];
            towers = new List<Tower>();
            enemies = new List<Enemy>();
            graph = new Graph();

            // Fills tilesList with Tiles.
            float curX = 0, curY = 0;
            for (int i = 0; i < tiles; i++)
            {
                BaseTile tile = new BaseTile(new Vector2D(curX, curY));
                curX += BaseTile.size;
                if (curX >= (BaseTile.size * tilesH))
                {
                    curX = 0;
                    curY += BaseTile.size;
                }
                this.tilesList[i] = tile;
            }
            graph.InitializeGraph();
            // Sets startTile to be upper-left tile.
            startTile = tilesList[0];
            // Sets endTile to be bottom-right tile.
            endTile = tilesList[tiles - 1];
            // Sets startTile and endTile to not-Buildable.
            startTile.buildable = false;
            endTile.buildable = false;

            // TEST CODE TEST CODE
            Bat testEnemy = new Bat(waveCount);
            testEnemy.pos = tilesList[125].pos;
            testEnemy.path = Path.GetPath(startTile, tilesList[674]);
            testEnemy.addForce = new Seek();
            Instance.enemies.Add(testEnemy);
        }

        // Draws each tile
        public void RenderWorld(Graphics g)
        {
            // Loops through Tiles.
            for (int i = 0; i < tiles; i++)
            {
                // Handles draw of not-buildable Tiles (Should be sprites).
                if (tilesList[i].buildable == false)
                {
                    if (tilesList[i].tower is ArrowTower)
                    {
                        SolidBrush ATBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 200));
                        g.FillRectangle(ATBrush, new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                    }
                    else if (tilesList[i].tower is CannonTower)
                    {
                        SolidBrush CTBrush = new SolidBrush(Color.FromArgb(128, 25, 25, 25));
                        g.FillRectangle(CTBrush, new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                    }
                }
                else
                { // If Tile is buildable.
                    g.DrawRectangle(new Pen(Color.LightGray), new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                }
                // Draws startTile and endTile.
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(tilesList[0].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(tilesList[tiles - 1].pos, new Vector2D(BaseTile.size, BaseTile.size)));
            }
            // Draw each enemy and their path if they're not dead.
            foreach (Enemy e in enemies)
            {
                if (!e.dead)
                {
                    e.Render(g);
                    if (e.path != null) e.path.Render(g);
                }
            }
        }

        // GameWorld updates all Towers and all non-dead Enemies.
        public void Update()
        {
            foreach (Tower t in towers) t.Update();
            foreach (Enemy e in enemies) if (!e.dead) e.Update();
            // If all the Enemies on screen are dead, wipe the list.
            if (enemies.All(i => i.dead)) enemies = new List<Enemy>();
        }

        // Spawns an Enemy on starTile, and gives it a path to the endTile.
        public void SpawnEnemy()
        {
            ResetAllVertices();
            Imp imp = new Imp(waveCount);
            imp.pos = startTile.pos;
            imp.path = Path.GetPath(startTile, endTile);
            Instance.enemies.Add(imp);
        }

        // Returns index of clicked Tile.
        public int GetIndexOfTile(Vector2D pos)
        {
            int index = (int)(pos.y / BaseTile.size) * tilesH;
            index += (int)(pos.x / BaseTile.size);
            return index;
        }

        // Checks if any of the Tiles within a 2x2 space (around the mouseclick) has something built on it already.
        public bool IsBuildable(List<BaseTile> selectedTiles)
        {
            if (selectedTiles.All(i => i.buildable) && selectedTiles.Count >= 4) return true;
            return false;
        }

        public bool CheckIfPathIsBlocked(List<BaseTile> tilesToCheck)
        {
            // Create a temporary new Path and disable all selected tiles
            Path path = new Path();
            foreach (BaseTile bt in tilesToCheck)
            {
                bt.DisableTile();
            }
            // Reset all vertices (to be safe) and try to find a Path
            ResetAllVertices();
            path = Path.GetPath(startTile, endTile);
            // Re-enable the checked tiles after you looked for a path.
            foreach (BaseTile bt in tilesToCheck)
            {
                bt.EnableTile();
            }
            // If the Path is non-existent, return true. Otherwise return false.
            if (path.Current == null) return true;
            return false;
        }

        // This method recalculates the paths for all Enemies.
        public void RecalculatePaths(List<BaseTile> tilesToCheck)
        {

            foreach (Enemy enemy in Instance.enemies)
            {
                if (!enemy.dead && enemy.path.IsBlocked(tilesToCheck))
                {
                    int enemyTileIndex = Instance.GetIndexOfTile(enemy.pos);
                    BaseTile enemyTile = Instance.tilesList[enemyTileIndex];
                    foreach (BaseTile tile in Instance.tilesList)
                    {
                        tile.vertex.Reset();
                        enemy.path = Path.GetPath(enemyTile, endTile);
                    }
                }
            }
        }
        public void ResetAllVertices()
        {
            foreach (BaseTile tile in Instance.tilesList)
                tile.vertex.Reset();
        }

        // Gets all neighbours of tile where building is allowed.
        public List<BaseTile> GetAvailableNeighbours(BaseTile tile)
        {
            // Initializes List of (available) neighbours.
            List<BaseTile> neighbours = new List<BaseTile>();

            // Used for determining if a Tile is present above/under/to the left/to the right of the Tile specified.
            bool up = false, down = false, left = false, right = false;
            if (tile.pos.x >= tilesH) left = true; // A tile to the left is present.
            if (tile.pos.x < (tilesH * BaseTile.size) - BaseTile.size) right = true; // A tile to the right is present.
            if (tile.pos.y >= BaseTile.size) up = true; // A tile above the specified tile is present.
            if (tile.pos.y < (tilesV * BaseTile.size) - BaseTile.size) down = true; // A tile under the specified tile is present.

            // If there is indeed a Tile above the selected Tile
            if (up)
            {
                // Gets that Tile.
                BaseTile upTile = tilesList[GetIndexOfTile(tile.pos - new Vector2D(0, BaseTile.size))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (upTile.buildable) neighbours.Add(upTile);
                // Check if the upTile has any Tiles to its left or right, and add those (for diagonal vertices)
                if (left)
                    neighbours.Add(tilesList[GetIndexOfTile(tile.pos + new Vector2D(-BaseTile.size, -BaseTile.size))]);
                if (right)
                    neighbours.Add(tilesList[GetIndexOfTile(tile.pos + new Vector2D(BaseTile.size, -BaseTile.size))]);
            }
            // The same happens for Tiles down, left and right.
            if (down)
            {
                BaseTile downTile = tilesList[GetIndexOfTile(tile.pos + new Vector2D(0, BaseTile.size))];
                if (downTile.buildable) neighbours.Add(downTile);
                if (left)
                    neighbours.Add(tilesList[GetIndexOfTile(tile.pos + new Vector2D(-BaseTile.size, BaseTile.size))]);
                if (right)
                    neighbours.Add(tilesList[GetIndexOfTile(tile.pos + new Vector2D(BaseTile.size, BaseTile.size))]);
            }
            if (left)
            {
                BaseTile leftTile = tilesList[GetIndexOfTile(tile.pos - new Vector2D(BaseTile.size, 0))];
                if (leftTile.buildable) neighbours.Add(leftTile);
            }
            if (right)
            {
                BaseTile rightTile = tilesList[GetIndexOfTile(tile.pos + new Vector2D(BaseTile.size, 0))];
                if (rightTile.buildable) neighbours.Add(rightTile);
            }
            // Returns list of neighbours.
            return neighbours;
        }

        public int AddGold(int amount)
        {
            return gold += amount;
        }
        public int DeductGold(int amount)
        {
            return gold -= amount;
        }
    }
}
