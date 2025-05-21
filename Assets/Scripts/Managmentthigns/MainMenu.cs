using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public LevelTransitionManager levelTransition;
    private void Start()
    {
        levelTransition = GameObject.FindGameObjectWithTag("Transition").GetComponent<LevelTransitionManager>();
    }


    public void PlayGame ()
    {
        levelTransition.MoveToDifferentLevel(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void ResetLevel()
    {
        levelTransition.MoveToDifferentLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        // called by pause menu button
        levelTransition.MoveToDifferentLevel(0);

    }

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }




}
