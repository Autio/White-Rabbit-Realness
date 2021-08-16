using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    private string Path = @"Assets/Data/";

    public Questions questions;
    public Endings endings;

    void Start()
    {
        string json = File.ReadAllText(Path + "Questions.json");
        Debug.Log(json);
        questions = JsonUtility.FromJson<Questions>(json);
        //Debug.Log(questions[0].title);
        string endingsJson = File.ReadAllText(Path + "Endings.json");
        endings = JsonUtility.FromJson<Endings>(endingsJson);
    }
}