using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
	public float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(-Vector3.up * Time.deltaTime * speed);
        if(Input.GetKeyDown(KeyCode.Return))
        {
            transform.gameObject.AddComponent<Rigidbody>();
            Destroy(gameObject, 10);
        }
    }
}
