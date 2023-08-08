using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private float magnitude;
    public float crawlSpeed = 10f;
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
            // Debug.Log("hit "+hit.point);
        }
        // else, just move based on our current speed and magnitude
        else
        {
            
            transform.Translate(transform.forward * speed * smoothInputMagnitude * Time.deltaTime, Space.World);
        }
        // now collisions check to make sure we aren't overlapping anything
        GameUtils.checkCollisions(gameObject, transform, pCol);
    }

    // Update is called once per frame
    void Update()
    {
        #region "player controls"

        #region "Speed"
        speed = walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            speed = crawlSpeed;
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
}
