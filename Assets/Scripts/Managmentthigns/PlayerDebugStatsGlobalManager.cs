using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerDebugStatsGlobalManager : MonoBehaviour
{

    public static PlayerDebugStatsGlobalManager Instance;

    // this is accessable by any script, will be managed by something else
    public PlayerDebugStatsGlobal dataLocal = null;

    public int levelCount = 6;
    public string savePath;




    private void Awake()
    {
        //  PlayerDebugStatsGlobalManager.Instance.DataIncreaseDash();
        //  PlayerDebugStatsGlobalManager.Instance.dataLocal.amountPlayerDies++;
        if (transform.parent != null)
        {
           // transform.SetParent(null); // Unparent it to make it a root GameObject
        }

        if (Instance == null)
        {
            Instance = this;
            levelCount = SceneManager.sceneCountInBuildSettings - 2;
            //DontDestroyOnLoad(gameObject);
            savePath = Application.dataPath + "/playerStats.txt";
            Load(); // Load on start
        }
        else
        {
            Instance.Save(); // saves each time a scene is loaded
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }

    public void Save()
    {
        PlayerDebugStatsGlobal dataLocalToSave;
        if (dataLocal == null)
        {
            print("is null");
            dataLocalToSave = new PlayerDebugStatsGlobal
            {
                amountPlayerDashes = 0,
                amountPlayerDies = 0,
                amountPlayerJumps = 0,
                hasBeatenGame = false,
                fastestLevelTimes = new List<float> { 0f, 0f, 0f, 0f, 0f, 0f },
                currentLevelTimes = new List<float> { 0f, 0f, 0f, 0f, 0f, 0f },
                fastestTimeToCompleteGame = 0,
                amountPlayerCandy = 0,
                isInMainRun = false,
                currentTimeToCompleteGame = 0,
                levelCurrentlyOnMainRun = 1,
            };

        }
        else
        {
            dataLocalToSave = new PlayerDebugStatsGlobal
            {
                amountPlayerDashes = dataLocal.amountPlayerDashes,
                amountPlayerDies = dataLocal.amountPlayerDies,
                amountPlayerJumps = dataLocal.amountPlayerJumps,
                hasBeatenGame = dataLocal.hasBeatenGame,
                fastestLevelTimes = dataLocal.fastestLevelTimes,
                currentLevelTimes = dataLocal.currentLevelTimes,
                fastestTimeToCompleteGame = dataLocal.fastestTimeToCompleteGame,
                amountPlayerCandy = dataLocal.amountPlayerCandy,
                isInMainRun = dataLocal.isInMainRun,
                currentTimeToCompleteGame = dataLocal.currentTimeToCompleteGame,
                levelCurrentlyOnMainRun = dataLocal.levelCurrentlyOnMainRun,
            };

        }

        string json = JsonUtility.ToJson(dataLocalToSave, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (!File.Exists(savePath))
        {
            print("it doesn't exist");
            dataLocal = null;
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
            dataLocal = null;
        }
        Load();
    }

    public void DataIncreaseDash()
    {
        dataLocal.amountPlayerDashes++;
    }

    public int DataGetDash()
    {
        return dataLocal.amountPlayerDashes;
    }


    public void DataIncreaseDies()
    {
        dataLocal.amountPlayerDies++;
    }

    public int DataGetDies()
    {
        return dataLocal.amountPlayerDies;
    }


    public void DataIncreaseJumps()
    {
        dataLocal.amountPlayerJumps++;
    }
    public int DataGetJumps()
    {
        return dataLocal.amountPlayerJumps;
    }


    public void DataIncreaseCandy()
    {
        dataLocal.amountPlayerCandy++;
    }
    public int DataGetCandy()
    {
        return dataLocal.amountPlayerCandy;
    }

    public void DataCompletedFullRun()
    {
        // called by end trigger at the last level load (need to hard code a check somewhere)
        PlayerDebugStatsTimer.Instance.PauseTimer();
        DataSetTimeCurrentToCompleteGame(PlayerDebugStatsTimer.Instance.GetTime());

        // check if current time is less then the old time, and set if it is
        if (dataLocal.currentTimeToCompleteGame < dataLocal.fastestTimeToCompleteGame || dataLocal.fastestTimeToCompleteGame <= 1.0)
        {
            DataSetTimeToCompleteWholeGame(dataLocal.currentTimeToCompleteGame);
        }
        DataSetCompletedWholeGame(true);
        ReselectDefaultButton.instance.OpenedMenuPausedButtons();
    }

    public void DataSetTimeToCompleteWholeGame(float setValue)
    {
        // called by something in only the last level
        dataLocal.fastestTimeToCompleteGame = setValue;
    }
    public float DataGetTimeCompleteWholeGame()
    {
        return dataLocal.fastestTimeToCompleteGame;
    }

    public void DataSetTimeCurrentToCompleteGame(float setValue)
    {
        dataLocal.currentTimeToCompleteGame = setValue;
    }
    public float DataGetTimeCurrentToCompleteGame()
    {
       return dataLocal.currentTimeToCompleteGame;
    }


    public void DataSetCompletedWholeGame(bool state)
    {
        dataLocal.hasBeatenGame = state;
    }

    public bool DataGetCompletedWholeGame()
    {
        return dataLocal.hasBeatenGame;
    }

    public void DataSetCompletedLevelLogic()
    {
        // sets current level num's time
        DataCompletedLevelSelectLevel(SceneManager.GetActiveScene().buildIndex-1, PlayerDebugStatsTimer.Instance.GetTime());
    }

    public void DataCompletedLevelSelectLevel(int levelNum, float timeToCheck)
    {
        print($"{levelNum}, {timeToCheck}");
        // check if current level time is less then old time, then set if it is
        // checks below 1.0 because floating rounding stuff and no level should be under 1 sec
        if (timeToCheck < dataLocal.fastestLevelTimes[levelNum] || dataLocal.fastestLevelTimes[levelNum] <= 1.0)
        {
            DataSetLevelCompleteTime(levelNum, timeToCheck);
        }


    }

    public void DataCompletedLevelSelectLevelFromMainRunCaller()
    {
        DataCompletedLevelSelectLevelFromMainRun(SceneManager.GetActiveScene().buildIndex - 1, PlayerDebugStatsTimer.Instance.GetTime());
    }
    

    public void DataCompletedLevelSelectLevelFromMainRun(int levelNum, float timeToCheck)
    {
        // time to check is the timer time

        // check if current level time is less then old time, then set if it is
        // checks below 1.0 because floating rounding stuff and no level should be under 1 sec

        // set current level's time since we can only do this once per run


        float totalTimeBeforeCurrentLevel = 0;

        for(int i = 0; i < dataLocal.levelCurrentlyOnMainRun-1; i++)
        {
            totalTimeBeforeCurrentLevel += dataLocal.currentLevelTimes[i];
        }

        print($"Total time before level: {totalTimeBeforeCurrentLevel}, Current timer: {timeToCheck} ");

        dataLocal.currentLevelTimes[levelNum] = timeToCheck - totalTimeBeforeCurrentLevel;

        if (dataLocal.currentLevelTimes[levelNum] < dataLocal.fastestLevelTimes[levelNum] || dataLocal.fastestLevelTimes[levelNum] <= 1.0)
        {
            DataCompletedLevelSelectLevel(levelNum, timeToCheck - totalTimeBeforeCurrentLevel);
        }


    }

    public float DataGetLevelCompleteCurrentTime(int levelNum)
    {

        return dataLocal.currentLevelTimes[levelNum];
    }


    public void DataSetLevelCompleteTime(int levelNum, float setValue)
    {
        dataLocal.fastestLevelTimes[levelNum] = setValue;
    }
    public float DataGetLevelCompleteTime(int levelNum)
    {

        return dataLocal.fastestLevelTimes[levelNum];
    }


    public void DataSetInMainGame(bool state)
    {
        dataLocal.isInMainRun = state;
    }
    public bool DataGetInMainGame()
    {
        return dataLocal.isInMainRun;
    }


    public void DataIncreaseLevelCount()
    {
        dataLocal.levelCurrentlyOnMainRun += 1;

        // last level is level 6 which is index 7

        // add guard to reset level back to 1 after finishing the last level
    }
    public int DataGetLevelCount()
    {
        print(dataLocal.levelCurrentlyOnMainRun);
        return dataLocal.levelCurrentlyOnMainRun;
    }
    public void DataResetLevelCount()
    {
        dataLocal.levelCurrentlyOnMainRun = 1;

        // reset current run count
        dataLocal.currentLevelTimes = new List<float> { 0f, 0f, 0f, 0f, 0f, 0f };
    }

}
