using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JSONReader : MonoBehaviour
{
    private string Path = @"Assets/Data/";

    private string filePath = Application.streamingAssetsPath;

    public Questions questions;
    public Endings endings;
    string questionsPath;
    string endingsPath;


    void Awake(){
        questionsPath = System.IO.Path.Combine(Application.streamingAssetsPath, "questions.json");
        endingsPath = System.IO.Path.Combine(Application.streamingAssetsPath, "endings.json");
    }

    void Start()
    {
        LoadFromResources();
        //StartCoroutine(GetTextFromFile());
        

    }
    IEnumerator GetTextFromFile()
    {
        print(filePath);
        if(filePath.Contains("://") || filePath.Contains(":///"))
        {
            bool successful = true;
            Debug.LogError("Loading questions");
            UnityWebRequest wr = UnityWebRequest.Get(questionsPath);
            yield return wr.SendWebRequest();
            if (wr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Network error");

            }
            if (wr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("HTTP Error");
            }            

            string txt = wr.downloadHandler.text;
            Debug.LogError(txt);
            questions = JsonUtility.FromJson<Questions>(txt);
            Debug.LogError(questions);
            Debug.LogError(questions.questions[0]);

                if (wr.error != null)
                {
                    successful = false;
                }
                else{
                    Debug.Log(wr.result);
                    successful = true;
                    
                }
            
            UnityWebRequest endingsRequest = UnityWebRequest.Get(endingsPath);
            yield return endingsRequest.SendWebRequest();
            endings = JsonUtility.FromJson<Endings>(endingsRequest.downloadHandler.text);
            Debug.Log(endings);
                if (endingsRequest.error != null)
                {
                    successful = false;
                }
                else{
                    Debug.Log(wr.result);
                    successful = true;
                    
                }        
        }
        else
        {     
            // Local files won't work on WebGL 
            string json = File.ReadAllText(Path + "Questions.json");
            Debug.Log(json);
            questions = JsonUtility.FromJson<Questions>(json);
            //Debug.Log(questions[0].title);
            string endingsJson = File.ReadAllText(Path + "Endings.json");
            endings = JsonUtility.FromJson<Endings>(endingsJson);
        }   
    }

    void LoadFromResources()
    {
        var jsonQuestions = Resources.Load<TextAsset>("Questions");
        var jsonEndings = Resources.Load<TextAsset>("Endings");

        questions = JsonUtility.FromJson<Questions>(jsonQuestions.ToString());
        endings = JsonUtility.FromJson<Endings>(jsonEndings.ToString());

    }
}