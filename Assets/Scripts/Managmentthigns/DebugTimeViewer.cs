using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DebugTimeViewer : MonoBehaviour
{
    public TextMeshProUGUI levelTimerText, timeTimerText;
    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        timeTimerText.text = $"Time: {GetFormattedTime(PlayerDebugStatsTimer.Instance.GetTime())}";
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
        levelTimerText.text = $"Time : {GetFormattedTime(PlayerDebugStatsTimer.Instance.GetTime())}\n";
        for (int i = 0; i < levelTimes.Count; i++)
        {
            levelTimerText.text += $"Level {i}: {GetFormattedTime(levelTimes[i])}\n";
        }
    }
    public string GetFormattedTime(float timeCurrent)
    {
        int minutes = Mathf.FloorToInt(timeCurrent / 60f);
        int seconds = Mathf.FloorToInt(timeCurrent % 60f);
        int milliseconds = Mathf.FloorToInt((timeCurrent * 1000f) % 1000);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }
}
