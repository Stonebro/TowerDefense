using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities.Projectiles;

namespace TowerDefense.FSM {
    class Idle : State<AttackDog> {
        public override void Enter(AttackDog t) {
            Console.WriteLine("Idle.");
        }

        public override void Execute(AttackDog t) {
            //if (t.attackIntervalCounter != t.attackInterval) t.attackIntervalCounter++;
        }

        public override void Exit(AttackDog t) {

        }

    }
}
