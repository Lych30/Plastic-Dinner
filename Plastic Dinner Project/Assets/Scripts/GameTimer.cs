using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private float currentTime = 0f;
    private float startingTime = 10f;
    private TMP_Text timerText;

    void Start()
    {
        timerText = GetComponentInChildren<TMP_Text>();
        currentTime = startingTime;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        timerText.text = currentTime.ToString("0");

        if(currentTime <= 0)
        {
            currentTime = 0;
        }
    }
}
