using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalText;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    [Header("Limit Settings")]
    public bool hasLimit;
    public float timerLimit;

    [Header("Format Settings")]
    public bool hasFormat;
    public TimerFormats format;
    private Dictionary<TimerFormats, string> timeFormats = new Dictionary<TimerFormats, string>();

    public bool startTimer;

    public float finalTime;

    void Start()
    {
        timeFormats.Add(TimerFormats.Whole, "0");
        timeFormats.Add(TimerFormats.TenthDecimal, "0.0");
        timeFormats.Add(TimerFormats.HundrethsDecimal, "0.00");
    }

    void Update()
    {
        if (!startTimer)
        {
            currentTime = 0;
        }
        if (!enabled) return;

        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;

        if (hasLimit && ((countDown && currentTime <= timerLimit) || (!countDown && currentTime >= timerLimit)))
        {
            currentTime = timerLimit;
            SetTimerText();
            timerText.color = Color.red;
            enabled = false;
        }

        SetTimerText();
    }

    private void SetTimerText()
    {
        if (hasFormat)
        {
            switch (format)
            {
                case TimerFormats.MinutesSecondsMilliseconds:
                    ShowMinutesSecondsMilliseconds();
                    break;
                case TimerFormats.MinutesSeconds:
                    ShowMinutesSeconds();
                    break;
                case TimerFormats.Whole:
                    timerText.text = currentTime.ToString("0");
                    break;
                case TimerFormats.TenthDecimal:
                    timerText.text = currentTime.ToString("0.0");
                    break;
                case TimerFormats.HundrethsDecimal:
                    timerText.text = currentTime.ToString("0.00");
                    break;
            }
        }
        else
        {
            timerText.text = currentTime.ToString();
        }
    }

    private void ShowMinutesSecondsMilliseconds()
    {
        if (currentTime < 1f)
        {
                        timerText.text = (currentTime * 1000f).ToString("0");
        }
        else if (currentTime < 60f)
        {
            
            timerText.text = currentTime.ToString("0.00");
        }
        else
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            float milliseconds = (currentTime - Mathf.Floor(currentTime)) * 1000f;  // Get milliseconds
            timerText.text = string.Format("{0}:{1:00}.{2:000}", minutes, seconds, Mathf.FloorToInt(milliseconds));
        }
    }

    private void ShowMinutesSeconds()
    {
        if (currentTime < 60f)
        {
            int seconds = Mathf.FloorToInt(currentTime);
            timerText.text = seconds.ToString();
        }
        else
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        finalTime = currentTime;
        enabled = false;
        timerText.color = Color.red;
        finalText.text = timerText.text;
        SetTimerText();
    }
}

public enum TimerFormats
{
    Whole,
    TenthDecimal,
    HundrethsDecimal,
    MinutesSecondsMilliseconds,  
    MinutesSeconds               
}
