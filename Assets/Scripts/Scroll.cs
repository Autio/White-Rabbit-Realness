using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
	public float speed = 2.0f;
    public Transform birthArea;
    
    // Start is called before the first frame update
    void Start()
    {
    //    CameraController.Instance.MoveToNextAnchor(birthArea);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(-Vector3.up * Time.deltaTime * speed);
        if(Input.GetKeyDown(KeyCode.Return))
        {
            transform.gameObject.AddComponent<Rigidbody>();
            transform.gameObject.GetComponent<Rigidbody>().drag = 3;
            Destroy(gameObject, 10);
            // Send camera down
            
        }
    }
}
