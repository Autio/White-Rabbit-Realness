using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    float counter = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            Nudge();
            counter = Random.Range(1f,4f);    
        }
        
    }

    void Nudge()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f),0) * Random.Range(1f,7f));

    }
}
