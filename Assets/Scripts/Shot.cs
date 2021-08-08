using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
                        
    private IEnumerator Shrink()
    {
        yield return new WaitForSeconds(8.0f);
        GetComponent<TMP_Text>().fontSize -= 5;
        GetComponent<RectTransform>().localScale *= 0.5f;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag == "Background")
        {
            StartCoroutine("AddGravity");
            StartCoroutine("Shrink");
        }    
    }
    

    
}
