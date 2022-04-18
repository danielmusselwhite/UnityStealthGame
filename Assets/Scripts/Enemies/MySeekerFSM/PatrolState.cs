using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFSM;

namespace Enemies.MySeekerFSM{
    public class PatrolState : State
    {

        public float speed = 5f;
        private GameObject player;

        public override void Start(GameObject gameObject, StateMachine SM)
        {
            Debug.Log("FSM | Entered Patrol State");
            base.Start(gameObject, SM);
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public override void Execute()
        {
            // TODO - replace with line of sight
            if(Vector3.Distance(transform.position, player.transform.position) < 10f){ // if the player is within a threshold of units of the enemy, switch to ChaseState
                SM.ChangeState(new ChaseState());
            }
        }

        public override void Exit()
        {
            Debug.Log("FSM | Exited Patrol State");
        }
    }
}
