using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Questions
{
    public Question[] questions;
    public Questions(Question[] myQuestions)
    {
         questions = myQuestions;
    }
}

[System.Serializable]
public class Question{
    public int id;
    public int phase;
    public int character;
    public string title;
    public Answer[] answers;
}

[System.Serializable]
public class Answer{
    public int id;
    public string answer;
    public int[] scores;
}