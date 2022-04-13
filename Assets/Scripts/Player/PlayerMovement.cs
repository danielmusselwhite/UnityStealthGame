using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed;
    private float magnitude;
    public float walkSpeed = 20f;
    public float runSpeed = 40f;
    public float turnSpeed;

    public float smoothMoveTime = .1f;
    private float smoothInputMagnitude;
    private float smoothMoveVelocity;

    private CapsuleCollider pCol;


    // Start is called before the first frame update
    void Start()
    {
        speed = walkSpeed;
        magnitude = 0;
        pCol = GetComponent<CapsuleCollider>();
    }

    // FixedUpdate for physics
    void FixedUpdate()
    {
        // if we will hit something based on our current speed and magnitude, snap to it
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, magnitude, ref smoothMoveVelocity, smoothMoveTime); //smoothing magnitude so we accelerates/decelerates
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward * smoothInputMagnitude, out hit, speed * Time.deltaTime + pCol.radius))
        {
            transform.Translate(transform.forward * smoothInputMagnitude * (hit.distance - pCol.radius), Space.World);
            Debug.Log("hit "+hit.point);
        }
        // else, just move based on our current speed and magnitude
        else
        {
            
            transform.Translate(transform.forward * speed * smoothInputMagnitude * Time.deltaTime, Space.World);
        }
        // now collisions check to make sure we aren't overlapping anything
        checkCollisions();
    }

    // Update is called once per frame
    void Update()
    {
        #region "player controls"

        #region "Speed"
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = walkSpeed;
        }
        #endregion

        #region "Movement"
        magnitude = 0;
        if (Input.GetKey(KeyCode.W))
        {
            magnitude = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            magnitude = -1;
        }
        #endregion
        #region "Turning"
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation *= Quaternion.Euler(0, -turnSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation *= Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0);
        }
        #endregion
    
        #endregion
    }

    private void checkCollisions(){
        //check if we're colliding with something
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pCol.radius);

        // for each collision box we are overlapping
        foreach(Collider col in hitColliders)
        {
           
            
            // if the collision box is not ours...
            if(col.gameObject != gameObject){
                Debug.Log(gameObject.name + "collided with wall " + col.gameObject.name);
                // if we are colliding with the back of the wall, snap to the back of the wall
                if(transform.position.z < col.bounds.min.z)
                {
                    Debug.Log("Collided with back of "+col.gameObject.name);
                    transform.position = new Vector3(transform.position.x, transform.position.y, col.transform.position.z - col.transform.localScale.z/2 - pCol.radius);
                }
                // if we are colliding with the front of the wall, snap to the front of the wall
                else if(transform.position.z > col.bounds.max.z)
                {
                    Debug.Log("Collided with front of "+col.gameObject.name);
                    transform.position = new Vector3(transform.position.x, transform.position.y, col.transform.position.z + col.transform.localScale.z/2 + pCol.radius);
                }

                // if we are colliding with the left side of the wall, snap to the left side of the wall
                if(transform.position.x < col.bounds.min.x)
                {
                    Debug.Log("Collided with left side of "+col.gameObject.name);
                    transform.position = new Vector3(col.transform.position.x - col.transform.localScale.x/2 - pCol.radius, transform.position.y, transform.position.z);
                }
                // if we are colliding with the right side of the wall, snap to the right side of the wall
                else if(transform.position.x > col.bounds.max.x)
                {
                    Debug.Log("Collided with right side of "+col.gameObject.name);
                    transform.position = new Vector3(col.transform.position.x + col.transform.localScale.x/2 + pCol.radius, transform.position.y, transform.position.z);
                }
            }
        }
    }
}
