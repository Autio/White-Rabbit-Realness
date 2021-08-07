using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class GameController : Singleton<GameController>
{
    enum GameStates {starting, playing}
    GameStates gameState;

    // 0, 1, 2
    int activePhase = 0;

    // What are the bars? 
    // Active v passive
    // Expressive v inexpressive
    // Animal v human
    int[] gameBars = {0,0,0};
    List<int> questionsAsked;
    List<Question> questions;
    // How many questions does the player need to pass before the game is won? 
    int questionLimit = 10;

    // Player needs to stay within -limit and limit
    public int limit = 10;

    public GameObject questionText;
    public GameObject answerOption;
    // Preset anchors for where the answer options can spawn
    public GameObject[] answerOptionAnchors;

    List<GameObject> activeOptions;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameStates.starting;
        questionsAsked = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // In the game mode there are three phases of questions 
    // Each question has some amount of responses
    // Each response moves one or more bars in a direction by one or more points
    // If the bar amount is exceeded, then the game ends

    // Read in the next question
    // Stored in a JSON
    // Structure: 
    // { id, phase, question, answers: { answer, scores: [0,0,0] }}

    void NextQuestion(int activePhase, List<int> questionsAsked)
    {
        Question question;
        // The game has been passed
        if(questionsAsked.Count > questionLimit)
        {
            WinGame();
        }
        // Load the next question from the array
        while (true)
        {
            int r = Random.Range(0, questions.Count);
            // if(questions[r].id not in questionsAsked && questions[r].phase == activePhase)
            // {
            //     // This question hasn't been asked yet
            //     questionsAsked.Add(r);                
            // }
        }


        questionsAsked.Add(question.id);

        // Display question text
        
        // Display options one by one 
        for (int i = 0; i < (question.answers.Length); i++)
        {
            GameObject newAnswerOption = Instantiate(answerOption, answerOptionAnchors[i].transform.position, Quaternion.identity) as GameObject;
            activeOptions.Add(newAnswerOption);
            // Set text 
            newAnswerOption.GetComponent<TMP_Text>().text = MapLetter(i) + " " + question.answers[i];
        }
    }

    string MapLetter(int l)
    {
        if (l == 0)
        {
            return "A";
        }
        else if (l == 1)
        {
            return "B";
        }
        else if (l == 2)
        {
            return "C";
        }
        else if (l == 3)
        {
            return "D";
        }
        else
        {
            return "N/A";
        }
    }

    void ClearQuestion()
    {
        // Any existing question text and options need to be cleared out 
        foreach (GameObject a in activeOptions)
        {
            activeOptions.Remove(a);
            Destroy(a, 0.1f);
        }
    }

    void WinGame()
    {

    }


    bool CheckEnd()
    {
        // check all bar values against the limit
        for (int i = 0; i < gameBars.Length; i++)
        {
            if (gameBars[i] < -limit || gameBars[i] > limit)
            {
                // Game over
                // Display relevant end text
                // Need to know the bar and whether it was + or - 
                return true;
            } 
        }
        return false;
    }


}
