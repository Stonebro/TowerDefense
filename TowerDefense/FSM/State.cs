using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.FSM {
    class State<T> {
        public virtual void Enter(T t) {

        }

        public virtual void Execute(T t) {

        }

        public virtual void Exit(T t) {

        }
    }
}
