using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    
    public GameObject targetBaby;
    float distanceAllowed = 1.0f;
    Vector3 startingPos;
    Vector3 startingRot;
    Quaternion startingRotQ;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        startingRotQ = transform.rotation;
        startingRot = transform.rotation.eulerAngles;    
    }

    // Update is called once per frame
    void Update()
    {
        if(targetBaby != null)
            {
            float offset = targetBaby.transform.position.x - transform.position.x;
            // If baby is -5 then offset is -5
            // If baby is at 5 then offset is 5
            if(offset < -distanceAllowed)
            {
                // Flip and reposition sprite
                GetComponent<SpriteRenderer>().flipX = false;
            // transform.position = new Vector3(-0.9f, 5.4f, -7.319548f);
            }
            if(offset > distanceAllowed)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                transform.position = startingPos;
                transform.rotation = startingRotQ;
                
                //transform.rotation = new Quaternion(0,0,-20.1f,0);
            }
        }
    }
}
