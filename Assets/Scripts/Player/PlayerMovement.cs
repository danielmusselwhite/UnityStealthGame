using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed;
    private float direction;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float turnSpeed;
    public float gravity;

    private CapsuleCollider pCol;


    // Start is called before the first frame update
    void Start()
    {
        speed = walkSpeed;
        direction = 0;
        pCol = GetComponent<CapsuleCollider>();
    }

    // FixedUpdate for physics
    void FixedUpdate()
    {
        // if we will hit something based on our current speed and direction, snap to it
        Debug.DrawRay(transform.position, transform.forward * direction * speed, Color.red, 0.1f);
        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward * direction, out hit, speed * Time.deltaTime + pCol.radius))
        // {
        //     // transform.position = hit.point;
        //     Debug.Log("hit "+hit.point);
        // }
        // // else, just move based on our current speed and direction
        // else
        // {
            transform.Translate(0, 0, direction * speed * Time.deltaTime, Space.Self);
        // }
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
        direction = 0;
        if (Input.GetKey(KeyCode.W))
        {
            direction = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = -1;
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

        #region "player jumping control"
        
        #endregion
        
        #endregion


        #region "applying gravity"
        
        #endregion
    }

    private void checkCollisions(){
        //check if we're colliding with something
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pCol.radius);

        // for each collision box we are overlapping
        foreach(Collider col in hitColliders)
        {
           
            
            // if the collision box is a wall
            if(col.gameObject.tag == "Wall")
            {
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
