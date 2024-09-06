using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class totalScraps : MonoBehaviour
{
    [SerializeField] Text text;
    private int currentScore = 0;
    private int totalScore = 0;
    private float elapsedTime;
    private bool delay = false;

    // Start is called before the first frame update
    void Start()
    {
        totalScore = inventory.totalScraps;
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        if (delay)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / 3;
            currentScore = (int)Mathf.Lerp(currentScore, totalScore, Mathf.SmoothStep(0, 1, percentageComplete));
            text.text = currentScore.ToString();
        }
    }

    private IEnumerator Delay()
    {
        delay = false;
        yield return new WaitForSeconds(3.7f);
        delay = true;
    }
}
