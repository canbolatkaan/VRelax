using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountDownTimerForBot : MonoBehaviour
{
    bool timerActive = false;
    // Start is called before the first frame update
    public static float currentTime;
    public int startMinutes;
    public Text currentTimeText;

    void Start()
    {
        currentTime = startMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive == true)
        {
            currentTime = currentTime - Time.deltaTime;
        }
        currentTimeText.text = currentTime.ToString();
    }
    public static float getCurrentTime()
    {
        return currentTime;
    }
    public void StartTimer()
    {
        timerActive = true;
    }
    public void StopTimer()
    {
        timerActive = false;
    }
    public void ResetTimer()
    {
        currentTime = 60;
    }
}