using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionHands : MonoBehaviour
{

    int counter = 0;
    
    
    
   void OnMouseDown() {
       
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 0.4f);
        transform.DOMove(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f);

       GameObject character = GameController.Instance.characterOptions[GameController.Instance.selectedCharacter];
       character.transform.DOScale(character.transform.localScale * 1.1f,.2f);
       // Start pulling out the baby
       if(counter > 1)
       {
            GameController.Instance.StartGame();
           
       }
       
       counter++;

       
   }

}
