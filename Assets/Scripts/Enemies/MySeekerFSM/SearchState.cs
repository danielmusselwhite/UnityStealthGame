using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFSM;

namespace Enemies.MySeekerFSM{
    public class SearchState : State
    {

        private iSeeker seeker; // reference to our interface, holding important variables

        private float stateTimer; // timer until we revert back to patrol state
        private float moveTimer; // timer until we move to the another random angle

        private float targetAngle;

        public override void Start(GameObject gameObject, StateMachine SM)
        {
            Debug.Log("FSM | Entered Search State");
            base.Start(gameObject, SM);
            
            // get component of type "iSeeker" which is the interface holding our important values
            seeker = gameObject.GetComponents<iSeeker>()[0];

            seeker.SetSpotLightColour(seeker.searchColor); // set the spot light to the search color

            stateTimer = 10f;

            //pick a random angle
            targetAngle = Random.Range(0, 360);
        }

        public override void Execute()
        {
            if (seeker.canSeePlayer()){ // if we can now see the player, switch to chase state
                SM.ChangeState(new ChaseState());
                return;
            }
            // if the timer has run out, revert back to patrol state
            if(stateTimer <= 0){
                SM.ChangeState(new PatrolState());
                return;
            }

            // move forwards
            transform.position += transform.forward * seeker.speed * Time.deltaTime;

            // if moveTimer has run out, pick a new angle
            if(moveTimer <= 0){
                targetAngle = Random.Range(0, 360);
                moveTimer = Random.Range(2f,4f);
            }
            
            // turn to face the target angle
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, seeker.turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            
            // decrease timers
            stateTimer -= Time.deltaTime;
            moveTimer -= Time.deltaTime;
        }

        public override void Exit()
        {
            Debug.Log("FSM | Exited Search State");
        }
    }
}
