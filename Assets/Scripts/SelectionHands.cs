using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionHands : MonoBehaviour
{
     float scaleSpeed = 13;

    int counter = 0;
    void Start() {

          var sequence = DOTween.Sequence()
               .Append(transform.DOPunchScale(new Vector3(0.8f, 0.8f, 0.8f), scaleSpeed, 4, 0.3f));
          sequence.SetLoops(-1, LoopType.Yoyo);

    }
    void Update() {

    }
    
    
   void OnMouseDown() {
       
     transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 0.4f);
     transform.DOMove(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f);

     GameObject character = GameController.Instance.characterOptions[GameController.Instance.selectedCharacter];
     character.transform.DOScale(character.transform.localScale * 1.1f,.2f);
     // Start pulling out the baby
     if(counter > 1)
     {
          GameController.Instance.StartGame();
          character.transform.DOScale(character.transform.localScale * .8f,.2f);
          
     }
     
     counter++;

       
   }

}
