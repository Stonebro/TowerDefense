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
    class AttackDog : Entity{
        public Tower home;
        public float attackIntervalCounter = 0;
        public BaseTile homePos;
        public float attackRange = 1f;
        public StateMachine<AttackDog> stateMachine;
        public Enemy target;

        public AttackDog(Tower home) {
            this.home = home;
            stateMachine = new StateMachine<AttackDog>(this);
            try {
                homePos = GameWorld.Instance.tilesList[GameWorld.Instance.GetIndexOfTile(home.position + new Vector2D(-BaseTile.size, BaseTile.size))];
            } catch {
                homePos = GameWorld.Instance.tilesList[GameWorld.Instance.GetIndexOfTile(home.position - new Vector2D(BaseTile.size*2, BaseTile.size*2))];
            }
            this.pos = homePos.pos;
        }

        public void Render(Graphics g) {
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(pos, new Vector2D(BaseTile.size, BaseTile.size)));
            float range = (attackRange * 2 + 1);
            g.DrawEllipse(new Pen(Color.FromArgb(128, 255, 0, 0)), pos.x - attackRange * BaseTile.size, pos.y - attackRange * BaseTile.size, range * BaseTile.size, range * BaseTile.size);
            g.DrawLine(new Pen(Color.FromArgb(200, 33, 33, 33), 2), home.position, (pos + new Vector2D(BaseTile.size / 2, BaseTile.size / 2)));
        }

        public void ChaseEnemy(Enemy enemy) {
            target = enemy;
            if (enemy.pos.Distance(pos) > attackRange * BaseTile.size && !enemy.dead)
                stateMachine.ChangeState(new Chase());
            else if (enemy.pos.Distance(pos) <= (attackRange + 1.5f) * BaseTile.size && !enemy.dead)
                AttackEnemy(enemy);
        }

        public void AttackEnemy(Enemy enemy) {
            stateMachine.ChangeState(new Attack());
        }

        public void Return() {
            if(this.pos == homePos.pos) {
                stateMachine.ChangeState(new Idle());
            } else stateMachine.ChangeState(new Return());
        }

        public void Update() {
            stateMachine.Update();
            if (attackIntervalCounter >= home.attackInterval) attackIntervalCounter = home.attackInterval;
            else attackIntervalCounter++;
        }
    }
}
