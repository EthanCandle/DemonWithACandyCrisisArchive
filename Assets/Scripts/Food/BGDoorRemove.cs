using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGDoorRemove : MonoBehaviour
{
    // public GameObject particleOnTeleport;
    public GameObject doorToRemove;

    public BoxGirlFunctions functions;
    public GameObject particlesToSummon, placeToSummon;
    private void Start()
    {
        functions = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BoxGirlFunctions>();
    }
    public void RemoveDoor()
    {
        functions.RemoveDoor(doorToRemove, particlesToSummon, placeToSummon);

    }

}
