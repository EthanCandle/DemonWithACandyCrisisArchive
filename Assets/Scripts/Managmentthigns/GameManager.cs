using System.Collections;
using System.Collections.Generic;
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

    public InputManager _input;
    //public GameManager gm;  gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _input = playerController.gameObject.GetComponent<InputManager>();
    StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           // SwitchGames();
        }
    }

    public void StartGame()
    {
        SwitchToPlatformer();
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
