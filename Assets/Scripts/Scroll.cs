using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
	public float speed = 2.0f;
    bool triggered = false;
    public Transform birthArea;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 45);

    //    CameraController.Instance.MoveToNextAnchor(birthArea);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(-Vector3.up * Time.deltaTime * speed);
        if(Input.GetKeyDown(KeyCode.Return) && !triggered)
        {
            triggered = true;
            transform.gameObject.AddComponent<Rigidbody>();
            transform.gameObject.GetComponent<Rigidbody>().drag = 3;
            Destroy(gameObject, 8);
            
        }
    }
}
