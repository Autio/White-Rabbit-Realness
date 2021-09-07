using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : Singleton<GameController>
{

    public enum GameStates {starting, birthing, playing, transitioning, ending}
    public GameStates gameState;
    public GameObject playCanvas;
    public Transform[] cameraAnchors;
    public Transform[] TitleTexts;
    public Transform[] scoreTexts;
    int titleTextTriggers = 0;

    // 0, 1, 2
    int activePhase = 0;

    // What are the bars? 
    // Active v passive
    // Expressive v inexpressive
    // Animal v human
    public int[] gameBars = {0,0,0};
    public Bar[] bars;
    List<int> questionsAsked;
    Question[] qs;
    Endings endings;
    // How many questions does the player need to pass before the game is won? 
    int questionLimit = 10;

    // Player needs to stay within -limit and limit
    public int limit = 5;

    public GameObject questionText;
    public GameObject answerPanel;
    public GameObject answerOption;
    // Preset anchors for where the answer options can spawn
    public GameObject[] answerOptionAnchors;

    List<GameObject> activeOptions;

    public int selectedCharacter;
    public GameObject[] characterOptions;
    public GameObject barGUI;

    public GameObject endText;
    float startTimer = 92;

    public Color chosenAnswerColor;
    public Color chosenTextColor;

    public GameObject reactionTextPrefab;

    private int gongCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameStates.starting;
        questionsAsked = new List<int>();
        activeOptions = new List<GameObject>();
        selectedCharacter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == GameStates.playing)
        {
            StartGame();
        }
        if(gameState == GameStates.birthing)
        {
            // Load the questions to have them ready
            qs = GetComponent<JSONReader>().questions.questions;
            endings = GetComponent<JSONReader>().endings;

            if(Input.GetKeyDown(KeyCode.Return))
            {
                CameraController.Instance.MoveToNextAnchor(cameraAnchors[1]);
                gameState = GameStates.playing;
                StartCoroutine("ShowCanvas", 2.0f);
            }
        }

        if(gameState == GameStates.starting)
        {
            startTimer -= Time.deltaTime;
            if(startTimer < 0)
            {
                GoToBirthing();
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                GoToBirthing();
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
            if(Time.time > 1.2f && titleTextTriggers == 2)
            {
                TitleTexts[2].GetComponent<Shot>().Go();
                titleTextTriggers = 3;
            }
        }
    }

    public void GoToBirthing()
    {
        // Skip the start
        gameState = GameStates.birthing;
        CameraController.Instance.MoveToNextAnchor(cameraAnchors[0]);
    }

    public void StartGame()
    {
        if(gameState != GameStates.playing)
        {
            // Plop birth sound
            int[] birthSoundRange = {0, 3};
            BaseSoundManager.Instance.PlaySoundByIndex(Random.Range(birthSoundRange[0], birthSoundRange[1]), Vector3.zero);
            BaseSoundManager.Instance.PlaySoundByIndex(4, Vector3.zero);
            

            CameraController.Instance.MoveToNextAnchor(cameraAnchors[1]);
            gameState = GameStates.playing;

            characterOptions[selectedCharacter].transform.DOMove(new Vector3(
                GameObject.Find("BabyBirthTarget").transform.position.x,
                GameObject.Find("BabyBirthTarget").transform.position.y,
                characterOptions[selectedCharacter].transform.position.z),
                3.2f
                );
            
            GameObject.Find("LadyHead").GetComponent<Head>().targetBaby = characterOptions[selectedCharacter];
            StartCoroutine("ShowCanvas", 2.0f);
            StartCoroutine("MaterializeBaby", 3.0f);
            
        }
    }
    

    // Cycle to the next available character or loop around
    public void NextCharacter()
    {
        BaseSoundManager.Instance.PlaySoundByIndex(7, Vector3.zero);

        Debug.Log("Picking next character");
        float transitionDuration = .39f;
        Transform characterTransform = characterOptions[selectedCharacter].transform;
        // Move old character away
        
        characterTransform.DOMove(new Vector3(characterTransform.position.x + 20, 
                            characterTransform.position.y, 
                            characterTransform.position.z), transitionDuration);

        // Pick the new character
        selectedCharacter++;
        if(selectedCharacter > characterOptions.Length - 1)
        {
            selectedCharacter = 0;
        }
        Debug.Log("Selected character " + selectedCharacter);


        // Swap out the old character and swap in the new one
        characterTransform = characterOptions[selectedCharacter].transform;

        // Make sure they start from the right place
        characterTransform.position = new Vector3(-20, 
                                                characterTransform.position.y,
                                                characterTransform.position.z);
        
        // Then tween them in
        characterTransform.DOMove(new Vector3(0, 
                            characterTransform.position.y, 
                            characterTransform.position.z), transitionDuration);

    }

    public void PreviousCharacter()
    {
        BaseSoundManager.Instance.PlaySoundByIndex(8, Vector3.zero);

        Debug.Log("Picking previous character");
        float transitionDuration = .6f;
        Transform characterTransform = characterOptions[selectedCharacter].transform;
        // Move old character away
        characterTransform.DOMove(new Vector3(characterTransform.position.x - 20, 
            characterTransform.position.y, 
            characterTransform.position.z), transitionDuration);       

        selectedCharacter--;
        if(selectedCharacter < 0)
        {
            selectedCharacter = characterOptions.Length - 1;
        }

        // Swap out the old character and swap in the new one
        characterTransform = characterOptions[selectedCharacter].transform;

        // Make sure they start from the right place
        characterTransform.position = new Vector3(20, 
                                                characterTransform.position.y,
                                                characterTransform.position.z);
        
        // Then tween them in
        characterTransform.DOMove(new Vector3(0, 
                            characterTransform.position.y, 
                            characterTransform.position.z), transitionDuration);


    }

    public void GoToVeryEnd()
    {
        CameraController.Instance.MoveToNextAnchor(cameraAnchors[cameraAnchors.Length - 1]);

    }

    private IEnumerator ShowCanvas(float delay = 1.9f)
    {
        yield return new WaitForSeconds(delay);
        playCanvas.SetActive(true);
        barGUI.SetActive(true);
        Question q = NextQuestion(activePhase, questionsAsked);
        StartCoroutine(CreateAnswer(q, .16f));
    }

    private IEnumerator HideCanvas(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        playCanvas.SetActive(false);
        yield return new WaitForSeconds(.3f);
        barGUI.SetActive(false);

    }

    private IEnumerator MaterializeBaby(float delay = 3.0f)
    {
        characterOptions[selectedCharacter].transform.Find("FaceAnchor").Find("babyface1").gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);
        characterOptions[selectedCharacter].layer = 3;
        Rigidbody rb = characterOptions[selectedCharacter].AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = false;
        characterOptions[selectedCharacter].GetComponent<Baby>().enabled = true;
        characterOptions[selectedCharacter].GetComponent<BoxCollider>().enabled = true;
    }
    
    // In the game mode there are three phases of questions 
    // Each question has some amount of responses
    // Each response moves one or more bars in a direction by one or more points
    // If the bar amount is exceeded, then the game ends

    // Read in the next question
    // Stored in a JSON
    // Structure: 
    // { id, phase, question, answers: { answer, scores: [0,0,0] }}

    Question NextQuestion(int activePhase, List<int> questionsAsked)
    {
        gameState = GameStates.playing;

        ClearQuestion();
        Question question = null;

        if(CheckEnd())
        {
            return null;
        }
        // The game has been passed
        if(questionsAsked.Count > questionLimit)
        {
            WinGame();
            return null;
        }

        int t = 100;
        if(questionsAsked.Count == questionLimit)
        {
            // Final question
            while (t > 0)
            {
                int r = Random.Range(0, qs.Length);
                if(!questionsAsked.Contains(qs[r].id) && qs[r].phase == 2)
                {
                    questionsAsked.Add(r); 
                    question = qs[r];  
                    t = -1;       
                    break;
                }
                t--;
            }
        } else {
            // Load the next question from the array
            while (t > 0)
            {
                int r = Random.Range(0, qs.Length);
                if(!questionsAsked.Contains(qs[r].id) && qs[r].phase != 2)// && questions[r].phase == activePhase)
                {
                    questionsAsked.Add(r); 
                    question = qs[r];  
                    t = -1;       
                    break;
                }
                t--;
            }
        }
        Debug.Log("Question picked " + question.title);

        if (t == 0)
        {
            return null;
        }

        
        // Display options one by one 
        return question;
    }

    private IEnumerator CreateAnswer(Question question, float delay = .5f)
    {
        yield return new WaitForSeconds(0.16f);
        for (int i = 0; i < (question.answers.Length); i++)
        {
            GameObject newAnswerOption = Instantiate(answerOption, new Vector3(), Quaternion.identity) as GameObject;
            activeOptions.Add(newAnswerOption);
            newAnswerOption.transform.SetParent(answerPanel.transform.Find("PanelInterim").transform, false);
            newAnswerOption.GetComponent<AnswerScores>().scores = question.answers[i].scores;

        }
        for (int i = 0; i < (question.answers.Length); i++)
        {
            activeOptions[i].transform.Find("Text").GetComponent<TMP_Text>().text = MapLetter(i) + ") " + question.answers[i].answer;
            yield return new WaitForSeconds(delay);
        }
        
    }

    public void AnswerChosen(int[] scores, GameObject chosenAnswer)
    {
        // Remove the other options from view
        ClearQuestionsExcept(chosenAnswer);
        // Make the chosen answer pop out
        chosenAnswer.GetComponent<Image>().color = chosenAnswerColor;
        chosenAnswer.transform.Find("Text").GetComponent<TMP_Text>().color = chosenTextColor;
        
        // Play sound, make sure it's not the same as before
        BaseSoundManager.Instance.PlaySoundByIndex(5 + gongCounter, Vector3.zero);
        if(gongCounter == 0)
        {
            gongCounter = 1;
        } else 
        {
            gongCounter = 0;
        }

        // Create reaction texts
        if(scores[0] != 0)
        {
            Debug.Log("Creatin'!");

            GameObject newTextGO = Instantiate(reactionTextPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newTextGO.transform.SetParent(playCanvas.transform);
            newTextGO.GetComponent<RectTransform>().localPosition = new Vector3(0, -270, 0);
            newTextGO.transform.Find("Text").GetComponent<Text>().text = "Ta-dah!"; // Dynamic text based on the bar

            // Make up to three texts, space them accordingly

            // Set their targets to the right bars

            // have the texts move

            // Should be UI object
            // GameObject rt = Instantiate(reactionTextPrefab, GameObject.Find("Reaction").transform.position, Quaternion.identity);
            // Destroy(rt, 2f);
        }

        int[] oldBarValues = {0,0,0};
        oldBarValues[0] = gameBars[0];
        oldBarValues[1] = gameBars[1];
        oldBarValues[2] = gameBars[2];

        gameBars[0] += scores[0];
        gameBars[1] += scores[1];
        gameBars[2] += scores[2];
        float level = 0;


        // Bars can be 
        // Move the bars appropriately
        if(scores[0] != 0)
        {
            level = Mathf.Abs(gameBars[0] / (float) limit);
            // Active
            if(gameBars[0] >= 0)
            {
                bars[0].HandlePctChanged(level);
                bars[1].HandlePctChanged(0);

            }
            // Passive
            if(gameBars[0] <= 0)
            {
                bars[0].HandlePctChanged(0);
                bars[1].HandlePctChanged(level);
            }
        }
        if(scores[1] != 0)
        {
            level = Mathf.Abs(gameBars[1] / (float) limit);
            // Expressive
            if(gameBars[1] >= 0)
            {
                bars[3].HandlePctChanged(0);
                bars[2].HandlePctChanged(level);
            }
            // Inexpressive
            if(gameBars[1] <= 0)
            {
                bars[2].HandlePctChanged(0);
                bars[3].HandlePctChanged(level);
            }
        }
        if(scores[2] != 0)
        {
            level = Mathf.Abs(gameBars[2] / (float) limit);
            // Animal
            if(gameBars[2] >= 0)
            {
                bars[5].HandlePctChanged(0);
                bars[4].HandlePctChanged(level);
            }
            // Human
            if(gameBars[2] <= 0)
            {
                bars[4].HandlePctChanged(0);
                bars[5].HandlePctChanged(level);
            }
        }

        scoreTexts[0].GetComponent<TMP_Text>().text = gameBars[0].ToString();
        scoreTexts[1].GetComponent<TMP_Text>().text = gameBars[1].ToString();
        scoreTexts[2].GetComponent<TMP_Text>().text = gameBars[2].ToString();

        StartCoroutine(TransitionToNextQuestions());  // NextQuestion(activePhase, questionsAsked);
    }

    private IEnumerator TransitionToNextQuestions()
    {
        gameState = GameStates.transitioning;
        yield return new WaitForSeconds(1.6f);
        Question question = NextQuestion(activePhase, questionsAsked);
        // Display question text
        questionText.GetComponent<TMP_Text>().text = question.title;
        ClearQuestion();

        StartCoroutine(CreateAnswer(question, .16f));
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

    void ClearQuestionsExcept(GameObject g)
    {
        List<GameObject> toRemove = new List<GameObject>();
        // Any existing question text and options need to be cleared out 
        for (int i = 0; i< activeOptions.Count; i++)
        {
            GameObject a = activeOptions[i];
            if(a != g){
                a.transform.Find("Text").GetComponent<TMP_Text>().text = "";
                Destroy(a, 0.01f);
                toRemove.Add(a);
            }
        }
        foreach(GameObject a in toRemove)
        {
            activeOptions.Remove(a);
        }
        
    }

    void ClearQuestion()
    {
        // Any existing question text and options need to be cleared out 
        foreach (GameObject a in activeOptions)
        {
            Debug.Log("Clearing " + a.transform.Find("Text").GetComponent<TMP_Text>().text);
            a.transform.Find("Text").GetComponent<TMP_Text>().text = "";
            Destroy(a, 0.01f);
        }
        activeOptions.Clear();
    }


    void WinGame()
    {
        gameState = GameStates.ending;
        // Hide GUI
        questionText.GetComponent<TMP_Text>().text = "Your baby is growing up.";

        // Happy ending
        endText.GetComponent<TMP_Text>().text = endings.EndingText(6);

        int threshold = 2; // How much leeway does the player have to get a good ending

        // Ending if the scores are a bit too close 
        string end = "Congrats! Your parenting style is above average (at least in this game). But you do have some minor problems in regulating";
        string check = end;
        
        int extras = 0;
        if(gameBars[0] > (limit - threshold) || gameBars[0] < (-limit + threshold))
        {
            end += " your activity level, ";
            extras++;
        }
        if(gameBars[1] > (limit  - threshold) || gameBars[1] < (-limit + threshold))
        {
            if(extras > 0)
            {
                end += " and your emotional expressiveness, ";
            } else
            {
                end += " your emotional expressiveness, ";
            }
            
        }
        if(gameBars[2] > (limit  - threshold) || gameBars[2] < (limit + threshold))
        {
            if(extras > 0)
            { 
                end += " and how animalistic vs humanoid you behave, ";
            }
            else
            {
                end += " how animalistic vs humanoid you behave, ";
            }
        }

        end += "but the balance is still there! To some degree! Maybe you should adopt a cat?";
        if(end != check)
        {
            endText.GetComponent<TMP_Text>().text = end;
        }
        
        StartCoroutine("HideCanvas", 3.0f);
        StartCoroutine("MoveToEnd", 4.2f);

    }

    void EndGame()
    {
        gameState = GameStates.ending;
        questionText.GetComponent<TMP_Text>().text = "Oh. Dear.";

        // Hide GUI
        StartCoroutine("HideCanvas", 3.0f);
        StartCoroutine("MoveToEnd", 4.2f);
    }

    IEnumerator MoveToEnd(float delay){
        yield return new WaitForSeconds(delay);
        CameraController.Instance.MoveToNextAnchor(cameraAnchors[2]);
    }

    bool CheckEnd()
    {
        // check all bar values against the limit
        for (int i = 0; i < gameBars.Length; i++)
        {
            if (gameBars[i] <= -limit || gameBars[i] >= limit)
            {
                EndGame();
                // Update the ending text accordingly
                Debug.Log("ending " + i);
                Debug.Log(gameBars[i]);
                if (i == 0 && gameBars[i] > 0)
                {
                    endText.GetComponent<TMP_Text>().text = endings.EndingText(0);
                }
                if (i == 0 && gameBars[i] < 0)
                {
                    endText.GetComponent<TMP_Text>().text = endings.EndingText(1);
                }
                if (i == 1 && gameBars[i] > 0)
                {
                    endText.GetComponent<TMP_Text>().text = endings.EndingText(2);
                }
                if (i == 1 && gameBars[i] < 0)
                {
                    endText.GetComponent<TMP_Text>().text = endings.EndingText(3);
                }
                if (i == 2 && gameBars[i] > 0)
                {
                    endText.GetComponent<TMP_Text>().text = endings.EndingText(4);
                }
                if (i == 2 && gameBars[i] < 0)
                {
                    endText.GetComponent<TMP_Text>().text = endings.EndingText(5);
                }
                return true;
            } 
        }
        return false;

    }

}