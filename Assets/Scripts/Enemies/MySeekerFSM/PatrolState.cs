using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFSM;

namespace Enemies.MySeekerFSM{
    public class PatrolState : State
    {

        public float turnSpeed = 90f; // rotate 90 degrees per second
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

            TurnToFace(player.transform.position);
            
        }

        public override void Exit()
        {
            Debug.Log("FSM | Exited Patrol State");
        }

        #region "Movement methods"
        private void TurnToFace(Vector3 lookTarget){
            // get the direction to the target
            Vector3 directionToTarget = (lookTarget - transform.position).normalized;
            // get the angle between the direction to the target and the direction the enemy is facing
            float targetAngle = 90 - Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;

             if(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) <= 0.05f){ // if we are in a threshold snap to it, set the angle to the target angle
                transform.eulerAngles = Vector3.up * targetAngle; 
                return; // and return as we don't want to keep rotating
            }
            // else, turn the enemy to face the target
            else{
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
            }

        }
        #endregion
    }
}
