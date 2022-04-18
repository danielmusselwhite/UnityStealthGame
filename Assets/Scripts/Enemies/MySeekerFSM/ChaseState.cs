using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFSM;

namespace Enemies.MySeekerFSM{
    public class ChaseState : State
    {

        public float speed = 5f;
        private GameObject player;
        

        public override void Start(GameObject gameObject, StateMachine SM)
        {
            Debug.Log("FSM | Entered Chase State");
            base.Start(gameObject, SM);
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public override void Execute()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            // replace with A* later
            if(player != null)
            {
                transform.LookAt(player.transform); // look at the player
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); // move towards the player
            }

            if (Vector3.Distance(transform.position, player.transform.position) > 10f){ // if the player is exceeding a threshold of units of the enemy, switch to PatrolState
                SM.ChangeState(new PatrolState());
            }
        }

        public override void Exit()
        {
            Debug.Log("FSM | Exited Chase State");
        }
    }
}
