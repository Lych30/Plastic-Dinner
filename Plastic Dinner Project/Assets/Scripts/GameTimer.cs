using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public bool waitBeforeStartTimer = true;
    public float delay = 1.0f;
    public MenuManager menuManager;
    private bool _isNotPlayedYet = true;

    [SerializeField]
    public float timerDuration;
    
    [SerializeField]
    private bool countDown = true;

    public float timer;
    [SerializeField]
    private TextMeshProUGUI firstMinute;
    [SerializeField]
    private TextMeshProUGUI secondMinute;
    [SerializeField]
    private TextMeshProUGUI separator;
    [SerializeField]
    private TextMeshProUGUI firstSecond;
    [SerializeField]
    private TextMeshProUGUI secondSecond;

    private void Awake()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        if (countDown)
        {
            timer = timerDuration;
        }
        else
        {
            timer = 0;
        }
        SetTextDisplay(true);
    }

    private void Start()
    {
        timer = timerDuration;
        UpdateTimerDisplay(timer);
    }
    void Update()
    {
        // for ui/ux purpose
        if(waitBeforeStartTimer)
        {
            delay -= Time.deltaTime;
            if(delay <= 0)
            {
                waitBeforeStartTimer = false;
            }
            return;
        }
        // -------

        if(timer <= 15 && _isNotPlayedYet)
        {
            SoundManager.Instance.PlaySound("Clock");
            _isNotPlayedYet = false;
        }

        if (countDown && timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay(timer);
        }
        else if (!countDown && timer < timerDuration)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay(timer);
        }

        if(timer <= 0)
        {
            EndGame();
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        if (time < 0)
        {
            time = 0;
        }

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}{01:00}", minutes, seconds);
        firstMinute.text = currentTime[0].ToString();
        secondMinute.text = currentTime[1].ToString();
        firstSecond.text = currentTime[2].ToString();
        secondSecond.text = currentTime[3].ToString();
    }

    private void SetTextDisplay(bool enabled)
    {
        firstMinute.enabled = enabled;
        secondMinute.enabled = enabled;
        separator.enabled = enabled;
        firstSecond.enabled = enabled;
        secondSecond.enabled = enabled;
    }

    private void EndGame()
    {   
        menuManager.GameOver();
    }
}