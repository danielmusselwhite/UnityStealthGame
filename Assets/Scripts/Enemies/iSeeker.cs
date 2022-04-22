using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iSeeker : MonoBehaviour
{
    public Transform pathHolder;
    public Vector3[] waypoints;
    public float viewDistance = 10f; // how far the enemy can see
    public float viewAngle = 90f; // how wide the enemy can see
    public float waitTime = .2f; // time to wait at each waypoint
    public Light spotLight;
    public Color chaseColor;
    public Color patrolColor;

    public float turnSpeed = 90f; // rotate 90 degrees per second
    public float speed = 5f;

    public GameObject player; // player we are seeking

    void Start(){
        // getting the path attached to this seeker
        pathHolder = GameUtils.GetChildWithName(gameObject, "Path").transform; 

        // create array of the waypoints positions
        waypoints = new Vector3[pathHolder.childCount]; // same length as the number of children in the path
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); // want the waypoint to be on the same y axis as the enemy
        }

        // get the player
        player = GameObject.FindGameObjectWithTag("Player");
        
        // getting the spotLight attached to this seeker
        spotLight = GameUtils.GetChildWithName(gameObject, "SpotLight").GetComponent<Light>();
        viewAngle = spotLight.spotAngle; // seekers view angle is the same as the spot light's angle

        // getting the path attached to this seeker
        pathHolder = GameUtils.GetChildWithName(gameObject, "Path").transform; 

    }

     void OnDrawGizmos(){
        // getting the path attached to this seeker
        if(!Application.isPlaying){
            pathHolder = GameUtils.GetChildWithName(gameObject, "Path").transform;
            waypoints = new Vector3[pathHolder.childCount]; // same length as the number of children in the path
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = pathHolder.GetChild(i).position;
                waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); // want the waypoint to be on the same y axis as the enemy
            }
        }
        //drawing a sphere and line for each waypoint to show the path in the editor
        Vector3 startPosition = waypoints[0];
        Vector3 previousPosition = startPosition;
        foreach (Vector3 waypoint in waypoints)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(waypoint, 0.2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(previousPosition, waypoint);
            previousPosition = waypoint;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
        
        

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(viewAngle / 2, transform.up) * transform.forward * viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-viewAngle / 2, transform.up) * transform.forward * viewDistance);
    }

    public bool canSeePlayer()
        {
            // if the player is in the view distance
            if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
            {
                // if the player is in the view angle
                if (Vector3.Angle(transform.forward, (player.transform.position - transform.position).normalized) < viewAngle / 2)
                {
                    // if the player is in the line of sight
                    if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit, ~(1 << LayerMask.NameToLayer("Player"))))
                    {
                        // if the player is the hit object
                        if (hit.transform.CompareTag("Player"))
                        {
                            // Debug.Log(gameObject.name+" can see player");
                            return true;
                        }
                    }
                }
            }
            return false;
        }
}