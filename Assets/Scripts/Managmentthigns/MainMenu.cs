using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public LevelTransitionManager levelTransition;

    public GameObject confirmationPopup;
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
        // called by the button
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
        levelTransition.MoveToDifferentLevel(0);
    }

    public void PlayLevel(int levelToPlay)
    {
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


    public void SpawnConfirmationPopup(Action onYes)
    {
        // action is the function itself
        GameObject popUp = Instantiate(confirmationPopup, confirmationPopup.transform.position, confirmationPopup.transform.rotation);
        ConfirmationPopUP popUpScript = popUp.GetComponent<ConfirmationPopUP>();
        popUpScript.Show(onYes);
    }

}
