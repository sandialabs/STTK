using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour {
    public Text timerText;
    private float secondsCount;
    private int minuteCount;
    private int hourCount;
    private bool timerOn;

    void Update()
    {
        if(timerOn)
            UpdateTimerUI();
    }


    //call this on update
    public void UpdateTimerUI()
    {
        //set timer UI
        secondsCount += Time.deltaTime;
        timerText.text = hourCount.ToString("00") + ":" + minuteCount.ToString("00") + ":" + ((int)secondsCount).ToString("00");
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 60)
        {
            hourCount++;
            minuteCount = 0;
        }
    }

    public void StartTimer()
    {
        hourCount = 0;
        minuteCount = 0;
        secondsCount = 0;
        timerOn = true;
    }
    public void StopTimer()
    {
        timerOn = false;
    }
    public string TotalTime()
    {
        int timeInSeconds = hourCount * 3600 + minuteCount * 60 + (int)secondsCount;
        return timeInSeconds.ToString();
    }
}
