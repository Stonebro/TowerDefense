using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.FSM {
    class StateMachine<T> {
        T entity;
        public State<T> PrevState;
        public State<T> CurrentState;

        public StateMachine(T entity) {
            this.entity = entity;
        }

        public void ChangeState(State<T> newState) {
            if (PrevState == null) {
                newState.Enter(entity);
                PrevState = newState;
            }
            if (CurrentState != null) PrevState = CurrentState;
            CurrentState = newState;
            if (CurrentState.GetType() != PrevState.GetType()) {
                PrevState.Exit(entity);
                CurrentState.Enter(entity);
            }
        }

        public void Update() {
            if(CurrentState != null) CurrentState.Execute(entity);
        }

    }
}
