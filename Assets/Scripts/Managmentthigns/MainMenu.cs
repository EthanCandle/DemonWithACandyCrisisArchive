using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public LevelTransitionManager levelTransition;
    public PlayerDebugStatsGlobalManager playerStatManager;

    public GameObject confirmationPopup;
    private void Start()
    {
        levelTransition = GameObject.FindGameObjectWithTag("Transition").GetComponent<LevelTransitionManager>();
        playerStatManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<PlayerDebugStatsGlobalManager>();

    }


    public void PlayGame () // new game
    {
        // done by nomal button
        // reset and start timer
        PlayerDebugStatsTimer.Instance.PauseTimer();
        PlayerDebugStatsTimer.Instance.ResetTimer();
        PlayerDebugStatsTimer.Instance.StartTimer();

        PlayerDebugStatsGlobalManager.Instance.DataSetInMainGame(true);

        PlayerDebugStatsGlobalManager.Instance.DataResetLevelCount();

        PlayerDebugStatsTimer.Instance.SetTimer(0);

        // change this to be 
        levelTransition.MoveToDifferentLevel(PlayerDebugStatsGlobalManager.Instance.DataGetLevelCount());
    }

    public void PlayGameContinue()
    {
        // done by nomal button
        // reset and start timer
        PlayerDebugStatsTimer.Instance.PauseTimer();
        PlayerDebugStatsTimer.Instance.ResetTimer();
        PlayerDebugStatsTimer.Instance.StartTimer();

        PlayerDebugStatsGlobalManager.Instance.DataSetInMainGame(true);

        // reset if we were past the final leve (end screen)
        if (PlayerDebugStatsGlobalManager.Instance.dataLocal.levelCurrentlyOnMainRun >= SceneManager.sceneCountInBuildSettings - 1)
        {
            print(SceneManager.sceneCountInBuildSettings - 1);
            // go back to level 1
            PlayerDebugStatsGlobalManager.Instance.DataResetLevelCount();
        }

        float totalTimeBeforeCurrentLevel = 0;

        for (int i = 0; i < PlayerDebugStatsGlobalManager.Instance.dataLocal.levelCurrentlyOnMainRun - 1; i++)
        {
            totalTimeBeforeCurrentLevel += PlayerDebugStatsGlobalManager.Instance.dataLocal.currentLevelTimes[i];
        }
        PlayerDebugStatsTimer.Instance.SetTimer(totalTimeBeforeCurrentLevel);

        // change this to be 
        levelTransition.MoveToDifferentLevel(PlayerDebugStatsGlobalManager.Instance.DataGetLevelCount());
    }

    public void ResetLevel()
    {
        // called by the button, should only reset time if not in main game
        if (!PlayerDebugStatsGlobalManager.Instance.dataLocal.isInMainRun)
        {
            PlayerDebugStatsTimer.Instance.PauseTimer();
            PlayerDebugStatsTimer.Instance.ResetTimer();
            PlayerDebugStatsTimer.Instance.StartTimer();
        }
        // if in main run do nothing
        SpawnConfirmationPopup(ResetLevelLogic);
    }

    public void ResetLevelLogic()
    {
        levelTransition.MoveToDifferentLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMainMenu()
    {
        // called by pause menu button
        // called by the button
        SpawnConfirmationPopup(BackToMainMenuLogic);
    }

    public void BackToMainMenuLogic()
    {
        // if in main run then save everything in timer, else stop
        // called by main menu button, end game button should call something else

        if (PlayerDebugStatsGlobalManager.Instance.dataLocal.isInMainRun)
        {
            PlayerDebugStatsGlobalManager.Instance.DataSetTimeCurrentToCompleteGame(PlayerDebugStatsTimer.Instance.GetTime());
        }
        PlayerDebugStatsTimer.Instance.PauseTimer();
        PlayerDebugStatsTimer.Instance.ResetTimer();
        levelTransition.MoveToDifferentLevel(0);
    }

    public void PlayLevel(int levelToPlay)
    {
        // called by level select buttons

        if(PlayerDebugStatsGlobalManager.Instance == null)
        {
            print("Instant null");
        }
        // set main game to false and reset and start the timer
        PlayerDebugStatsGlobalManager.Instance.DataSetInMainGame(false);
        PlayerDebugStatsTimer.Instance.PauseTimer();
        PlayerDebugStatsTimer.Instance.ResetTimer();
        PlayerDebugStatsTimer.Instance.StartTimer();


        // called by pause menu button
        levelTransition.MoveToDifferentLevel(levelToPlay);

    }



    public void QuitGame ()
    {
        // called by the button
        SpawnConfirmationPopup(QuitGameLogic);
    }

    public void QuitGameLogic()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


    public void SpawnConfirmationPopup(Action onYes, bool shouldKillSelf = false)
    {
        // action is the function itself
        GameObject popUp = Instantiate(confirmationPopup, confirmationPopup.transform.position, confirmationPopup.transform.rotation);
        ConfirmationPopUP popUpScript = popUp.GetComponent<ConfirmationPopUP>();
        popUpScript.Show(onYes);
    }

}
