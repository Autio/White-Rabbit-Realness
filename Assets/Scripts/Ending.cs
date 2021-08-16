using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Endings
{
    public Ending[] endings;
    public Endings(Ending[] myEndings)
    {
         endings = myEndings;
    }

    public string EndingText(float id) {
        for (int i = 0; i < endings.Length; i++)
        {
            if(endings[i].id == id)
            {
                return endings[i].text;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Ending{
    public int id;
    public string desc;
    public string text;
    
}
