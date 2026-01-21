using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<GameObject> checkPointObjectList;
    public Vector3 initialPosition;
    public PlayerController playerController;
    public bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive || HTMLPlatformUtil.IsEditor())
        {
            for (int i = 1; i <= 9; i++)
            {
                // KeyCodes are just a enum, so adding 1 goes down the line of which one it checks
                if (Input.GetKeyDown(KeyCode.Alpha1 + (i - 1)))
                {
                    if (i == 1)
                    {
                        // Alpha1 sends player back to spawn
                        playerController.SetPlayerPositionBackToSpawn();
                    }
                    else
                    {
                        // Other numbers send player to checkpoint (i-2)
                        SetPlayerPositionToCheckPoint(i - 2);
                    }
                    break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                // its 8 because its index 0 and we add players initial one
                SetPlayerPositionToCheckPoint(8); // 0 maps to checkpoint index 8
            }

        }
    }


    public void SetPlayerPositionToCheckPoint(int checkPointNumber)
    {
        if(checkPointNumber >= checkPointObjectList.Count)
        {
            print($"Checkpoint number too big: {checkPointNumber} compared to amount of checkpoints: {checkPointObjectList.Count}");
            return;
        }

        playerController.SetPlayerPosition(checkPointObjectList[checkPointNumber]);
    }

}
