using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    float counter = 2.0f;
    Ray ray;
    RaycastHit hit;
    public GameObject Target;

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

        if (Input.GetMouseButtonDown (0)) {
          Debug.Log ("MouseDown");
          // Reset ray with new mouse position
          ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
          RaycastHit[] hits = Physics.RaycastAll (ray);
          foreach (RaycastHit hit in hits) {
              if (hit.collider.gameObject.tag == "Baby") {
                  Debug.Log("Nudging");
                  Target = hit.collider.gameObject;
                  PowerNudge();
              }
          }
        
        }
    }

    void Nudge()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f),0) * Random.Range(1f,7f));

    }

    public void PowerNudge()
    {
        Debug.Log("Nudging occurs");
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3f,3f), Random.Range(-2f,2f),0) * Random.Range(7f,14f));
    }
}
