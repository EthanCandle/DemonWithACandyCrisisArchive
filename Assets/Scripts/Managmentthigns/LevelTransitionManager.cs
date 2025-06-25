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
        
    }
    
    public void DoNotPlayTransition()
    {
        UnPauseTime();
        levelTransitionUIHolder.SetActive(false);
    }

    public void PlayTransitionOut()
    {
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(goingOutSFX);
        PauseTime();
        levelTransitionUIHolder.SetActive(true);
        levelTransAnimator.Play("GoingOut");
        audioManager.FadeIn();
    }

    public void PlayTransitionIn(int levelNum = 0)
    {
       
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
    }

    IEnumerator WaitToLoadNextScene(float timeToWait, int levelToGoTo)
    {

        yield return new WaitForSecondsRealtime(timeToWait);
        LoadLevel(levelToGoTo);
    }

    public void StartGame()
    {
        PlayTransitionOut();
    }

    public void EndGame()
    {
        print("End game");
        // set level time



        // called when hitting a end trigger by player 



        // called by the level transition
        // return to main menu if its not in the main run
        if (PlayerDebugStatsGlobalManager.Instance.dataLocal.isInMainRun)
        {
            PlayerDebugStatsGlobalManager.Instance.DataCompletedLevelSelectLevelFromMainRunCaller();
            // this should try to set the best time in the level select
            print("INcrease level");
            PlayerDebugStatsGlobalManager.Instance.DataIncreaseLevelCount();

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
