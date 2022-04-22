using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static GameObject GetChildWithName(GameObject obj, string name) {
        Transform trans = obj.transform;
        Transform childTrans = trans. Find(name);
        if (childTrans != null) {
            return childTrans.gameObject;
        } else {
            return null;
        }
    }

    public static void checkCollisions(GameObject gameObject, Transform transform, CapsuleCollider pCol){
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
