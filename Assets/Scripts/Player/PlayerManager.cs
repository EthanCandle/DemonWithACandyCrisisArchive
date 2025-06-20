using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // This package contains the third person player using the cinemachine player
    // Should just be able to drag and drop the PlayerHolder ThirdPerson prefab onto the scene
    // you do have to download the Input System under the package Manager
    // This was package was made 3/12/24 using Unity 2022.3.17f1

    public bool isInMainMenu = false;
    // this is now being used to alter the player's stats i guess
    public PlayerController playerController;
    public GameObject trialRunner; // players line runner trail

    public GameObject skinCurrent;
    public List<GameObject> skinObjects;

    public float playerSprintSpeedBoost, playerWalkSpeedBoost;

    public int skinNumber = 0;
    public void Awake()
    {
        if (isInMainMenu)
        {
            return;
        }
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    public void EnableTrailRunner(bool state)
    {
        trialRunner.SetActive(state);
    }

    public void IncreasePlayerSpeed(bool state)
    {
        if (state)
        {
            playerController.SprintSpeed = playerController.speedSprintHolder * 2f;
            playerController.WalkSpeed = playerController.speedWalkHolder * 2f;

        }
        else
        {
            playerController.SprintSpeed = playerController.speedSprintHolder;
            playerController.WalkSpeed = playerController.speedWalkHolder;

        }
    }

    public void DecreasePlayerSpeed(bool state)
    {
        if (state)
        {
            playerController.SprintSpeed = playerController.speedSprintHolder / 4;
            playerController.WalkSpeed = playerController.speedWalkHolder / 4;
        }
        else
        {
            playerController.SprintSpeed = playerController.speedSprintHolder;
            playerController.WalkSpeed = playerController.speedWalkHolder;
        }
    }

    public void IncreaseDashDistance(bool state)
    {
        if (state)
        {
            playerController.dashSpeed = playerController.dashSpeedHolder * 2f;
        }
        else
        {
            playerController.dashSpeed = playerController.dashSpeedHolder;
        }
    }

    public void DecreaseDashDistance(bool state)
    {
        if (state)
        {
            playerController.dashSpeed = playerController.dashSpeedHolder / 4;
        }
        else
        {
            playerController.dashSpeed = playerController.dashSpeedHolder;
        }
    }


    public void EnableSkin(int skinToGoTo)
    {
        // remove the old skin
        skinObjects[skinNumber].SetActive(false);

        // assign number
        skinNumber = skinToGoTo;

        // set another one active
        skinObjects[skinNumber].SetActive(true);
    }


}
