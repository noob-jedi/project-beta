using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateProperties
{
    public class StateMachine<T>
    { 
        public State<T> CurrentState { get; private set; }
        public T Owner;
        
        public StateMachine(T _own)
        {
            Owner = _own;
            // Initial State = null
            CurrentState = null;
        }

        public void ChangeState(State<T> newState)
        {
            if(CurrentState != null)
            {
                CurrentState.ExitState(Owner);
            }
            CurrentState = newState;
            CurrentState.EnterState(Owner);

        }
        public void Update()
        {
            if(CurrentState != null)
            {
                CurrentState.UpdateState(Owner);
            }
        }
    }

    public abstract class State<T>
    {
        public abstract void EnterState(T _owner);
        public abstract void UpdateState(T _owner);
        public abstract void ExitState(T _owner);


    }
}
