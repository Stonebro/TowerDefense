﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.CommandPattern {
    public abstract class Command {
        protected IReceiver receiver;

        public Command(IReceiver receiver) {
            this.receiver = receiver;
        }

        public abstract void Execute();
    }
}
