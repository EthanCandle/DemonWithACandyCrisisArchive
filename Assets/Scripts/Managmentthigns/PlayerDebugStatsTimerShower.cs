using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDebugStatsTimerShower : MonoBehaviour
{
    // this shows a specific level's time
    public int levelNumToShow;
    public TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start()
    {
        SetTimeText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimeText()
    {
        //print(levelNumToShow);
        timeText.text = GetFormattedTime(PlayerDebugStatsGlobalManager.Instance.DataGetLevelCompleteTime(levelNumToShow-1));
    }

    public string GetFormattedTime(float timeToConvert)
    {
        int minutes = Mathf.FloorToInt(timeToConvert / 60f);
        int seconds = Mathf.FloorToInt(timeToConvert % 60f);
        int milliseconds = Mathf.FloorToInt((timeToConvert * 1000f) % 1000);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

}
