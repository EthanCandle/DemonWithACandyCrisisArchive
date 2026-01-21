using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
    public bool shouldPlayTransition = true;
    public GameObject levelTransitionUIHolder; // thing that holds all of the ui othertahn the canvas tiself
    public Animator levelTransAnimator;
    public Sound goingInSFX, goingOutSFX, test;

    public AudioManager audioManager;

    public TimelineController timeLineController;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        //print("Playing test sound");

        if (audioManager.inMainMenuFirstTime)
        {
            DoNotPlayTransition();
            audioManager.FadeIn();
            audioManager.inMainMenuFirstTime = false;
            return;
        }

        StartGame();

    }

    // Update is called once per frame
    void Update()
    {
      //  print(audioManager.gm.fm.debugStore.mainMenuScript.settingManagerScript.allowedToPause);

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (HTMLPlatformUtil.IsEditor())
            {
                EndGame();
            }
            //   
        }
    }
    
    public void DoNotPlayTransition()
    {
        UnPauseTime();
        levelTransitionUIHolder.SetActive(false);
    }

    public void PlayTransitionOut()
    {
        print("Going Out");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(goingOutSFX);
        PauseTime();
        levelTransitionUIHolder.SetActive(true);
        levelTransAnimator.Play("GoingOut");
        audioManager.FadeIn();

        float duration = 1;

        foreach (var clip in levelTransAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "GoingOut")
            {
                duration = clip.length;
                break;
            }
        }

        SetSettingAllowedToPause(false); 
        StartCoroutine(DelayForStartingGame(duration, true));
    }

    public void PlayTransitionIn(int levelNum = 0)
    {
        print("Trans in");
        PauseTime();
        levelTransitionUIHolder.SetActive(true);
        levelTransAnimator.Play("GoingIn");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(goingInSFX);  
        audioManager.FadeOut();

        float duration = 1;

        foreach (var clip in levelTransAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "GoingIn")
            {
                duration = clip.length;
                break;
            }
        }
        StartCoroutine(WaitToLoadNextScene(duration, levelNum));

        SetSettingAllowedToPause(false);
    }

    IEnumerator WaitToLoadNextScene(float timeToWait, int levelToGoTo)
    {

        yield return new WaitForSecondsRealtime(timeToWait);
        LoadLevel(levelToGoTo);
    }

    IEnumerator DelayForStartingGame(float timeToWait, bool state)
    {
        
        yield return new WaitForSecondsRealtime(timeToWait);
        print("Allow pausing");
        //print(state);
        SetSettingAllowedToPause(state);
    }

    public void SetSettingAllowedToPause(bool state)
    {
        if (!audioManager.gm)
        {
            return;
        }
        if (audioManager == null)
        {
            Debug.LogError("audioManager is null");
        }
        else if (audioManager.gm == null)
        {
            Debug.LogError("audioManager.gm is null");
        }
        else if (audioManager.gm.fm == null)
        {
            Debug.LogError("audioManager.gm.fm is null");
        }
        else if (audioManager.gm.fm.debugStore == null)
        {
            Debug.LogError("audioManager.gm.fm.debugStore is null");
        }
        else if (audioManager.gm.fm.debugStore.mainMenuScript == null)
        {
            Debug.LogError("audioManager.gm.fm.debugStore.mainMenuScript is null");
        }
        else if (audioManager.gm.fm.debugStore.mainMenuScript.settingManagerScript == null)
        {
            Debug.LogError("audioManager.gm.fm.debugStore.mainMenuScript.settingManagerScript is null");
        }
        print(state);
       // Debug.LogWarning(audioManager.gm.fm.debugStore.mainMenuScript.settingManagerScript.allowedToPause);
        audioManager.gm.fm.debugStore.mainMenuScript.settingManagerScript.allowedToPause = state;
       // Debug.LogWarning(audioManager.gm.fm.debugStore.mainMenuScript.settingManagerScript.allowedToPause);
       // print(state);
    }

    public void StartGame()
    {
        PlayTransitionOut();
    }

    public void EndGame()
    {
        print("End game");
        // set level time
        if (timeLineController)
        {

//			return;
        }


        // called when hitting a end trigger by player so its at the last level

        audioManager.gm.playerController.HasWonLevel();

        // called by the level transition
        // return to main menu if its not in the main run
        if (PlayerDebugStatsGlobalManager.Instance.dataLocal.isInMainRun)
        {
            PlayerDebugStatsGlobalManager.Instance.DataCompletedLevelSelectLevelFromMainRunCaller();
            // this should try to set the best time in the level select
            print($"{SceneManager.GetActiveScene().buildIndex}, {SceneManager.sceneCountInBuildSettings - 1}");
            PlayerDebugStatsGlobalManager.Instance.DataIncreaseLevelCount();

            if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2)
            {
                print("Completed full run");
                // then we finished the game
                PlayerDebugStatsGlobalManager.Instance.DataCompletedFullRun();

            }

            PlayTransitionIn(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            PlayerDebugStatsGlobalManager.Instance.DataSetCompletedLevelLogic();
            // go back to main menu and reset the timer
            Cursor.lockState = CursorLockMode.None;
            PlayTransitionIn(0);
            PlayerDebugStatsTimer.Instance.PauseTimer();
            PlayerDebugStatsTimer.Instance.ResetTimer();

        }

    }

    public void MoveToDifferentLevel(int levelNum)
    {
        PlayTransitionIn(levelNum);


    }

    public void UnPauseTime()
    {
        Time.timeScale = 1;
    }

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void LoadLevel(int levelNum)
    {

        SceneManager.LoadScene(levelNum);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
