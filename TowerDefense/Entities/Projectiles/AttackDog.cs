using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities;
using TowerDefense.Entities.Enemies;
using TowerDefense.FSM;
using TowerDefense.Tiles;
using TowerDefense.Util;
using TowerDefense.World;

namespace TowerDefense.Entities.Projectiles {
    class AttackDog : Entity {
        // The Tower this dog belongs to and the BaseTile it calls home.
        public Tower home;
        public BaseTile homePos;
        public float attackIntervalCounter = 0;
        public float attackRange = 1f;
        public StateMachine<AttackDog> stateMachine;
        public Enemy target;

        public AttackDog(Tower home) {
            this.home = home;
            stateMachine = new StateMachine<AttackDog>(this);
            // Try to set the homePos to the square below the home Tower, if it's outside the playing field, make it above the Tower instead.
            try {
                homePos = GameWorld.Instance.tilesList[GameWorld.Instance.GetIndexOfTile(home.position + new Vector2D(-BaseTile.size, BaseTile.size))];
            } catch {
                homePos = GameWorld.Instance.tilesList[GameWorld.Instance.GetIndexOfTile(home.position - new Vector2D(BaseTile.size*2, BaseTile.size*2))];
            }
            this.pos = homePos.pos;
        }

        // Draws the dog and its attack range
        public void Render(Graphics g) {
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(pos, new Vector2D(BaseTile.size, BaseTile.size)));
            float range = (attackRange * 2 + 1);
            g.DrawEllipse(new Pen(Color.FromArgb(128, 255, 0, 0)), pos.x - attackRange * BaseTile.size, pos.y - attackRange * BaseTile.size, range * BaseTile.size, range * BaseTile.size);
            g.DrawLine(new Pen(Color.FromArgb(200, 33, 33, 33), 2), home.position, (pos + new Vector2D(BaseTile.size / 2, BaseTile.size / 2)));
        }

        // Chase the Enemy if it's outside attack range or attack it when it's within range.
        public void ChaseEnemy(Enemy enemy) {
            target = enemy;
            if (enemy.pos.Distance(pos) > (attackRange+0.5f) * BaseTile.size && !enemy.dead)
                stateMachine.ChangeState(new Chase()); // Change State to Chase
            else if (enemy.pos.Distance(pos) <= (attackRange + 0.5f) * BaseTile.size && !enemy.dead)
                AttackEnemy(enemy);
        }

        // Change State to Attack
        public void AttackEnemy(Enemy enemy) {
            stateMachine.ChangeState(new Attack());
        }

        // Idle when doggy's home, or Return home when he's not there yet.
        public void Return() {
            if(this.pos == homePos.pos) stateMachine.ChangeState(new Idle()); // Change State to Idle
            else stateMachine.ChangeState(new Return()); // Change State to Return
        }

        // Update the StateMachine and ready an attack with each tick.
        public void Update() {
            stateMachine.Update();
            if (attackIntervalCounter >= home.attackInterval) attackIntervalCounter = home.attackInterval;
            else attackIntervalCounter++;
        }
    }
}
