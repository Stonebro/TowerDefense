using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Entities.Projectiles;

namespace TowerDefense.FSM {
    // Idle literally does nothing.
    class Idle : State<AttackDog> {
        public override void Enter(AttackDog t) {
            Console.WriteLine("Idle.");
        }

        public override void Execute(AttackDog t) {
        }

        public override void Exit(AttackDog t) {

        }

    }
}
