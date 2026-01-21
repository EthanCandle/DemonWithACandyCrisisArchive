using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DebugTimeViewer : MonoBehaviour
{
    public TextMeshProUGUI levelTimerText, timeTimerText, fastestTimerText, debugStatsText;
    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        // updates the timer text per seond
        timeTimerText.text = $"Time: {GetFormattedTime(PlayerDebugStatsTimer.Instance.GetTime())}";
       // timeTimerText.text += $"\nTime: {GetFormattedTime(PlayerDebugStatsTimer.Instance.GetTime())}";



    }
    //using UnityEngine.SceneManagement;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        // Call your desired function here
        SetText();
    }
    public void SetText()
    {
        List<float> levelTimes = PlayerDebugStatsGlobalManager.Instance.dataLocal.currentLevelTimes;
        levelTimerText.text = $"Current Levels Time:\n";
        for (int i = 0; i < levelTimes.Count; i++)
        {
            levelTimerText.text += $"Level {i+1}: {GetFormattedTime(levelTimes[i])}\n";
        }
        levelTimerText.text += $"Final Time: {GetFormattedTime(PlayerDebugStatsGlobalManager.Instance.dataLocal.currentTimeToCompleteGame)}\n";

        List<float> fastestTimes = PlayerDebugStatsGlobalManager.Instance.dataLocal.fastestLevelTimes; 
        fastestTimerText.text = $"Fastest Levels Time:\n";
        for (int i = 0; i < fastestTimes.Count; i++)
        {
            fastestTimerText.text += $"Level {i+1}: {GetFormattedTime(fastestTimes[i])}\n";
   
        }

        fastestTimerText.text += $"Final Time: {GetFormattedTime(PlayerDebugStatsGlobalManager.Instance.dataLocal.fastestTimeToCompleteGame)}\n";

    }

    public string GetFormattedTime(float timeCurrent)
    {
        int minutes = Mathf.FloorToInt(timeCurrent / 60f);
        int seconds = Mathf.FloorToInt(timeCurrent % 60f);
        int milliseconds = Mathf.FloorToInt((timeCurrent * 1000f) % 1000);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    public void ToggleLevelTimer(bool state)
    {

        timeTimerText.gameObject.SetActive(state);
    }

    public void ToggleCurrentLevelTimer(bool state)
    {
        levelTimerText.gameObject.SetActive(state);
    }

    public void ToggleFastestLevelTimer(bool state)
    {
        fastestTimerText.gameObject.SetActive(state);
    }

    public void ToggleDebugStats(bool state)
    {
        debugStatsText.gameObject.SetActive(state);
    }
}
