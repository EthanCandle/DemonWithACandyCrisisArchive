using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDebugStatsGlobalShowerEndMenu : MonoBehaviour
{
    public TextMeshProUGUI statsText, levelTimeText;

    [TextArea]
    public string timeTakenString = "\nTime Taken: ", candyCollectString = "\nCandy Collected : ", totalDeathsString = "\nTotal Deaths: ", totalDashesString = "\nTotal Dashes: ", totalJumpsString = "\nTotal Jumps: ";
    public PlayerDebugStatsGlobalManager playerStatsManager;

    // Start is called before the first frame update
    void Start()
    {
        playerStatsManager = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerDebugStatsGlobalManager>();
        SetTextBoth();

        Cursor.lockState = CursorLockMode.None;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetTextBoth()
    {
        SetText();
        SetLevelText();
    }

    public void SetText()
    {
        statsText.text = $"{timeTakenString}{GetFormattedTime(PlayerDebugStatsGlobalManager.Instance.DataGetTimeCompleteWholeGame())}";

    }

    public void SetLevelText()
    {
        levelTimeText.text = "";
        for (int i = 0; i < PlayerDebugStatsGlobalManager.Instance.levelCount; i++)
        {
            levelTimeText.text += $"Level {i + 1}: {GetFormattedTime(PlayerDebugStatsGlobalManager.Instance.DataGetLevelCompleteCurrentTime(i))}\n";
        }
    }

    public string GetFormattedTime(float timeToConvert)
    {
        int minutes = Mathf.FloorToInt(timeToConvert / 60f);
        int seconds = Mathf.FloorToInt(timeToConvert % 60f);
        int milliseconds = Mathf.FloorToInt((timeToConvert * 1000f) % 1000);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }
}
