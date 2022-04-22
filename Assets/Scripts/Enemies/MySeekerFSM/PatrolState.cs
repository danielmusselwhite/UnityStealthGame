using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFSM;

namespace Enemies.MySeekerFSM{
    public class PatrolState : State
    {
        private iSeeker seeker; // reference to our interface, holding important variables
        private int targetWaypointIndex;
        private float targetAngle;
        
        

        public override void Start(GameObject gameObject, StateMachine SM)
        {
            Debug.Log("FSM | Entered Patrol State");
            base.Start(gameObject, SM);

            

            // get component of type "iSeeker" which is the interface holding our important values
            seeker = gameObject.GetComponents<iSeeker>()[0];

            seeker.spotLight.color = seeker.patrolColor; // set the spot light to the patrol color

            targetWaypointIndex = 1; // the index of the next waypoint to go to
            transform.LookAt(seeker.waypoints[targetWaypointIndex]); // look at the next waypoint
            targetAngle = transform.eulerAngles.y; // the angle we want to be facing
        }


        public override void Execute()
        {
            //if we can see the player, change to chase state
            if(seeker.canSeePlayer())
            {
                SM.ChangeState(new ChaseState());
                return;
            }
            
            // Movetowards the next position in the path, if we are looking at it
            if(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) <= 0.05f){
                transform.position = Vector3.MoveTowards(transform.position, seeker.waypoints[targetWaypointIndex], seeker.speed * Time.deltaTime); // move towards the next waypoint
                if (transform.position == seeker.waypoints[targetWaypointIndex]) // if we are at the next waypoint
                {
                    targetWaypointIndex = (targetWaypointIndex + 1) % seeker.waypoints.Length; // set the next waypoint to go to (modulo with length to loop back to start)

                    // get the direction to the next target
                    Vector3 directionToTarget = (seeker.waypoints[targetWaypointIndex] - transform.position).normalized;
                    // get the angle between the direction to the target and the direction the enemy is facing
                    targetAngle = 90 - Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;
                }
            }
            // else, turn to face the target angle
            else
            {
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, seeker.turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                // if we are sufficiently close to facing the target, set the angle to the target angle
                if(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) <= 0.05f)
                    transform.eulerAngles = Vector3.up * targetAngle;
            }
        }

        public override void Exit()
        {
            Debug.Log("FSM | Exited Patrol State");
        }
    }
}
