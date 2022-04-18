using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFSM
{
    public abstract class StateMachine : MonoBehaviour{
        protected State state;

        public void ChangeState(State state){ // change the state of the state machine
            if(this.state != null){
                this.state.Exit(); // exit the current state
            }
            this.state = state; // update to the new state
            this.state.Start(gameObject, this); // start coroutine for the new state
        }

        protected virtual void Update(){
            if(state != null){
                state.Execute(); // at each frame, execute the state's code
            }
        }
    }
}