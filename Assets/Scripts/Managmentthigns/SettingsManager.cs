using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public GameManager gm; 
    public InputManager _input;
    public Settings settingsScript;
    public DebugStore debugStoreScript;
    public ConfirmationPopUpManager popUpManager;
    public bool isPaused = false;

    public MainMenu mainMenuScript;
    public Sound summonSound, deSummonSound;

    public bool unPaused;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // pause input or if we are already paused and they press back button)
        if (gm._input.pause || (isPaused && gm._input.goBack))
        {
            gm._input.pause = false;
            gm._input.dash = false;
            gm._input.jump = false; 
            gm._input.jumpHold = false;
            gm._input.goBack = false;
            print($"{gm._input.jump}");
            // if in pause menu then remove it
            if (settingsScript != null && settingsScript.isInOptions)
            {
                settingsScript.DesummonOptionsMenu();
            }
            // else if in the debugStore then remove it
            else if (debugStoreScript != null && debugStoreScript.isInDebugStore)
            {
                debugStoreScript.DesummonDebugMenu();
            }
            // else remove the pause menu
            else
            {
                ChangePauseMenuState();
            }
            settingsScript.ReselectButton();
        }

        if (unPaused)
        {
            gm.TurnOffPlayerJump();
        }
        //print($"{gm._input.jump}");
    }

    public void ChangePauseMenuState()
    {
        if (!isPaused)
        {
            OpenPauseMenu();
            isPaused = true;
        }
        else if (mainMenuScript.DestroyAreYouSure())
        {

        }
        else
        {

            ClosePauseMenu();
            isPaused = false;
            //if (settingsScript != null)
            //{
            //    settingsScript.DesummonOptionsMenu();
            //}
            //if (debugStoreScript != null)
            //{
            //    debugStoreScript.DesummonDebugMenu();
            //}
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        gm.TurnOnMouse();
        gm.TurnOffTime();
        gm.TurnOffCamerControl();
        gm.TurnOffPlayerMovement();
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        gm.TurnOffMouse();
        gm.TurnOnTime();
        gm.TurnOnCamerControl();
        gm.TurnOnPlayerMovement();
        gm.TurnOffPlayerJump();

        print($"{gm._input.jump}");
        print("closed pause menu");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);
        StartCoroutine(DelayFrame());
       
    }

    public IEnumerator DelayFrame()
    {
        unPaused = true;
        yield return null;
        print("closed pause menu null");
        unPaused = false;
    }


}

