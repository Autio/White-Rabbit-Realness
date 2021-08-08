using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    // Next or previous
    public bool nextCharacter = true;

    void OnMouseDown() {
        if(nextCharacter)
        {
            GameController.Instance.NextCharacter();
        } else 
        {
            GameController.Instance.PreviousCharacter();
        }
    }

}
