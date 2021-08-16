using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    bool triggered = false;
    
    private void ResetButton() 
    {
        if(!triggered) {
            triggered = true;
            StartCoroutine("ResetLevel");    
        }
        
    }

    private IEnumerator ResetLevel(){

        GameController.Instance.GoToVeryEnd();
        yield return new WaitForSeconds(1f);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void OnMouseDown() {
        ResetButton();
    }
}
