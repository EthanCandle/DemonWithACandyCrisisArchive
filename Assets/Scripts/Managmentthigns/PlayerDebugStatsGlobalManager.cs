using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerDebugStatsGlobalManager : MonoBehaviour
{

    public static PlayerDebugStatsGlobalManager Instance;

    public PlayerDebugStatsGlobal dataLocal = new PlayerDebugStatsGlobal();
    public GameManager gm;
    public string savePath;

    public float elapsedTimeCurrentRun = 0f, elapsedTimeBestRun;
    public bool isRunning = true;
    public TextMeshProUGUI timerText, statsText, candyCollectedText, deathText, dashesText, jumpsText;
    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //         PlayerDebugStatsGlobalManager.Instance.dataLocal.amountPlayerDies++;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/playerStats.txt";
            Load(); // Load on start
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTimeCurrentRun += Time.deltaTime;
        }
    }

    public void Save()
    {
        PlayerDebugStatsGlobal dataLocalToSave;
        if (dataLocal == null)
        {
            dataLocalToSave = new PlayerDebugStatsGlobal {
                amountPlayerJumps = 0,
                amountPlayerDashes = 0,
                amountPlayerDies = 0,
                timeToCompleteGame = 0,
                levelCurrentlyOn = 1,
                candyCollectedDuringRun = 0,
            };

        }
        else
        {
            dataLocalToSave = new PlayerDebugStatsGlobal
            {
                amountPlayerJumps = dataLocal.amountPlayerJumps,
                amountPlayerDashes = dataLocal.amountPlayerDashes,
                amountPlayerDies = dataLocal.amountPlayerDies,
                timeToCompleteGame = dataLocal.timeToCompleteGame,
                levelCurrentlyOn = dataLocal.levelCurrentlyOn,
                candyCollectedDuringRun = dataLocal.candyCollectedDuringRun,
            };

        }

        string json = JsonUtility.ToJson(dataLocalToSave, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (!File.Exists(savePath))
        {
            Save();
        }


        string json = File.ReadAllText(savePath);
        dataLocal = JsonUtility.FromJson<PlayerDebugStatsGlobal>(json);
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }

    public void IncreaseData(string nameOfVar)
    {

    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex;
        string sceneName = scene.name;

        Debug.Log($"Loaded scene: {sceneName} (Index: {sceneIndex})");

        if(sceneIndex == dataLocal.levelCurrentlyOn)
        {

        }
        dataLocal.levelCurrentlyOn += 1; // incremenet to show next level is one wanted

        if (sceneIndex == 0)
        {
            OnMainMenu();
        }


        // Example: Stop timer if this is the final level
        if (sceneIndex == SceneManager.sceneCountInBuildSettings - 1) 
        {
            OnFinalScene();
        }
    }

    public void OnMainMenu()
    {
        // when going back to the main menu, remove the current elapsed time
        //ResetTimer();
        StopTimer();
        SetTimerText();
    }

    public void OnFinalScene()
    {
      //  gm.TurnOnMouse();
        StopTimer();

        dataLocal.levelCurrentlyOn = 0; // default level
        Debug.Log("Final time: " + GetFormattedTime());

        SetStatsText();
    }

    public void StopTimer()
    {
        // 
        isRunning = false;
    }

    public void ResetTimer()
    {
        // called when pressing the play button at the beggining (this holds the overall run timer)
        elapsedTimeCurrentRun = 0f;
        isRunning = true;
    }

    public void ResetLevelStats()
    {
        ResetTimer();
        dataLocal.levelCurrentlyOn = 1;
    }

    public void SetTimerText()
    {
        timerText.text = "Time: " + GetFormattedTime();
    }

    public string GetFormattedTime()
    {
        // used in final screen and stats
        int minutes = Mathf.FloorToInt(elapsedTimeCurrentRun / 60f);
        int seconds = Mathf.FloorToInt(elapsedTimeCurrentRun % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTimeCurrentRun * 1000f) % 1000);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    public void SetStatsText()
    {
        // statsText.text = $"Candy Collected: "
        SetTimerText();
        SetTextCandy();
        SetTextDeath();
        SetTextDash();
        SetTextJump();
    }

    public void SetTextCandy()
    {
        candyCollectedText.text = $"Candy Collected: {dataLocal.candyCollectedDuringRun}";
    }
    public void SetTextDeath()
    {
        deathText.text = $"Total Deaths: {dataLocal.candyCollectedDuringRun}";
    }
    public void SetTextDash()
    {
        dashesText.text = $"Total Dashes: {dataLocal.candyCollectedDuringRun}";
    }
    public void SetTextJump()
    {
        jumpsText.text = $"Total Jumps: {dataLocal.candyCollectedDuringRun}";
    }

}
