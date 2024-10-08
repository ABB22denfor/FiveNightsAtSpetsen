using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float minutes = 10;

    TextMeshProUGUI timerText;
    float timeRemaining;
    bool timerRunning = false;

    void Start()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
        timeRemaining = minutes * 60;
        timerRunning = true;
    }

    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining > 0)
            {
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                OnTimerEnd();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutesRemaining = Mathf.FloorToInt(timeRemaining / 60);
        int secondsRemaining = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutesRemaining, secondsRemaining);
    }

    void OnTimerEnd()
    {
        EventsManager.Instance.teacherEvents.PlayerCaptured();
    }
}
