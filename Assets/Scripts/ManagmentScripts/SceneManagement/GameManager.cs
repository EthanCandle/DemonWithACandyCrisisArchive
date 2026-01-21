using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public enum GameState { Platformer, Typing }
public class GameManager : MonoBehaviour
{
    public GameState gameStateCurrent = GameState.Platformer;

    public GameObject platformerArea, typingArea;


    public FoodManager fm;
    public TypingManager tm;
    public PlayerController playerController;
    public CheckPointManager checkPointManager;
    public InputManager _input;

    //public GameManager gm;  gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    // Start is called before the first frame update
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _input = playerController.gameObject.GetComponent<InputManager>();
        checkPointManager = GetComponent<CheckPointManager>();


        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // SwitchGames();
        }
		//print(DebugStore.debugStore.GetShopItemDataLowLevelStatus("player_ui"));
	}

    public void StartGame()
    {
        SwitchToPlatformer();
    }
    public void GetInputScript()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _input = playerController.gameObject.GetComponent<InputManager>();
    }

    public void SwitchGames()
    {
        switch (gameStateCurrent)
        {
            case GameState.Platformer:
                gameStateCurrent = GameState.Typing;
                SwitchToTypingGame();
                break;
            case GameState.Typing:
                gameStateCurrent = GameState.Platformer;
                SwitchToPlatformer();
                break;
        }
    }

    public void SwitchToPlatformer()
    {
        // turn on demon girl and controls
        // turn off box girl
        platformerArea.SetActive(true);
        typingArea.SetActive(false);
        TurnOffMouse();
    }

    public void SwitchToTypingGame()
    {
        // disable demon girl
        // turn on box girl
        // turn on typing inputs
        // turn on all UI
        typingArea.SetActive(true);
        platformerArea.SetActive(false);

    }

    public void TurnOffGame()
    {
		TurnOnMouse();
	    TurnOffTime();
	    TurnOffCamerControl();
		TurnOffPlayerMovement();
	}

    public void TurnOnGame()
    {
		TurnOffMouse();
		TurnOnTime();
		TurnOnCamerControl();
		TurnOnPlayerMovement();
		TurnOffPlayerJump();
	}

    public void TurnOffPlayerUI()
	{
		playerController.playerCanvasAnimator.ResetTrigger("On");
		playerController.playerCanvasAnimator.SetTrigger("Off");
		playerController.lolipopCanvas.transform.parent.gameObject.SetActive(false);
		playerController.lolipopCanvas.transform.parent.gameObject.SetActive(false);
        StartCoroutine(DelayTurnOffPlayerUI());
		print("off");
        print($"{playerController.lolipopCanvas.activeSelf}");
	}

    public IEnumerator DelayTurnOffPlayerUI()
    {
        yield return null; yield return null;
		playerController.playerCanvasAnimator.ResetTrigger("On");
		playerController.playerCanvasAnimator.SetTrigger("Off");
        playerController.lolipopCanvas.transform.parent.gameObject.SetActive(false);
        playerController.lolipopCanvas.transform.parent.gameObject.SetActive(false);
		print("off");
		print($"{playerController.lolipopCanvas.activeSelf}");
	}

	public void TurnOnPlayerUI()
	{
        print("Turning on player UI Attempt");
        if (DebugStore.debugStore.GetShopItemDataLowLevelStatus("player_ui")){
            print("failed to turn on because player ui is off");
            return;
        }
		if (DebugStore.debugStore.GetShopItemDataLowLevelStatus("Player UI"))
		{
			print("failed to turn on because player ui is asdaoff");
			return;
		}
		print(DebugStore.debugStore.GetShopItemDataLowLevelStatus("player_ui"));
		playerController.playerCanvasAnimator.ResetTrigger("Off");
		playerController.playerCanvasAnimator.SetTrigger("On");
        playerController.lolipopCanvas.transform.parent.gameObject.SetActive(true);
        print("on");
	}

	public void GivePlayerControls()
    {
        // used in the timeline controller to give player controls after cutscene
		TurnOnCamerControl();
		TurnOnPlayerMovement();
		TurnOffPlayerJump();
	}

    public void RemovePlayerControls()
    {
		// used in the timeline controller to remove  player controls before the cutscene
		TurnOffCamerControl();
		TurnOffPlayerMovement();
	}

	public void TurnOffPlayerMovement()
    {
        playerController.LosePlayerControl();
    }

    public void TurnOnPlayerMovement()
    {
        playerController.GainPlayerControl();
    }

    public void TurnOffPlayerJump()
    {
        playerController._input.jump = false;
    }
    public void TurnOffMouse()
    {
        _input.TurnOffMouse();
    }

    public void TurnOnMouse()
    {
        _input.TurnOnMouse();
    }

    public void TurnOffTime()
    {
        Time.timeScale = 0;
    }

    public void TurnOnTime()
    {
        Time.timeScale = 1;
    }

    public void TurnOffCamerControl()
    {
        playerController.LoseCameraControl();
    }
    public void TurnOnCamerControl()
    {
        playerController.GainCameraControl();
    }
}
