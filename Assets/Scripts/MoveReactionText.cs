using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveReactionText : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 1f;
    Vector3 dir;
    void Start()
    {   
        float x = -1;
        // Random direction 
        if(Random.Range(0,10)< 5)
        {
            x = 1;
        }
        dir = new Vector3(x, Random.Range(-1f,1),0);

        speed = Random.Range(1f,2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * Time.deltaTime);
    }
}
