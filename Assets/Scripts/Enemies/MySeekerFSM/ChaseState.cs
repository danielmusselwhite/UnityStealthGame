using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFSM;

namespace Enemies.MySeekerFSM{
    public class ChaseState : State
    {

        private iSeeker seeker; // reference to our interface, holding important variables
        private Vector3 lastKnownPosition;

        public override void Start(GameObject gameObject, StateMachine SM)
        {
            Debug.Log("FSM | Entered Chase State");
            base.Start(gameObject, SM);
            
            // get component of type "iSeeker" which is the interface holding our important values
            seeker = gameObject.GetComponents<iSeeker>()[0];

            seeker.spotLight.color = seeker.chaseColor; // set the spot light to the chase color
            lastKnownPosition = seeker.player.transform.position;
        }

        public override void Execute()
        {
            //if we can see the player
            if (seeker.canSeePlayer()){
                // update the last known position
                lastKnownPosition = new Vector3(seeker.player.transform.position.x, transform.position.y, seeker.player.transform.position.z);
            }
            // else, we can't see the player, if we have also reached his last known location, switch to search state
            else if(Vector3.Distance(lastKnownPosition, transform.position) <= 0.1f){
                SM.ChangeState(new SearchState());
                return;
            }

            // move towards the last known location (replace with A* later)
            seeker.TurnToFace(lastKnownPosition); // look at the players last known location
            transform.position = Vector3.MoveTowards(transform.position, lastKnownPosition, seeker.speed * Time.deltaTime); // move towards the player
        }

        public override void Exit()
        {
            Debug.Log("FSM | Exited Chase State");
        }

        #region "Movement methods"
        
        #endregion
    }
}
