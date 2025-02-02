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
        print("Playing test sound");
        if (!shouldPlayTransition)
        {
            DoNotPlayTransition();
            audioManager.FadeIn();
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

    public void PlayTransitionIn()
    {
       
        PauseTime();
        levelTransitionUIHolder.SetActive(true);
        levelTransAnimator.Play("GoingIn");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(goingInSFX);  
        audioManager.FadeOut();
    }

    public void StartGame()
    {
        PlayTransitionOut();
    }

    public void EndGame()
    {
        PlayTransitionIn();
    }

    public void UnPauseTime()
    {
        Time.timeScale = 1;
    }

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
