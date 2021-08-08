using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : Singleton<GameController>
{
    enum GameStates {starting, birthing, playing}
    GameStates gameState;
    public GameObject playCanvas;
    public Transform[] cameraAnchors;
    public Transform[] TitleTexts;
    int titleTextTriggers = 0;

    // 0, 1, 2
    int activePhase = 0;

    // What are the bars? 
    // Active v passive
    // Expressive v inexpressive
    // Animal v human
    int[] gameBars = {0,0,0};
    List<int> questionsAsked;
    Question[] qs;
    // How many questions does the player need to pass before the game is won? 
    int questionLimit = 10;

    // Player needs to stay within -limit and limit
    public int limit = 10;

    public GameObject questionText;
    public GameObject answerPanel;
    public GameObject answerOption;
    // Preset anchors for where the answer options can spawn
    public GameObject[] answerOptionAnchors;

    List<GameObject> activeOptions;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameStates.starting;
        questionsAsked = new List<int>();
        activeOptions = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == GameStates.playing)
        {

        }
        if(gameState == GameStates.birthing)
        {
            // Load the questions to have them ready
            qs = GetComponent<JSONReader>().questions.questions;

            if(Input.GetKeyDown(KeyCode.Return))
            {
                CameraController.Instance.MoveToNextAnchor(cameraAnchors[1]);
                gameState = GameStates.playing;
                StartCoroutine("ShowCanvas", 2.0f);

            }
        }

        if(gameState == GameStates.starting)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                // Skip the start
                gameState = GameStates.birthing;
                CameraController.Instance.MoveToNextAnchor(cameraAnchors[0]);
            }

            if(Time.time > 0.1f && titleTextTriggers == 0)
            {
                TitleTexts[0].GetComponent<Shot>().Go();
                titleTextTriggers = 1;
            }
            if(Time.time > 0.7f && titleTextTriggers == 1)
            {
                TitleTexts[1].GetComponent<Shot>().Go();
                titleTextTriggers = 2;
            }
            if(Time.time > 1.5f && titleTextTriggers == 2)
            {
                TitleTexts[2].GetComponent<Shot>().Go();
                titleTextTriggers = 3;
            }
        }
    }

    private IEnumerator ShowCanvas(float delay = 2.0f)
    {
        yield return new WaitForSeconds(delay);
        playCanvas.SetActive(true);
        NextQuestion(activePhase, questionsAsked);
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
        Question question = null;
        // The game has been passed
        if(questionsAsked.Count > questionLimit)
        {
            WinGame();
        }
        // Load the next question from the array
        int t = 100;
        while (t > 0)
        {
            int r = Random.Range(0, qs.Length);
            if(!questionsAsked.Contains(qs[r].id))// && questions[r].phase == activePhase)
            {
                 questionsAsked.Add(r); 
                 question = qs[r];  
                 t = -1;       
                 break;
            }
            t--;
        }

        Debug.Log("Question picked " + question.title);

        if (t == 0)
        {
       //     return;
        }

        // Display question text
        questionText.GetComponent<TMP_Text>().text = question.title;
        // Display options one by one 
        for (int i = 0; i < (question.answers.Length); i++)
        {
            GameObject newAnswerOption = Instantiate(answerOption, new Vector3(), Quaternion.identity) as GameObject;
            activeOptions.Add(newAnswerOption);
            newAnswerOption.transform.SetParent(answerPanel.transform.Find("PanelInterim").transform, false);

            // Set text 
            newAnswerOption.transform.Find("Text").GetComponent<TMP_Text>().text = MapLetter(i) + ") " + question.answers[i].answer;

            // Force refresh
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
            Destroy(a, 0.01f);
        }
        activeOptions.Clear();
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