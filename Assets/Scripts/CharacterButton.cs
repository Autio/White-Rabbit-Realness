using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    // Next or previous
    float timeOut = .4f;
    public bool nextCharacter = true;

    private void Update() {
        timeOut -= Time.deltaTime;
        
    }

    void OnMouseDown() {
        if(timeOut < 0)
        {
            if(nextCharacter)
            {
                GameController.Instance.NextCharacter();
            } else 
            {
                GameController.Instance.PreviousCharacter();
            }
        }
        timeOut = .4f;
    }

}
