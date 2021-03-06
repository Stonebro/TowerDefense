﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.Entities.Powerups;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.Util.Steering;
using static TowerDefense.Util.Steering.SteeringBehavior;

namespace TowerDefense.World
{
    /// Represents the GameWorld.
    public class GameWorld
    {
        // Singleton instance of GameWorld.
        private static GameWorld _instance;

        // Getter that initializes Gameworld if it doesn't already exist and returns the GameWorld instance.
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
        // List of Enemies.
        public List<Enemy> enemies;
        // List of Coins, used for demonstrating explore.
        public Queue<Powerup> coins;
        // List of Flying Entities.
        public List<FlyingEntity> flyingEntities;

        // Eagles for showing required steering behaviours.
        public Eagle testEagle; // Shows Seek.
        public Eagle testEagle2; // Shows Offsetpursuit.
        public Eagle testEagle3; // Shows Explore (also uses Arrive).

        // The target for seek/arrive like behaviors. Can be updated by pressing the middle mouse button on a position on the PictureBox.
        public Vector2D Crosshair = new Vector2D(350, 350);
        public Graph graph;
        // StartTile and EndTile.
        public BaseTile startTile, endTile;
        // Number of waves, this affects enemy strength.
        public int waveCount;

        public bool pathBlocked;
        public Stopwatch stopwatch;

        // UI Elements
        public int Gold { get; private set; }
        public int Lives { get; set; }
        public Tower Tower { get; set; }

        /// GameWorld constructor. 
        public GameWorld()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            Gold = 500;
            Lives = 50;
            // Initializes GameWorld.
            _instance = this;

            // Sets total amount of Tiles.
            tiles = tilesH * tilesV;
            // Creates an array of Tiles with size equal to total amount of tiles.
            tilesList = new BaseTile[tiles];
            // Initializes list of Towers.
            towers = new List<Tower>();
            enemies = new List<Enemy>();
            coins = new Queue<Powerup>();
            flyingEntities = new List<FlyingEntity>();
            // Creates Graph.
            graph = new Graph();

            // Fills TileList with Tiles.
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

            // Adds coins to queue (to be able to explore them).
            coins.Enqueue(new Coin(0, 250, 30, 30));
            coins.Enqueue(new Coin(200, 250, 30, 30));
            coins.Enqueue(new Coin(0, 500, 30, 30));
            coins.Enqueue(new Coin(200, 500, 30, 30));


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
            Bat testEnemy = new Bat(waveCount);
            testEagle = new Eagle(new Vector2D(100, 100), Vector2D.Zero, Vector2D.Zero, Vector2D.Zero, 20, 5, 5, 10, 10, BehaviorType.SEEK);
            testEagle2 = new Eagle(new Vector2D(100, 100), Vector2D.Zero, Vector2D.Zero, Vector2D.Zero, 20, 5, 5, 10, 10, BehaviorType.OFFSETPURSUIT);
            testEagle2.SetTargetAgent1(testEagle);
            testEagle3 = new Eagle(new Vector2D(100, 100), Vector2D.Zero, Vector2D.Zero, Vector2D.Zero, 20, 5, 5, 10, 10, BehaviorType.EXPLORE)
            {
                goals = coins,

            };
            // Deep copy of goals to be able keep repeating the original queue of goals.
            testEagle3.originalGoals = new Queue<Powerup>(coins);

            flyingEntities.Add(testEagle);
            flyingEntities.Add(testEagle2);
            flyingEntities.Add(testEagle3);
            testEnemy.pos = tilesList[125].pos;
            testEnemy.path = Path.GetPath(startTile, tilesList[674]);
            testEnemy.AddForce = new Seek();
            Instance.enemies.Add(testEnemy);
        }

        /// Draws each tile
        public void RenderWorld(Graphics g)
        {
            // Loops through Tiles.
            for (int i = 0; i < tiles; i++)
            {
                { // If Tile is buildable.
                    g.DrawRectangle(new Pen(Color.LightGray), new Rectangle(tilesList[i].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                }
                // Draws startTile and endTile.
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(tilesList[0].pos, new Vector2D(BaseTile.size, BaseTile.size)));
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(tilesList[tiles - 1].pos, new Vector2D(BaseTile.size, BaseTile.size)));
            }

            // Renders non-dead enemies.
            foreach (Enemy e in enemies)
            {
                if (!e.dead)
                {
                    e.Render(g);
                    if (e.path != null) e.path.Render(g);
                }
            }

            // Renders Flying Entities and updates them.
            foreach (FlyingEntity flyingEntity in flyingEntities)
            {
                g.FillRectangle(new SolidBrush(Color.DarkTurquoise), new Rectangle(flyingEntity.Pos, new Vector2D(BaseTile.size, BaseTile.size)));
                flyingEntity.Update(stopwatch.ElapsedMilliseconds);
                if (flyingEntity.goals != null)
                {
                    if (flyingEntity.goals.Count == 0) { flyingEntity.goals = flyingEntity.originalGoals; Console.WriteLine(flyingEntity.originalGoals.Count); }
                    else {
                        flyingEntity.goals.Peek().Draw(g);
                    }
                }
            }

            // Restarts stopwatch for elapsedseconds.
            stopwatch.Restart();
        }

