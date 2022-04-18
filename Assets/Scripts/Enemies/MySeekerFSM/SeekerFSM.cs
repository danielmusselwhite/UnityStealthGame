using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFSM;

namespace Enemies.MySeekerFSM{
    public class SeekerFSM : StateMachine{

        public GameObject player;

        void Start(){
            player = GameObject.FindGameObjectWithTag("Player");
            ChangeState(new PatrolState()); // default to PatrolState
        }

        protected override void Update(){
            base.Update(); // call the base update to execute the states behaviour
        }

    }
}