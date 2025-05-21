using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<GameObject> checkPointObjectList;

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetPlayerPositionToCheckPoint(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetPlayerPositionToCheckPoint(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetPlayerPositionToCheckPoint(2);
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
