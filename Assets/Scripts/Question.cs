using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    private const string Path = @"S:\repos\myProject\Assets\gameText\";
    Questions questions;

    void Start()
    {
        string json = File.ReadAllText(Path + "Questions.json");
        questions = new Questions();
        questions = JsonUtility.FromJson<Question>(json);
    }
}

[System.Serializable]
public class Questions
{
    public List<Question> questions = new List<Question>();
}

[System.Serializable]
public class Question{
    public int id;
    public int phase;
    public string question;
    public Answer[] answers;
}

[System.Serializable]
public class Answer{
    public int id;
    public string answer;
    public int[] scores;
}