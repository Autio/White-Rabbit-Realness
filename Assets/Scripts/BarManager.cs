using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    public string barName;
    public event Action<float> OnBarPctChanged = delegate {};
    public float current;
    private float max = 5;

    public void ModifyLevel(float amount) {
        current += amount;
        float currentLevelPct = current / max; 
        OnBarPctChanged(currentLevelPct);
    }      

    public void SetCurrentLevel(float level)
    {
        current = (int) level;
    }

    public float GetCurrentLevel()
    {
        return current;
    }

}
