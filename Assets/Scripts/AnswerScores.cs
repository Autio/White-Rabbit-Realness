using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScores : MonoBehaviour
{
    public int[] scores = {0,0,0};
    
    public void AnswerChosen()
    {
        GameController.Instance.AnswerChosen(scores);
    }
}