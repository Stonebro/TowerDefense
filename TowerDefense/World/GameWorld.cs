﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Enemies;
using TowerDefense.Tiles;
using TowerDefense.Towers;
using TowerDefense.Util;

namespace TowerDefense.World {
    /// Represents the GameWorld.
    public class GameWorld {
        // Singleton instance of GameWorld.
        private static GameWorld _instance;

        // Getter that initializes Gameworld if it doesn't already exist and returns the GameWorld instance.
        public static GameWorld Instance {
            get {
                if (_instance == null) {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }
        // Amount of horizontal Tiles.
        public int tilesH = 40;
        // Amount of vertical Tiles
        public int tilesV = 40;
        // Total amount of Tiles.
        public int tiles;
        // List containing all Tiles.
        public BaseTile[] tilesList;
        // List of Towers.
        public List<Tower> towers;
        public List<Enemy> enemies;
        public Graph graph;
        // StartTile and EndTile.
        public BaseTile startTile, endTile;

        // UI Elements
        public int gold { get; private set; }
        public int lives { get; private set; }
        public Tower tower { get; set; }

        /// GameWorld constructor. 
        public GameWorld() {
            gold = 500;
            lives = 50;
            // Initializes GameWorld.
            _instance = this;

            // Sets total amount of Tiles.
            tiles = tilesH * tilesV;
            // Creates an array of Tiles with size equal to total amount of tiles.
            tilesList = new BaseTile[tiles];
            // Initializes list of Towers.
            towers = new List<Tower>();
            enemies = new List<Enemy>();
            // Creates Graph.
            graph = new Graph();

            // Fills TileList with Tiles.
            float curX = 0, curY = 0;
            for(int i = 0; i < tiles; i++) {
                BaseTile tile = new BaseTile(new Vector2D(curX, curY));
                curX += BaseTile.size;
                if(curX >= (BaseTile.size * tilesH)) {
                    curX = 0;
                    curY += BaseTile.size;
                }
                this.tilesList[i] = tile;
            }
            // Initializes Graph.
            graph.InitializeGraph();
            // Sets startTile to be upper-left tile.
            startTile = tilesList[0];
            // sets startTile not Buildable.
            startTile.buildable = false;
            // Sets endTile to be bottom-right tile.
            endTile = tilesList[tiles - 1];
            // Sets endTile to not Buildable.
            endTile.buildable = false;
            Enemy testEnemy = new Imp(startTile.pos, 10, 10, new Vector2D(), Path.GetPath(startTile, endTile));
            Instance.enemies.Add(testEnemy);
        }

        /// Draws each tile
        public void RenderWorld(Graphics g) {
            // Loops through Tiles.
            for (int i = 0; i < tiles; i++) {
                // Handles draw of not-buildable Tiles (Should be sprites).
                if (tilesList[i].buildable == false) {
                    if (tilesList[i].tower is ArrowTower) {
                        SolidBrush ATBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 200));
                        g.FillRectangle(ATBrush, new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                    } else if (tilesList[i].tower is CannonTower) {
                        SolidBrush CTBrush = new SolidBrush(Color.FromArgb(128, 25, 25, 25));
                        g.FillRectangle(CTBrush, new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                    }
                /*else g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 200, 0)), new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));*/
                }  else { // If Tile is buildable.
                    g.DrawRectangle(new Pen(Color.LightGray), new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                }
                // Draws startTile and endTile.
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(tilesList[0].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(tilesList[tiles-1].pos, new Vector2D(BaseTile.size, BaseTile.size)));
            }
            foreach (Enemy e in enemies)
            {
                e.Render(g);
                e.path.Render(g);
            }
        }

        public void Update() {
            foreach (Tower t in towers) t.Update();
            foreach (Enemy e in enemies) e.Update();
        }

        public void SpawnEnemy()
        {
            ResetAllVertices();
            Imp imp = new Imp(startTile.pos, 5, 15, new Vector2D(), Path.GetPath(startTile, endTile));
            Instance.enemies.Add(imp);
        }

        /// Returns index of clicked Tile.
        public int GetIndexOfTile(Vector2D pos) {
            int index = (int)(pos.y / BaseTile.size) * tilesH;
            index += (int)(pos.x / BaseTile.size);
            return index;
        }

        /// Checks if any of the Tiles within a 2x2 space (around the mouseclick) has something built on it already.
        public bool isBuildable(List<BaseTile> selectedTiles) {
            if (selectedTiles.All(i => i.buildable)) return true;
            return false;
        }

        public void RecalculatePaths()
        {
            BaseTile endTile = Instance.endTile;
            foreach (Enemy enemy in Instance.enemies)
            {
                int enemyTileIndex = Instance.GetIndexOfTile(enemy.pos);
                BaseTile enemyTile = Instance.tilesList[enemyTileIndex];
                foreach (BaseTile tile in Instance.tilesList)
                {
                    tile.vertex.Reset();
                }
                enemy.path = Path.GetPath(enemyTile, endTile);
            }

        }

        public void ResetAllVertices()
        {
            foreach (BaseTile tile in Instance.tilesList)
            {
                tile.vertex.Reset();
            }
        }

        /// Gets all neighbours of tile where building is allowed.
        public List<BaseTile> GetAvailableNeighbours(BaseTile tile) {
            // Initializes List of (available) neighbours.
            List<BaseTile> neighbours = new List<BaseTile>();

            // Used for determining if a Tile is present above/under/to the left/to the right of the Tile specified.
            bool up =false, down=false, left=false, right=false;
            if (tile.pos.x >= tilesH) left = true; // A tile to the left is present.
            if (tile.pos.x < (tilesH * BaseTile.size) - BaseTile.size) right = true; // A tile to the right is present.
            if (tile.pos.y >= BaseTile.size) up = true; // A tile above the specified tile is present.
            if (tile.pos.y < (tilesV * BaseTile.size) - BaseTile.size) down = true; // A tile under the specified tile is present.

            // If there is a Tile above the specified Tile.
            if (up) {
                // Gets the Tile upwards of the current tile.
                BaseTile upTile = tilesList[GetIndexOfTile(tile.pos - new Vector2D(0, BaseTile.size))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (upTile.buildable) neighbours.Add(upTile);
            }
            // If there is a Tile under the specified Tile.
            if (down) {
                // Gets the Tile under the current Tile.
                BaseTile downTile = tilesList[GetIndexOfTile(tile.pos + new Vector2D(0, BaseTile.size))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (downTile.buildable) neighbours.Add(downTile);
            }
            // If there is a Tile to the left of the specified Tile.
            if (left) {
                // Gets the Tile to the left of the current Tile.
                BaseTile leftTile = tilesList[GetIndexOfTile(tile.pos - new Vector2D(BaseTile.size, 0))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (leftTile.buildable) neighbours.Add(leftTile);
            }
            // If theres a Tile to the right of the specified Tile.
            if (right) {
                // Gets the Tile to the right of the current Tile.
                BaseTile rightTile = tilesList[GetIndexOfTile(tile.pos + new Vector2D(BaseTile.size, 0))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (rightTile.buildable) neighbours.Add(rightTile);
            }
            // Returns list of neighbours.
            return neighbours;
        }

        public int AddGold(int amount) {
            return gold += amount;
        }
        public int DeductGold(int amount) {
            return gold -= amount;
        }
    }
}
