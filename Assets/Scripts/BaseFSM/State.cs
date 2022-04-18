using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFSM
{
    public abstract class State
    {

        public GameObject gameObject;
        protected Transform transform;

        protected StateMachine SM;
        
        public virtual void Start(GameObject gameObject, StateMachine SM){ // called when we enter this state
            this.gameObject = gameObject;
            this.transform = gameObject.transform;
            this.SM = SM;
            return;
        }
        public virtual void Execute(){ // execute the state at each frame
            return;
        }
        public virtual void Exit(){ // called when we leave this state
            return;
        }
    }
}