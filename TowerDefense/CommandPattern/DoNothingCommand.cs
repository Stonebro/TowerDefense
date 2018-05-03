using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.CommandPattern {
    class DoNothingCommand : Command {

        public DoNothingCommand(IReceiver receiver) : base(receiver) {
            //Console.WriteLine("Not doing anything");
        }

        public override void Execute() {
            //Console.WriteLine("Not doing anything");
        }
    }
}
