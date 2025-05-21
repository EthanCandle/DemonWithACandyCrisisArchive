using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public GameManager gm; 
    public InputManager _input;
    public Settings settingsScript;

    public bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gm._input.pause)
        {
            gm._input.pause = false;

            if (settingsScript != null && settingsScript.isInOptions)
            {
                settingsScript.DesummonOptionsMenu();
            }
            else
            {
                ChangePauseMenuState();
            }

        }
    }

    public void ChangePauseMenuState()
    {
        if (!isPaused)
        {
            OpenPauseMenu();
            isPaused = true;
        }
        else
        {
            ClosePauseMenu();
            isPaused = false;
            if (settingsScript != null)
            {
                settingsScript.DesummonOptionsMenu();
            }
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        gm.TurnOnMouse();
        gm.TurnOffTime();
        gm.TurnOffCamerControl();
        gm.TurnOffPlayerMovement();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        gm.TurnOffMouse();
        gm.TurnOnTime();
        gm.TurnOnCamerControl();
        gm.TurnOnPlayerMovement();
    }

}
