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
    private Vector3 lastSafePosition;


    // Start is called before the first frame update
    void Start()
    {
        speed = walkSpeed;
        direction = 0;
        pCol = GetComponent<CapsuleCollider>();
        lastSafePosition = transform.position;
    }

    // FixedUpdate for physics
    void FixedUpdate()
    {
        // move the player based on their input
        transform.Translate(0, 0, direction * speed * Time.deltaTime, Space.Self);
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

        // for each hitCollider
        for(int i = 0; i < hitColliders.Length; i++){
            // if this is our hitCollider, skip
            if(hitColliders[i].gameObject == gameObject)
                continue;

            Debug.Log(""+gameObject.name+" is colliding with "+hitColliders[i].gameObject.name);
            
            Vector3 left = transform.position - transform.right * pCol.radius;
            Vector3 right = transform.position + transform.right * pCol.radius;
            // Vector3 forward = transform.position + transform.forward * pCol.radius;
            // Vector3 back = transform.position - transform.forward * pCol.radius;
            // Vector3 top = transform.position + transform.up * pCol.height / 2;
            // Vector3 bottom = transform.position - transform.up * pCol.height / 2;

            //detect the bounds we are colliding with and do not exceed it


            if (left.x > hitColliders[i].bounds.min.x)
                transform.position = new Vector3(hitColliders[i].bounds.max.x + pCol.radius, transform.position.y, transform.position.z);
            if (right.x < hitColliders[i].bounds.max.x)
                transform.position = new Vector3(hitColliders[i].bounds.min.x - pCol.radius, transform.position.y, transform.position.z);
            // if (forward.z > hitColliders[i].bounds.max.z)
            //     transform.position = new Vector3(transform.position.x, transform.position.y, hitColliders[i].bounds.max.z);
            // if (back.z < hitColliders[i].bounds.min.z)
            //     transform.position = new Vector3(transform.position.x, transform.position.y, hitColliders[i].bounds.min.z);
            // if (top.y > hitColliders[i].bounds.max.y)
            //     transform.position = new Vector3(transform.position.x, hitColliders[i].bounds.max.y, transform.position.z);
            // if (bottom.y < hitColliders[i].bounds.min.y)
            //     transform.position = new Vector3(transform.position.x, hitColliders[i].bounds.min.y, transform.position.z);
        }

        //check if we are colliding to the right of the x


        //if we're colliding with something, move us back to the last safe position
        // if (hitColliders.Length > 1)
        //     transform.position= new Vector3(lastSafePosition.x, lastSafePosition.y, transform.position.z);
        // //we're not colliding with a wall, save our position
        // else
        //     lastSafePosition = transform.position;
    }
}
