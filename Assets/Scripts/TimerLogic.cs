using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerLogic : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> timerObjects;
    [SerializeField] private Color runningColor;
    [SerializeField] private Color stoppedColor;
    private float timer;
    private bool isRunning = false;

    public void AddTimerObject(TextMeshProUGUI timerObject)
    {
        timerObjects.Add(timerObject);
    }

    public void ClearTimerObjects()
    {
        timerObjects.Clear();
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    public float GetTimer()
    {
        return timer;
    }

    public void StartTimer()
    {
        isRunning = true;
        foreach (var timerObject in timerObjects)
            timerObject.color = runningColor;
    }

    public void StopTimer()
    {
        isRunning = false;
        foreach (var timerObject in timerObjects)
            timerObject.color = stoppedColor;
    }

    public void InitTimerObject(TextMeshProUGUI timerObject)
    {
        timerObject.color = isRunning ? runningColor : stoppedColor;
        timerObject.text = string.Format("{0:D2}:{1:D2}",
            Mathf.FloorToInt(timer / 60),
            Mathf.FloorToInt(timer % 60));
        AddTimerObject(timerObject);
    }

    private void UpdateTimers()
    {
        if (!isRunning) return;
        timer += Time.deltaTime;
        foreach (var timerObject in timerObjects)
        {
            timerObject.text = string.Format("{0:D2}:{1:D2}",
                Mathf.FloorToInt(timer / 60),
                Mathf.FloorToInt(timer % 60));
        }
    }

    private void Start()
    {
        ResetTimer();
        StartTimer();
    }

    private void Update()
    {
        UpdateTimers();
    }
}
