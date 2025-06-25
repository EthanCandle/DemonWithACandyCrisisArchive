using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDebugStatsGlobalShower : MonoBehaviour
{
    public TextMeshProUGUI statsText;
    public string candyCollectString = "\nCandy Collected : ", timeTakenString = "\nTime Taken: ", totalDeathsString = "\nTotal Deaths: ", totalDashesString = "\nTotal Dashes: ", totalJumpsString = "\nTotal Jumps: ";
    public PlayerDebugStatsGlobalManager playerStatsManager;

    // Start is called before the first frame update
    void Start()
    {
        playerStatsManager = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerDebugStatsGlobalManager>();

        SetText();
        Cursor.lockState = CursorLockMode.None;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText()
    {
        statsText.text = $"{candyCollectString}{PlayerDebugStatsGlobalManager.Instance.DataGetCandy()}" +
            $"{timeTakenString}{PlayerDebugStatsGlobalManager.Instance.DataGetTimeCompleteWholeGame()}" +
            $"{totalDeathsString}{PlayerDebugStatsGlobalManager.Instance.DataGetDies()}" +
            $"{totalDashesString}{PlayerDebugStatsGlobalManager.Instance.DataGetDash()}" +
            $"{totalJumpsString}{PlayerDebugStatsGlobalManager.Instance.DataGetJumps()}";

    }
    // 4:15-8
}
