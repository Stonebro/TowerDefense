using System;
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
    public class GameWorld {
        private static GameWorld _instance;
        public static GameWorld instance {
            get {
                if (_instance == null) {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }
        public int tilesH = 40, tilesV = 40, tiles;
        public BaseTile[] tilesList;
        public List<Tower> towers;
        public List<Enemy> enemies;
        public Graph graph;
        public BaseTile startTile, endTile;

        public GameWorld() {
            _instance = this;
            tiles = tilesH * tilesV;
            tilesList = new BaseTile[tiles];
            towers = new List<Tower>();
            enemies = new List<Enemy>();
            graph = new Graph();

            // Fill tileList with all grid tiles
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
            graph.initGraph();
            startTile = tilesList[0];
            startTile.buildable = false;
            endTile = tilesList[tiles - 1];
            endTile.buildable = false;
            Enemy testEnemy = new Imp(tilesList[60].pos, 10, 10, new Vector2D());
            instance.enemies.Add(testEnemy);
        }

        // Draw each tile
        public void RenderWorld(Graphics g) {
            for (int i = 0; i < tiles; i++) {
                if (tilesList[i].buildable == false) {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 200, 0)), new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                } else {
                    g.DrawRectangle(new Pen(Color.LightGray), new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                }
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(tilesList[0].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(tilesList[tiles-1].pos, new Vector2D(BaseTile.size, BaseTile.size)));
            }
            foreach (Enemy e in enemies) e.Render(g);
        }

        public void Update() {
            foreach (Tower t in towers) t.Update();
            foreach (Enemy e in enemies) e.Update();
        }

        // Returns index of clicked tile
        public int GetIndexOfTile(Vector2D pos) {
            int index = (int)(pos.y / BaseTile.size) * tilesH;
            index += (int)(pos.x / BaseTile.size);
            return index;
        }

        // Check if any of the 4 spaces have something built on them already
        public bool isBuildable(List<BaseTile> selectedTiles) {
            if (selectedTiles.All(i => i.buildable)) return true;
            else return false;
        }

        public List<BaseTile> GetAvailableNeighbours(BaseTile tile) {
            List<BaseTile> neighbours = new List<BaseTile>();
            bool up=false, down=false, left=false, right=false;
            if (tile.pos.x >= tilesH) left = true;
            if (tile.pos.x < (tilesH * BaseTile.size) - BaseTile.size) right = true;
            if (tile.pos.y >= BaseTile.size) up = true;
            if (tile.pos.y < (tilesV * BaseTile.size) - BaseTile.size) down = true;

            if (up) {
                BaseTile upTile = tilesList[GetIndexOfTile(tile.pos - new Vector2D(0, BaseTile.size))];
                if (upTile.buildable) neighbours.Add(upTile);
            }
            if (down) {
                BaseTile downTile = tilesList[GetIndexOfTile(tile.pos + new Vector2D(0, BaseTile.size))];
                if (downTile.buildable) neighbours.Add(downTile);
            }
            if (left) {
                BaseTile leftTile = tilesList[GetIndexOfTile(tile.pos - new Vector2D(BaseTile.size, 0))];
                if (leftTile.buildable) neighbours.Add(leftTile);
            }
            if (right) {
                BaseTile rightTile = tilesList[GetIndexOfTile(tile.pos + new Vector2D(BaseTile.size, 0))];
                if (rightTile.buildable) neighbours.Add(rightTile);
            }
            return neighbours;
        }
    }
}
