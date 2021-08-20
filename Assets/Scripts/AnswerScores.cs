using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScores : MonoBehaviour
{
    public int[] scores = {0,0,0};
    
    public void AnswerChosen()
    {
        if(GameController.Instance.gameState == GameController.GameStates.playing)
        {
            GameController.Instance.AnswerChosen(scores, this.gameObject);
        }
    }
}