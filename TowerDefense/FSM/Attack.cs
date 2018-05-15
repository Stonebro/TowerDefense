using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities.Projectiles;
using TowerDefense.Tiles;
using TowerDefense.World;

namespace TowerDefense.FSM {
    class Attack : State<AttackDog> {
        public override void Enter(AttackDog t) {
        }

        public override void Execute(AttackDog t) {
            // Check if the dog is ready to attack, if the target's in range and if the target is not dead
            if (t.attackIntervalCounter % t.home.attackInterval == 0 && t.target.pos.Distance(t.pos) < (t.attackRange + 1f) * BaseTile.size && !t.target.dead) {
                if (t.target.health - t.home.attackPower <= 0) {
                    t.home.kills++;
                    GameWorld.Instance.AddGold(t.target.bounty);
                }
                t.target.health -= t.home.attackPower;
                t.attackIntervalCounter = 0;
                t.home.shotsFired++;
                // Fills a little black circle to show the dog attacked
                float range = (t.attackRange * 2 + 1);
                if (!t.target.dead) {
                    if (t.home.g != null)
                        t.home.g.FillEllipse(new SolidBrush(Color.FromArgb(200, 100, 100, 100)),
                            t.pos.x - t.attackRange * BaseTile.size,
                            t.pos.y - t.attackRange * BaseTile.size, range * BaseTile.size, range * BaseTile.size);
                }
            }
        }

        public override void Exit(AttackDog t) {

        }
    }

}
