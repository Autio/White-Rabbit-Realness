using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    private string Path = @"Assets/Data/";

    Questions questions;

    void Start()
    {
        string json = File.ReadAllText(Path + "Questions.json");
        Debug.Log(json);
        questions = JsonUtility.FromJson<Questions>(json);
        Debug.Log(questions.questions);
        Debug.Log(questions.questions[0].title);
        //Debug.Log(questions[0].title);
    }
}