using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;
    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    public BarManager barManager;

    // Start is called before the first frame update
    void Start()
    {
        barManager.OnBarPctChanged += HandlePctChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandlePctChanged(float pct) {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct) {
        float preChangePct = foregroundImage.fillAmount;
        
        float elapsed = 0f;
        float fillAmount = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            Debug.Log(fillAmount);
            foregroundImage.fillAmount = fillAmount;
            
            yield return null;
        }
        foregroundImage.fillAmount = pct;
    }
}
