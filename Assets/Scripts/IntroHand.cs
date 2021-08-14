using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroHand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        GameController.Instance.GoToBirthing();
    }
}
