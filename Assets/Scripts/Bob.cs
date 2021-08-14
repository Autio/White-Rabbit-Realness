using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bob : MonoBehaviour
{
    bool goingUp;
    float yStart;
    public float oscillation = 8f;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        yStart = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = transform.GetComponent<Transform>().localPosition.y; 
        float yStep = Time.deltaTime * speed;
        if(!goingUp)
        {
            yStep = -yStep;
        }
        if (yPos - yStart > oscillation && goingUp)
        {
            goingUp = false;
        } 
        if (yStart - yPos > oscillation && !goingUp)
        {
            goingUp = true;
        }

        transform.position = new Vector2(transform.position.x, transform.position.y + yStep);
        
    }


}
