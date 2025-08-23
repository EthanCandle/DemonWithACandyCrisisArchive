using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public LevelTransitionManager levelTransition;
    public PlayerDebugStatsGlobalManager playerStatManager;

    public GameObject confirmationPopup, levelSelectObject, controlsObject;
    public CanvasGroup mainMenuCG, pauseMenuCG;

    public SettingsManager settingManagerScript;
    public Settings settingsScript;
    public DebugStore debugStoreScript;
    public bool isInOptions, isInDebugStore, isInLevelSelect, isInControls;

    public InputManager _input;
    public ConfirmationPopUP areYouSureScript;
    public ConfirmationPopUpManager popUpManager;

    public ReselectDefaultButton reselectButtonScript;

    public Sound summonSound, deSummonSound;

    public Button levelSelectDefaultButton, mainMenuDefaultButton, controlsDefaultButton;

    public List<GameObject> playerUI;

    private void Awake()
    {
        levelTransition = GameObject.FindGameObjectWithTag("Transition").GetComponent<LevelTransitionManager>();
        playerStatManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<PlayerDebugStatsGlobalManager>();
        reselectButtonScript = gameObject.GetComponent<ReselectDefaultButton>();
        settingManagerScript = GetComponent<SettingsManager>();
    }


    public void PlayGame () // new game
    {
        ReselectDefaultButton.instance.ClosedMenuGoToGamePlay();
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
        ReselectDefaultButton.instance.ClosedMenuGoToGamePlay();
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

    public void ResetLevel(Button button)
    {
        // called by pause button
        // called by the button, should only reset time if not in main game
        if (!PlayerDebugStatsGlobalManager.Instance.dataLocal.isInMainRun)
        {
            PlayerDebugStatsTimer.Instance.PauseTimer();
            PlayerDebugStatsTimer.Instance.ResetTimer();
            PlayerDebugStatsTimer.Instance.StartTimer();
        }
        // if in main run do nothing
        SpawnConfirmationPopup(ResetLevelLogic, pauseMenuCG, button);
    }

    public void ResetLevelLogic()
    {
        levelTransition.MoveToDifferentLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMainMenu(Button button)
    {
        // called by pause menu button
        // called by the button
        SpawnConfirmationPopup(BackToMainMenuLogic, pauseMenuCG, button);
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
        ReselectDefaultButton.instance.ClosedMenuGoToGamePlay();
        // called by level select buttons

        if (PlayerDebugStatsGlobalManager.Instance == null)
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



    public void QuitGame (Button button)
    {
        // called by the button
        SpawnConfirmationPopup(QuitGameLogic, mainMenuCG, button);
    }

    public void QuitGameLogic()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


    public void SpawnConfirmationPopup(Action onYes, CanvasGroup canvasGroup, Button button)
    {
        // action is the function itself
        GameObject popUp = Instantiate(confirmationPopup, confirmationPopup.transform.position, confirmationPopup.transform.rotation);
        ConfirmationPopUP popUpScript = popUp.GetComponent<ConfirmationPopUP>();
        popUpScript.Show(onYes, canvasGroup,reselectButtonScript);
        ReselectDefaultButton.instance.SetPreviousButton(button);
        areYouSureScript = popUpScript;
    }


    // Update is called once per frame
    void Update()
    {
        if(_input == null)
        {
            return;
        }
        if (_input.pause || _input.goBack)
        {
            _input.pause = false;
            _input.goBack = false;

            if(DestroyAreYouSure())
            {
               
            }
            else if (settingsScript != null && settingsScript.isInOptions)
            {
                settingsScript.DesummonOptionsMenu();
            }
            // else if in the debugStore then remove it
            else if (debugStoreScript != null && debugStoreScript.isInDebugStore)
            {
                debugStoreScript.DesummonDebugMenu();
            }
            else if (isInLevelSelect)
            {
                TurnOffLevelSelect();
            }
            else if (isInControls)
            {
                TurnOffControls();
            }

        }
    }

   public void TurnOffLevelSelect()
    {
        mainMenuCG.interactable = true;
        
        isInLevelSelect = false;
        levelSelectObject.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);

        ReselectDefaultButton.instance.GoBackToPreviousButton();
    }

    public void TurnOnLevelSelect(Button button)
    {

        mainMenuCG.interactable = false;
        print("main menu not interactable");
        isInLevelSelect = true;
        levelSelectObject.gameObject.SetActive(true);
       // reselectButtonScript.SelectRandomButton();
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);

        ReselectDefaultButton.instance.SetPreviousButton(button);
        ReselectDefaultButton.instance.SetButton(levelSelectDefaultButton);

    }   
    
    public void TurnOffControls()
    {
        mainMenuCG.interactable = true;

        isInControls = false;
        controlsObject.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);

        ReselectDefaultButton.instance.GoBackToPreviousButton();
    }

    public void TurnOnControls(Button button)
    {

        mainMenuCG.interactable = false;
        print("main menu not interactable");
        isInControls = true;
        controlsObject.gameObject.SetActive(true);
       // reselectButtonScript.SelectRandomButton();
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);

        ReselectDefaultButton.instance.SetPreviousButton(button);
        ReselectDefaultButton.instance.SetButton(controlsDefaultButton);

    }

    public bool DestroyAreYouSure()
    {
        if (areYouSureScript != null)
        {
            areYouSureScript.CloseOnNo();
            return true;
        }
        return false;
    }

    public void TogglePlayerUI(bool state)
    {
        for(int i = 0; i < playerUI.Count; i++)
        {
            playerUI[i].SetActive(state);
        }
    }


}
