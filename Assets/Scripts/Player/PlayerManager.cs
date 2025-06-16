using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // This package contains the third person player using the cinemachine player
    // Should just be able to drag and drop the PlayerHolder ThirdPerson prefab onto the scene
    // you do have to download the Input System under the package Manager
    // This was package was made 3/12/24 using Unity 2022.3.17f1


    // this is now being used to alter the player's stats i guess
    public PlayerController playerController;
    public GameObject trialRunner; // players line runner trail

    public float playerSprintSpeedBoost, playerWalkSpeedBoost;
    public void Awake()
    {
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
            playerController.SprintSpeed = playerController.speedSprintHolder * 1.5f;
            playerController.WalkSpeed = playerController.speedWalkHolder * 1.5f;

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
}
