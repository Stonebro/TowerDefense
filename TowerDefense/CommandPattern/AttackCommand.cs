using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.CommandPattern {
    class AttackCommand : Command {
        public AttackCommand(IReceiver receiver) : base(receiver) {
        }

        public override void Execute() {
            Console.WriteLine("Attacking");
        }
    }
}
