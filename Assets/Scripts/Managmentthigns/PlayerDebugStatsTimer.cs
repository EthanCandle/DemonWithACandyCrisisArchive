using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugStatsTimer : MonoBehaviour
{
    public static PlayerDebugStatsTimer Instance;

    public float timeCurrent;

    public bool isRunning = true;
    // Start is called before the first frame update
    void Awake()
    {
//        PlayerDebugStatsTimer.Instance
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            timeCurrent += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        // used each time
        isRunning = true;
    }

    public void PauseTimer()
    {
        // not tused
        isRunning = false;
    }

    public void ResetTimer()
    {
        // used on new game or level select
        timeCurrent = 0f;
    }



    public float GetTime()
    {
        // used to save time in global and local time
        return timeCurrent;
    }

    public void SetTimer(float timeToSet)
    {
        // used for normal play (set timer with current time to continue)
        timeCurrent = timeToSet;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(timeCurrent / 60f);
        int seconds = Mathf.FloorToInt(timeCurrent % 60f);
        int milliseconds = Mathf.FloorToInt((timeCurrent * 1000f) % 1000);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

}
