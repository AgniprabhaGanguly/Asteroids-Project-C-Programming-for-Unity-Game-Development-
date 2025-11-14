using UnityEngine;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    float currentTime;
    bool isRunning;
    public TextMeshProUGUI timerText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            // Keep the first instance
            Destroy(this);
            return;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = 0;
        isRunning = false;
        StartStopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }
    
    void UpdateTimerDisplay()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", time.Minutes, time.Seconds, time.Milliseconds / 10); // MM:SS.FF format
    }

    public void StartStopwatch()
    {
        isRunning = true;
    }

    public void StopStopwatch()
    {
        // stop without resetting so final time remains visible
        isRunning = false;
        UpdateTimerDisplay();
    }

    public void ResetStopwatch()
    {
        currentTime = 0.0f;
        UpdateTimerDisplay();
    }
}
