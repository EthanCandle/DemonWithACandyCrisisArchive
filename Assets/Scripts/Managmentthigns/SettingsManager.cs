using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject pauseMenu, controlsObject, candyNumberObject;
    public CanvasGroup pauseMenuCanvasGroup;
    public GameManager gm; 
    public InputManager _input;
    public Settings settingsScript;
    public DebugStore debugStoreScript;
    public ConfirmationPopUpManager popUpManager;
    public bool isPaused = false, isInControls;

    public MainMenu mainMenuScript;
    public Sound summonSound, deSummonSound;

    public Button pauseMenuDefaultButton, controlsDefaultButton;
    public bool unPaused, allowedToPause = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pauseMenu.SetActive(false);
        print("Allowed to pause in state is now false");
        allowedToPause = false;
		candyNumberObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
	{
        if (HTMLPlatformUtil.IsEditor())
        {
			if (Input.GetKeyDown(KeyCode.Z))
			{
				gm.TurnOffCamerControl();

			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				gm.TurnOnCamerControl();

			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				Time.timeScale = 0;
				gm.TurnOffCamerControl();
				gm.TurnOnMouse();
			}
			if (Input.GetKeyDown(KeyCode.V))
			{
				Time.timeScale = 1;
				gm.TurnOnCamerControl();
				gm.TurnOffMouse();
			}
		}

		// Debug.LogAssertion(allowedToPause);
		// pause input or if we are already paused and they press back button)
		if (gm._input.pause || (isPaused && gm._input.goBack))
        {
            if (!allowedToPause)
            {
               // Debug.LogAssertion(allowedToPause);
                return;
            }
           //Debug.LogAssertion(allowedToPause);
            gm._input.pause = false;
            gm._input.dash = false;
            gm._input.jump = false; 
            gm._input.jumpHold = false;
            gm._input.goBack = false;
            //print($"{gm._input.jump}");
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
            else if (isInControls)
            {
                TurnOffControls();
            }
            // else remove the pause menu
            else
            {
                ChangePauseMenuState();
            }

           // settingsScript.ReselectButton();
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
        candyNumberObject.SetActive(true);
		gm.TurnOffGame();
		FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);

        ReselectDefaultButton.instance.SetPreviousButton(pauseMenuDefaultButton);
        ReselectDefaultButton.instance.SetButton(pauseMenuDefaultButton);
        ReselectDefaultButton.instance.OpenedMenuPausedButtons();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
		candyNumberObject.SetActive(false);
		gm.TurnOnGame();

		print($"{gm._input.jump}");
        print("closed pause menu");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);
        StartCoroutine(DelayFrame());

        // causes button selector to stop
        ReselectDefaultButton.instance.ClosedMenuGoToGamePlay();
    }

    public IEnumerator DelayFrame()
    {
        unPaused = true;
        yield return null;
        print("closed pause menu null");
        unPaused = false;
    }
    public void TurnOffControls()
    {
        //mainMenuCG.interactable = true;

        isInControls = false;
        controlsObject.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);
        pauseMenuCanvasGroup.interactable = true;
        ReselectDefaultButton.instance.GoBackToPreviousButton();
    }

    public void TurnOnControls(Button button)
    {

       // mainMenuCG.interactable = false;
        print("main menu not interactable");
        isInControls = true;
        controlsObject.gameObject.SetActive(true);
        // reselectButtonScript.SelectRandomButton();
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);
        pauseMenuCanvasGroup.interactable = false;
        ReselectDefaultButton.instance.SetPreviousButton(button);
        ReselectDefaultButton.instance.SetButton(controlsDefaultButton);

    }

    public void TurnOffAllowedToPause()
    {
        allowedToPause = false;
        pauseMenuCanvasGroup.interactable = false;
    }

    public void TurnOnAllowedToPause()
    {
        print("allowed to pause in function");
        
        allowedToPause = true;
        print(allowedToPause);
    }
}

