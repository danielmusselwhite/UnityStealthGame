using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameUtils;

public class Seeker : MonoBehaviour
{
    private Transform pathHolder;

    public float speed = 5f; // speed of the enemy
    public float waitTime = .2f; // time to wait at each waypoint
    public float turnSpeed = 90; // rotate 90 degrees per second

    // called during the DrawGizmos frame in the editor ONLY
    void OnDrawGizmos(){
        // getting the path attached to this seeker
        pathHolder = GameUtils.GetChildWithName(gameObject, "Path").transform; 
        //drawing a sphere and line for each waypoint to show the path in the editor
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(waypoint.position, 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }

    #region "Game"
    // Start is called before the first frame update
    void Start()
    {
        // getting the path attached to this seeker
        pathHolder = GameUtils.GetChildWithName(gameObject, "Path").transform; 

        // create array of the waypoints positions
        Vector3[] waypoints = new Vector3[pathHolder.childCount]; // same length as the number of children in the path
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); // want the waypoint to be on the same y axis as the enemy
        }

        // start the coroutine for following the path
        StartCoroutine(FollowPath(waypoints));
    }

    #region "Coroutines for movement"

    // Coroutine handling logic to make the enemy follow the path
    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints [0]; // set the position of the enemy to the first waypoint
        int targetWaypointIndex = 1; // the index of the next waypoint to go to
        transform.LookAt(waypoints[targetWaypointIndex]); // look at the next waypoint

        // loop forever
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[targetWaypointIndex], speed * Time.deltaTime); // move towards the next waypoint
            if (transform.position == waypoints[targetWaypointIndex]) // if we are at the next waypoint
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length; // set the next waypoint to go to (modulo with length to loop back to start)
                yield return new WaitForSeconds(waitTime); // wait for the waitTime, then...
                yield return StartCoroutine(TurnToFace(waypoints[targetWaypointIndex])); // ...start the coroutine to turn to face the next waypoint
            }
            yield return null; // return to the next frame
        }
    }

    // Coroutine handling logic to make the enemy turn to face the next waypoint
    IEnumerator TurnToFace(Vector3 lookTarget){
        // get the direction to the target
        Vector3 directionToTarget = (lookTarget - transform.position).normalized;
        // get the angle between the direction to the target and the direction the enemy is facing
        float targetAngle = 90 - Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;

        // loop until we are sufficiently close to facing the target
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null; // return to the next frame
        }
        transform.eulerAngles = Vector3.up * targetAngle; // set the angle to the target angle
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

}