        public void Update()
        {
            foreach (Tower t in towers) t.Update();
            foreach (Enemy e in enemies) if (!e.dead) e.Update();
            if (enemies.All(i => i.dead)) enemies = new List<Enemy>();
        }

        public void SpawnEnemy()
        {
            ResetAllVertices();
            Imp imp = new Imp(waveCount)
            {
                pos = startTile.pos,
                path = Path.GetPath(startTile, endTile)
            };
            Instance.enemies.Add(imp);
            foreach (Tower t in towers)
                t.nearbyEnemies.Add(imp);
        }

        /// Returns index of clicked Tile.
        public int GetIndexOfTile(Vector2D pos)
        {
            int index = (int)(pos.y / BaseTile.size) * tilesH;
            index += (int)(pos.x / BaseTile.size);
            return index;
        }

        /// Checks if any of the Tiles within a 2x2 space (around the mouseclick) has something built on it already.
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

        public void RecalculatePaths()
        {
            //BaseTile endTile = Instance.endTile;
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

        /// RecalculatePaths overload for after a Tower is placed. 
        /// This method will only recalculate the path if the current path is obstructed by the new Tower.
        public void RecalculatePaths(List<BaseTile> tilesToCheck)
        {
            foreach (Enemy enemy in Instance.enemies)
            {
                if (!enemy.dead)
                {
                    if (enemy.path.IsBlocked(tilesToCheck))
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

            // If there is a Tile above the specified Tile.
            if (up)
            {
                // Gets the Tile upwards of the current tile.
                BaseTile upTile = tilesList[GetIndexOfTile(tile.pos - new Vector2D(0, BaseTile.size))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (upTile.buildable) neighbours.Add(upTile);
                if (left)
                    neighbours.Add(tilesList[GetIndexOfTile(tile.pos + new Vector2D(-BaseTile.size, -BaseTile.size))]);
                if (right)
                    neighbours.Add(tilesList[GetIndexOfTile(tile.pos + new Vector2D(BaseTile.size, -BaseTile.size))]);

            }
            // If there is a Tile under the specified Tile.
            if (down)
            {
                // Gets the Tile under the current Tile.
                BaseTile downTile = tilesList[GetIndexOfTile(tile.pos + new Vector2D(0, BaseTile.size))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (downTile.buildable) neighbours.Add(downTile);
                if (left)
                    neighbours.Add(tilesList[GetIndexOfTile(tile.pos + new Vector2D(-BaseTile.size, BaseTile.size))]);
                if (right)
                    neighbours.Add(tilesList[GetIndexOfTile(tile.pos + new Vector2D(BaseTile.size, BaseTile.size))]);

            }
            // If there is a Tile to the left of the specified Tile.
            if (left)
            {
                // Gets the Tile to the left of the current Tile.
                BaseTile leftTile = tilesList[GetIndexOfTile(tile.pos - new Vector2D(BaseTile.size, 0))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (leftTile.buildable) neighbours.Add(leftTile);
            }
            // If theres a Tile to the right of the specified Tile.
            if (right)
            {
                // Gets the Tile to the right of the current Tile.
                BaseTile rightTile = tilesList[GetIndexOfTile(tile.pos + new Vector2D(BaseTile.size, 0))];
                // Adds the Tile to List of neighbours if its possible to build on the Tile.
                if (rightTile.buildable) neighbours.Add(rightTile);
            }
            // Returns list of neighbours.
            return neighbours;
        }

        public int AddGold(int amount)
        {
            return Gold += amount;
        }
        public int DeductGold(int amount)
        {
            return Gold -= amount;
        }
        /// <summary>
        /// Tags any entities contained in a std container that are within the
        /// radius of the vehicle specified.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="radius"></param>
        public void TagVehiclesWithinViewRange(FlyingEntity FlyingEntity, double radius)
        {
            foreach (FlyingEntity flyingEntity in flyingEntities)
            {

                // First clear any current tag.
                FlyingEntity.Tag = false;

                Vector2D to = flyingEntity.Pos - FlyingEntity.Pos;

                /* The bounding radius of the other is taken into account by adding it 
                   to the range. */
                double range = radius + flyingEntity.BoundingRadius;

                /* If entity within range, tag for further consideration. (working in
                distance-squared space to avoid sqrts) */
                if ((flyingEntity != FlyingEntity) && (to.LengthSq() < range * range))
                {
                    flyingEntity.Tag = true;
                }

            }// Next entity.
        }

        public static Vector2D PointToWorldSpace(Vector2D point, Vector2D agentHeading, Vector2D agentSide, Vector2D agentPosition)
        {
            Vector2D transPoint = point;
            C2DMatrix matTransform = new C2DMatrix();
            matTransform.Rotate(agentHeading, agentSide);
            matTransform.Translate(agentPosition.x, agentPosition.y);
            matTransform.TransformVector2Ds(ref transPoint);
            return transPoint;
        }
    }
}
