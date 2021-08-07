using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    // WHITE RABBIT REALNESS
    public Transform target;
    public float force = 10;
    public void Go() {
        GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position) * force);
    }
    private void Update() {
        if(Input.GetKey(KeyCode.G))
        {
         //   Go();
        }
    }


    private IEnumerator AddGravity()
    {
        yield return new WaitForSeconds(1.2f);
        GetComponent<Rigidbody>().freezeRotation = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
                        


    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag == "Background")
        {
            StartCoroutine("AddGravity");
        }    
    }
    

    
}
